using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemManager : MonoBehaviour
{
    public static Music selectedMusic;
    public static float gameSpeed;
    public enum GuageStatus {Normal, Hard, Dream};
    public static GuageStatus guageStatus = GuageStatus.Normal;
    public static int difficulty;
    public static int difficultyIndex;
}

public static class User
{
    public static string UserName = "";
    public static int UserLevel = 1;
    public static int UserExp = 0;
    public static int MaxUserExp()
    {
        int maxValue = 0;
        if (UserLevel >= 100) maxValue = 9999;
        else maxValue = Mathf.CeilToInt(Mathf.Sqrt(1000000 * UserLevel));
        return maxValue;
    }
}
