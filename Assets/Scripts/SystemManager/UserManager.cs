using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserManager : MonoBehaviour
{
    private static UserData userData = new UserData();
    private const string playerDataPath = "UserData/PlayerData.json";
    public static string s_userId;
    public static string s_userPassword;
    public static float s_userRating = 0.00f;
    public static float s_userLevelProgress = 0.00f;
    public static float s_userSpecialProgress = 0.00f;

    public static class UserInfoData
    {
        public static bool s_isLogin = false;
        public static string s_userName = "Defualt";
        public static int s_userLevel;
        public static int s_userCredit;
        public static int s_userSpecialCredit;
    }
    public static class UserOptionData
    {
        public static int? s_LastMusicId = null;
        public static int? s_LastDifficulty = null;
        public static int? s_DefaultGameSpeed = null;
        public static int? s_CharacterIndex = 0;
        public static bool s_SavingGameSpeed;
        public static int?[] s_fragmentIndex = new int?[5]{null, null, null, null, null};
    }

    public static void SavePlayerData(){
        string path = Path.Combine(Application.dataPath, playerDataPath);
    }
    public static void LoadPlayerData(){
        string path = Path.Combine(Application.dataPath, playerDataPath);
    }
}
class UserData
{
    public string dataUserName;
    public int dataUserLevel;
    public float dataUserRating;
    public float dataUserLevelProgress;
    public float dataUserSpecialProgress;
    public int dataUserCredit;
    public int dataUserSpecialCredit;
} 
