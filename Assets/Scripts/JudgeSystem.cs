using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JudgeSystem : MonoBehaviour
{
    public static int s_judgeMs;
    public static bool s_isTesting = false;
    public static float s_bpm;
    private static int s_longDelay;
    public KeyCode inputKeycode;
    public List<NormalNote> notes = new List<NormalNote>();
    private bool isLongJudge = false;
    private Thread thread;
    private IEnumerator longKeep;
    private void start()
    {
        
    }
    private void Update()
    {
        
    }
    private void LongJudge(int legnth)
    {
        Thread.Sleep(s_longDelay);
        for (int i = 1; i < legnth; i++)
        {
            if (isLongJudge) { ; }
            else { ; }
            Thread.Sleep(s_longDelay);
        }
    }
    private IEnumerator IlongKeep()
    {
        yield return new WaitForSeconds(s_longDelay / 500.0f);
        isLongJudge = false;
    }
}