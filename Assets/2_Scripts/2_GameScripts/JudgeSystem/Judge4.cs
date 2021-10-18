﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Judge4 : MonoBehaviour
{
    public static Judge4 judge4;

    [SerializeField]
    public List<(GameObject note, int legnth, int ms)> Note4;

    public int nowOnIndex4;

    private bool isLongJudge;
    private int longNoteJudgeMs;

    MainJudgeManage mainJudge;

    [SerializeField]
    Animator JudgeEffect;

    private void Awake()
    {
        judge4 = this;
        Note4 = new List<(GameObject note, int legnth, int ms)>();
    }

    private void Start()
    {
        nowOnIndex4 = 0;
        longNoteJudgeMs = 0;
        isLongJudge = false;
        mainJudge = MainJudgeManage.mainJudge;
    }

    private void Update()
    {
        int judge_ms;
        try
        {
            judge_ms = Note4[nowOnIndex4].ms - MainJudgeManage.mainJudge.GameMS;

            if (Input.GetKeyDown(KeyCode.V) || Input.GetKeyDown(KeyCode.Slash))
            {
                isLongJudge = true;
                JudgeResult(judge_ms);
            }
            else if (Input.GetKeyUp(KeyCode.V) || Input.GetKeyUp(KeyCode.Slash))
            {
                isLongJudge = false;
            }
            else if (Note4[nowOnIndex4].legnth != 0)
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

                if (longNoteJudgeMs >= Note4[nowOnIndex4].legnth)
                {
                    longNoteJudgeMs = 0;
                    Note4[nowOnIndex4].note.SetActive(false);
                    nowOnIndex4++;
                }
            }
            else if (judge_ms < -85)
            {
                isLongJudge = false;
                mainJudge.Lost_Late++;
                ComboManager.comboManager.resetCombo();
                Note4[nowOnIndex4].note.SetActive(false);
                nowOnIndex4++;
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

        Note4[nowOnIndex4].note.SetActive(false);
        nowOnIndex4++;
    }
}
