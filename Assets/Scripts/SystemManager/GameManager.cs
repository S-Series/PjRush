using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    private static GamePlaySystem gamePlaySystem;

    public static int GamePlayScore;
    public static float GamePlayClearRate = 99.99f;

    public static int[] Record = new int[3] { 0, 0, 0 };
    public static int[] Rough = new int[2] { 0, 0 };
    public static int[] Lost = new int[2] { 0, 0 };
    public static int MaxSustain;
    public static bool isFullCombo;
    public static bool isAllPerfect;
    public static bool isHardGame;

    private void Awake(){
        ResetJudge();
    }
    public static void ResetJudge()
    {
        isFullCombo = true;
        isAllPerfect = true;
        MaxSustain = 0;
        GamePlayScore = 0;
        GamePlayClearRate = 0.0f;
        Record = new int[3] { 0, 0, 0 };
        Rough = new int[2] { 0, 0 };
        Lost = new int[2] { 0, 0 };

        try{
            gamePlaySystem.ResetGame();
        }
        catch { }
    }
}
