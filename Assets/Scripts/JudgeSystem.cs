using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JudgeSystem : MonoBehaviour
{
    public static int s_judgeMs;
    public static bool s_isTesting = false;
    public static float s_bpm;
    public static int s_longDelay;
    public KeyCode inputKeycode;
    public List<NormalNote> notes = new List<NormalNote>();
    private int noteIndex;
    private int testNoteMs = 0;
    private bool isTestAlive;
    private bool isLongJudge = false;
    private Thread thread;
    private IEnumerator longKeep;
    [SerializeField] Animator AnimatorJudgeEffect;
    private void start()
    {
        thread = new Thread(ThreadWork);
        longKeep = IlongKeep();
    }
    private void Update()
    {
        if (!s_isTesting) { return; }
        if (Input.GetKeyDown(inputKeycode))
        {
            if (testNoteMs < 35
                && testNoteMs > -35)
            {
                // 세부 퍼펙트 판정
            }
            else if (testNoteMs < 47.5
                && testNoteMs > -47.5)
            {
                // 일반 퍼펙트 판정
            }
            else if (testNoteMs < 75
                && testNoteMs > -75)
            {
                // 간접 입력 판정
            }
            else if (testNoteMs < 100
                && testNoteMs > 0)
            {
                // 빠른 미스 판정
            }
            else 
            {
                // Dummy 입력 
            }
        }
        if (testNoteMs < -75)
        {
            // 느린 미스 판정
        }
    }
    private void ThreadWork()
    {
        while (isTestAlive)
        {
            if (noteIndex == notes.Count) { return; }
            testNoteMs = notes[noteIndex].ms - s_judgeMs;
            Thread.Sleep(1);
        }
    }
    private void LongJudge(int legnth)
    {
        Thread.Sleep(s_longDelay);
        for (int i = 1; i < legnth; i++)
        {
            if (isLongJudge) 
            { 
                
            }
            else 
            { 
                
            }
            Thread.Sleep(s_longDelay);
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
        thread.Start();
    }
}