using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    GamePlaySystem gamePlaySystem;

    public int GamePlayScore;
    public float GamePlayClearRate;

    public int[] Record = new int[3] { 0, 0, 0 };
    public int[] Rough = new int[2] { 0, 0 };
    public int[] Lost = new int[2] { 0, 0 };
    public int MaxSustain;

    public static bool isFullCombo;
    public static bool isAllPerfect;
    public static bool isHardGame;

    private void Awake()
    {
        ResetJudge();
    }

    public void ResetJudge()
    {
        isFullCombo = true;
        isAllPerfect = true;
        MaxSustain = 0;
        GamePlayScore = 0;
        GamePlayClearRate = 0.0f;
        Record = new int[3] { 0, 0, 0 };
        Rough = new int[2] { 0, 0 };
        Lost = new int[2] { 0, 0 };

        try
        {
            gamePlaySystem.ResetGame();
        }
        catch { }
    }
}
