using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JudgeBottom : MonoBehaviour
{
    public static JudgeBottom judge_bt;

    [SerializeField]
    public List<(GameObject note, int legnth, int ms)> NoteBt;

    public int nowOnIndex5;

    private bool isLongJudge;
    private int longNoteJudgeMs;

    MainJudgeManage mainJudge;

    [SerializeField]
    Animator JudgeEffect;

    private void Awake()
    {
        judge_bt = this;
        NoteBt = new List<(GameObject note, int legnth, int ms)>();
    }

    private void Start()
    {
        nowOnIndex5 = 0;
        longNoteJudgeMs = 0;
        isLongJudge = false;
        mainJudge = MainJudgeManage.mainJudge;
    }

    private void Update()
    {
        int judge_ms;
        try
        {
            judge_ms = NoteBt[nowOnIndex5].ms - MainJudgeManage.mainJudge.GameMS;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                isLongJudge = true;
                JudgeResult(judge_ms);
            }
            else if (Input.GetKeyUp(KeyCode.Space))
            {
                isLongJudge = false;
            }
            else if (NoteBt[nowOnIndex5].legnth != 0)
            {
                int num;
                num = judge_ms - longNoteJudgeMs;

                if (isLongJudge == true)
                {
                    if (num <= 30 && num >= -85)
                    {
                        mainJudge.PerfectRushPlus++;
                        longNoteJudgeMs += 50;
                    }
                }
                else if (num < -85)
                {
                    mainJudge.Lost_Late++;
                    ComboManager.comboManager.resetCombo();
                    longNoteJudgeMs += 50;
                }

                if (longNoteJudgeMs >= NoteBt[nowOnIndex5].legnth)
                {
                    longNoteJudgeMs = 0;
                    NoteBt[nowOnIndex5].note.SetActive(false);
                    nowOnIndex5++;
                }
            }
            else if (judge_ms < -85)
            {
                isLongJudge = false;
                mainJudge.Lost_Late++;
                ComboManager.comboManager.resetCombo();
                NoteBt[nowOnIndex5].note.SetActive(false);
                nowOnIndex5++;
            }
        }
        catch { }
    }

    private void JudgeResult(int judgeMs)
    {
        if (judgeMs >= -30 && judgeMs <= 30)
        {
            mainJudge.PerfectRushPlus++;
            JudgeEffect.SetTrigger("EffectPlay");
        }
        else if (judgeMs >= -55 && judgeMs <= 55)
        {
            if (judgeMs > 0)
            {
                mainJudge.PerfectRush_Fast++;
            }
            else
            {
                mainJudge.PerfectRush_Late++;
            }
            JudgeEffect.SetTrigger("EffectPlay");
        }
        else if (judgeMs >= -85 && judgeMs <= 85)
        {
            if (judgeMs > 0)
            {
                mainJudge.GreatRush_Fast++;
            }
            else
            {
                mainJudge.GreatRush_Late++;
            }
        }
        else if (judgeMs > 85 && judgeMs <= 100)
        {
            isLongJudge = false;
            mainJudge.Lost_Fast++;
            ComboManager.comboManager.resetCombo();
        }
        else { return; }

        NoteBt[nowOnIndex5].note.SetActive(false);
        nowOnIndex5++;
    }
}
