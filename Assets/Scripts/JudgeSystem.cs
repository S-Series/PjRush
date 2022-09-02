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
    public static int s_longDelay;
    public KeyCode[] inputKeycode = new KeyCode[2];
    public List<NormalNote> notes = new List<NormalNote>();
    private int noteIndex;
    private int testNoteMs = 0;
    private bool isTestAlive;
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
            JudgeApply(0);
            print("Losted");
        }
        if (noteIndex == notes.Count) { print("Dead"); isTestAlive = false; }
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
        }
        //* 퍼펙 판정
        else if (_inputMs > -45.5 && _inputMs < 45.5)
        {
            GameManager.s_PerfectJudgeCount[FastLateIndex]++;
            AnimatorJudgeEffect.SetTrigger(c_Perfect);
        }
        //* 간접 판정
        else if (_inputMs > -70.5 && _inputMs < 70.5)
        {
            GameManager.s_IndirectJudgeCount[FastLateIndex]++;
            AnimatorJudgeEffect.SetTrigger(c_Indirect);
        }
        //* 빠른 미스
        else
        {
            GameManager.s_IndirectJudgeCount[0]++;
            AnimatorJudgeEffect.SetTrigger(c_Indirect);
        }
        if (isLongJudge) { return; }
        noteIndex++;
        if (noteIndex >= notes.Count) { isTestAlive = false; }
    }
}