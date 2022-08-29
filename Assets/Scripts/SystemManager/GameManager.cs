using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static Music s_OnGameMusic;
    public static float s_bpm;
    public static float s_delay;
    public static int s_OnGameDifficultyIndex;

    public static bool s_isDetailPerfect;
    public static bool s_isPerfect;
    public static bool s_isMaximum;
    public static int s_DetailPerfectJudgeCount;
    public static int[] s_PerfectJudgeCount = {0, 0};
    public static int[] s_IndirectJudgeCount = {0, 0};
    public static int[] s_LostedJudgeCount = {0, 0};

    public static int s_NoteCount;
    public static int s_GameScore;

    public static void ResetGameData()
    {
        s_isDetailPerfect = true;
        s_isPerfect = true;
        s_isMaximum = true;
        s_DetailPerfectJudgeCount = 0;
        s_PerfectJudgeCount = new int[2]{0, 0};
        s_IndirectJudgeCount = new int[2]{0, 0};
        s_LostedJudgeCount = new int[2]{0, 0};
        s_GameScore = 0;
    }
}
