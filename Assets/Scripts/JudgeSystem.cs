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
    private const string c_Missed = "Missed";
    private const string c_Long = "Long";
    private const string c_Dummy = "Dummy";
    #endregion
    public static float s_bpm;
    public static float s_longDelay;
    public KeyCode[] inputKeycode = new KeyCode[2];
    public List<NormalNote> notes = new List<NormalNote>();
    public bool isTestAlive;
    private int noteIndex;
    private int testNoteMs = 0;
    private bool isLongJudge = false;
    private IEnumerator longKeep;
    [SerializeField] Animator AnimatorJudgeEffect;
    private void Awake()
    {
        longKeep = IlongKeep();
        inputKeycode = new KeyCode[2];
        inputKeycode[0] = KeyCode.T;
        inputKeycode[1] = KeyCode.Y;
    }
    private void Update()
    {
        if (!s_isTesting) { return; }
        if (!isTestAlive) { return; }
        testNoteMs = notes[noteIndex].ms - GamePlaySystem.s_gameMs;
        if (Input.GetKeyDown(inputKeycode[0]) || Input.GetKeyDown(inputKeycode[1]))
        {
            if (testNoteMs < 85.5 && testNoteMs > -75.5)
            {
                JudgeApply(testNoteMs);
            }
            else 
            {
                AnimatorJudgeEffect.SetTrigger(c_Dummy);
            }
        }
        if (testNoteMs < -70.5)
        {
            JudgeApply(-100);
        }
        if (noteIndex == notes.Count) { print("dead"); isTestAlive = false; GamePlaySystem.CheckGameEnd(); }
    }
    private IEnumerator ILongJudge(int legnth)
    {
        if (legnth == 0) { yield break; }
        for (int i = 1; i < legnth; i++)
        {
            /*if (isLongJudge) { JudgeApply(0, true);}
            else { JudgeApply(-100, true); }*/
            JudgeApply(0, true);
            yield return new WaitForSeconds(s_longDelay);
        }
    }
    private IEnumerator IlongKeep()
    {
        yield return new WaitForSeconds(s_longDelay / 500.0f);
        isLongJudge = false;
    }
    public void ActivateTest()
    {
        noteIndex = 0;
        isTestAlive = true;
    }
    public void ChangeKey(KeyCode[] _keys)
    {
        inputKeycode = _keys;
    }
    private void JudgeApply(int _inputMs, bool isLongJudge = false)
    {
        if (!isLongJudge) { StartCoroutine(ILongJudge(notes[noteIndex].legnth)); }
        int FastLateIndex;
        if (_inputMs > 0) { FastLateIndex = 0; }
        else { FastLateIndex = 1; }

        //* 세부 판정
        if (_inputMs > -22.5 && _inputMs < 22.5) 
        { 
            GameManager.s_DetailPerfectJudgeCount++;
            AnimatorJudgeEffect.SetTrigger(c_Perfect);
            GameInfoField.AddScore(0);
            ComboSystem.AddCombo(_isSemi:true);
        }
        //* 퍼펙 판정
        else if (_inputMs > -45.5 && _inputMs < 45.5)
        {
            GameManager.s_PerfectJudgeCount[FastLateIndex]++;
            AnimatorJudgeEffect.SetTrigger(c_Perfect);
            GameInfoField.AddScore(1);
            ComboSystem.AddCombo(_isPerfect:true);
        }
        //* 간접 판정
        else if (_inputMs > -70.5 && _inputMs < 70.5)
        {
            GameManager.s_IndirectJudgeCount[FastLateIndex]++;
            AnimatorJudgeEffect.SetTrigger(c_Indirect);
            GameInfoField.AddScore(2);
            ComboSystem.AddCombo();
        }
        //* 빠른 미스
        else if (_inputMs > 0 && _inputMs < 85.5)
        {
            GameManager.s_LostedJudgeCount[FastLateIndex]++;
            AnimatorJudgeEffect.SetTrigger(c_Missed);
            ComboSystem.ComboCutoff();
        }
        else
        {
            GameManager.s_LostedJudgeCount[FastLateIndex]++;
            AnimatorJudgeEffect.SetTrigger(c_Missed);
            ComboSystem.ComboCutoff();
        }
        if (isLongJudge) { return; }
        noteIndex++;
        if (noteIndex >= notes.Count) { isTestAlive = false; }
    }
}