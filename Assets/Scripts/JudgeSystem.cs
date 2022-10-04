using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JudgeSystem : MonoBehaviour
{
    public static bool s_isTesting = false;

    #region Animator Triggers
    private const string c_sPerfect = "SPerfect";
    private const string c_Perfect = "Perfect";
    private const string c_Indirect = "Near";
    private const string c_Missed = "Miss";
    private const string c_Long = "Long";
    private const string c_Exit = "Exit";
    #endregion
    public static float s_bpm;
    public static float s_longDelay;
    public KeyCode[] inputKeycode = new KeyCode[2];
    public List<NormalNote> notes = new List<NormalNote>();
    public bool isTestAlive;
    public int noteIndex;
    [SerializeField] private int testLine;
    private int testNoteMs = 0;
    [SerializeField] private bool isLongJudge = false;
    [SerializeField] Animator AnimatorJudgeEffect;
    [SerializeField] SpriteRenderer LineEffectRenderer;
    [SerializeField] SpriteRenderer JudgeTextRenderer;
    private void Awake()
    {
        inputKeycode = new KeyCode[2];
    }
    private void Update()
    {
        if (!s_isTesting) { return; }
        if (!isTestAlive) { return; }
        
        testNoteMs = notes[noteIndex].ms - GamePlaySystem.s_gameMs;

        if (Input.GetKeyDown(inputKeycode[0]) || Input.GetKeyDown(inputKeycode[1]))
        {
            LineEffectRenderer.enabled = true;
            isLongJudge = true;
            //LineEffectRenderer.enabled = true;
            if (testNoteMs < 85.5 && testNoteMs > -70.5)
            {
                // JudgeApply(0); return; //! Debugging
                if ( notes[noteIndex].isPowered )
                { 
                    if (testNoteMs < 75.5) { JudgeApply(0, _isPowered:true); }
                    else { JudgeApply(80); }
                }
                else { JudgeApply(testNoteMs); }
            }
            else { ; }
        }
        if (Input.GetKeyUp(inputKeycode[0]) || Input.GetKeyUp(inputKeycode[1]))
        {
            LineEffectRenderer.enabled = false;
            LineEffectRenderer.sprite = GamePlaySystem.gamePlaySystem.InputEffectSprite[0];
            if (!Input.GetKey(inputKeycode[0]) && !Input.GetKey(inputKeycode[1]))
            { isLongJudge = false; }
        }
        if (testNoteMs < -70.5) { JudgeApply(-100); }
        if (noteIndex == notes.Count) { isTestAlive = false; }
    }
    private IEnumerator ILongStart(int _TargetDalayMs, int _index)
    {
        while(true)
        {
            if (_TargetDalayMs < GamePlaySystem.s_gameMs)
            {
                StartCoroutine(ILongJudge(notes[_index].legnth, 
                notes[_index].noteObject.transform.GetChild(0).GetComponent<SpriteRenderer>()));
                yield break;
            }
            yield return null;
        }
    }
    private IEnumerator ILongJudge(int legnth, SpriteRenderer longSprite)
    {
        bool isAnimating = false;
        if (legnth == 0) { yield break; }
        if (testLine > 4) { longSprite.sprite = GamePlaySystem.gamePlaySystem.LongSprite[3]; }
        else { longSprite.sprite = GamePlaySystem.gamePlaySystem.LongSprite[1]; }
        yield return new WaitForSeconds(s_longDelay);
        for (int i = 1; i < legnth; i++)
        {
            if (isLongJudge)
            { 
                if (!isAnimating) 
                {
                    isAnimating = true;
                    AnimatorJudgeEffect.SetTrigger(c_Long);
                    if (testLine > 4) { longSprite.sprite = GamePlaySystem.gamePlaySystem.LongSprite[3]; }
                    else { longSprite.sprite = GamePlaySystem.gamePlaySystem.LongSprite[1]; }
                }
                JudgeApply(0, true);
                MaskMoving.ChangeBlind(testLine, true);
            }
            else
            { 
                if (isAnimating) 
                {
                    if (testLine > 4) { longSprite.sprite = GamePlaySystem.gamePlaySystem.LongSprite[2]; }
                    else { longSprite.sprite = GamePlaySystem.gamePlaySystem.LongSprite[0]; }
                }
                JudgeApply(-100, true);
                AnimatorJudgeEffect.SetTrigger(c_Missed);
                isAnimating = false;
                MaskMoving.ChangeBlind(testLine, false);
            }
            yield return new WaitForSeconds(s_longDelay);
        }
        if (isAnimating) { AnimatorJudgeEffect.SetTrigger(c_Exit); }
        yield return new WaitForSeconds(1.0f);
        longSprite.enabled = false;
    }
    public void EndGame()
    {
        LineEffectRenderer.enabled = false;
    }
    public void ActivateTest()
    {
        noteIndex = 0;
        if (notes[0].legnth != 0) { StartCoroutine(ILongStart(notes[0].ms, 0)); }
        isTestAlive = true;
    }
    public void Restart()
    {
        noteIndex = 0;
        if (notes[0].legnth != 0) { StartCoroutine(ILongStart(notes[0].ms, 0)); }
        isTestAlive = true;
    }
    public void ChangeKey(KeyCode[] _keys)
    {
        inputKeycode = _keys;
    }
    private void JudgeApply(int _inputMs, bool _isLongJudge = false, bool _isPowered = false)
    {
        //_inputMs = 0; //! Debugging Code
        GameInfoField.AddCount();

        if (!_isLongJudge) 
            { notes[noteIndex].noteObject.GetComponent<SpriteRenderer>().enabled = false; }

        int FastLateIndex;
        if (_inputMs > 0) { FastLateIndex = 0; }
        else { FastLateIndex = 1; }

        //* 세부 판정
        if (_inputMs > -22.5 && _inputMs < 22.5)
        //if (_inputMs > -75.5 && _inputMs < 75.5)
        { 
            GameManager.s_DetailPerfectJudgeCount++;
            if (!_isLongJudge) { AnimatorJudgeEffect.SetTrigger(c_Perfect); }
            GameInfoField.AddScore(0);
            ComboSystem.AddCombo(_isSemi:true);
            PlaySound(__isPowered:_isPowered, _isLong:_isLongJudge);
            LineEffectRenderer.sprite = GamePlaySystem.gamePlaySystem.InputEffectSprite[1];
            JudgeTextRenderer.sprite = GamePlaySystem.gamePlaySystem.JudgeTextSprite[0];
        }
        //* 퍼펙 판정
        else if (_inputMs > -45.5 && _inputMs < 45.5)
        {
            GameManager.s_PerfectJudgeCount[FastLateIndex]++;
            AnimatorJudgeEffect.SetTrigger(c_Perfect);
            GameInfoField.AddScore(1);
            ComboSystem.AddCombo(_isPerfect:true);
            PlaySound(__isPowered:_isPowered, _isLong:_isLongJudge);
            LineEffectRenderer.sprite = GamePlaySystem.gamePlaySystem.InputEffectSprite[1];
            JudgeTextRenderer.sprite = GamePlaySystem.gamePlaySystem.JudgeTextSprite[1];
        }
        //* 간접 판정
        else if (_inputMs > -70.5 && _inputMs < 70.5)
        {
            GameManager.s_IndirectJudgeCount[FastLateIndex]++;
            AnimatorJudgeEffect.SetTrigger(c_Indirect);
            GameInfoField.AddScore(2);
            ComboSystem.AddCombo();
            PlaySound(__isPowered:_isPowered, _isLong:_isLongJudge);
            LineEffectRenderer.sprite = GamePlaySystem.gamePlaySystem.InputEffectSprite[2];
            JudgeTextRenderer.sprite = GamePlaySystem.gamePlaySystem.JudgeTextSprite[2];
        }
        //* 빠른 미스
        else if (_inputMs > 0 && _inputMs < 85.5)
        {
            GameManager.s_LostedJudgeCount[FastLateIndex]++;
            AnimatorJudgeEffect.SetTrigger(c_Missed);
            ComboSystem.ComboCutoff();
            LineEffectRenderer.sprite = GamePlaySystem.gamePlaySystem.InputEffectSprite[3];
            JudgeTextRenderer.sprite = GamePlaySystem.gamePlaySystem.JudgeTextSprite[3];
        }
        //* 느린 미스
        else
        {
            GameManager.s_LostedJudgeCount[FastLateIndex]++;
            AnimatorJudgeEffect.SetTrigger(c_Missed);
            ComboSystem.ComboCutoff();
            JudgeTextRenderer.sprite = GamePlaySystem.gamePlaySystem.JudgeTextSprite[3];
        }
        
        if (_isLongJudge) { return; }
        noteIndex++;
        if (noteIndex >= notes.Count) { isTestAlive = false; return; }
        if (notes[noteIndex].legnth != 0) 
            { StartCoroutine(ILongStart(notes[noteIndex].ms, noteIndex)); }
    }
    private void PlaySound(bool __isPowered = false, bool _isLong = false)
    {
        if (__isPowered) {  GamePlaySystem.gamePlaySystem.judgeSound[2].Play(); }
        else if (_isLong) { GamePlaySystem.gamePlaySystem.judgeSound[1].Play(); }
        else { GamePlaySystem.gamePlaySystem.judgeSound[0].Play(); }
    }
}