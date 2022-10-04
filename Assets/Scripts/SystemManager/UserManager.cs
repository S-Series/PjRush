using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserManager : MonoBehaviour
{
    public static string s_userId;
    public static string s_userPassword;
    public static float s_userRating = 0.00f;
    public static float s_userLevelProgress = 0.00f;
    public static float s_userSpecialProgress = 0.00f;

    public class UserInfoData
    {
        public static UserInfoData data;
        public static bool s_isLogin = false;
        public static string s_userName = "Defualt";
        public static int s_userLevel;
        public static int s_userCredit;
        public static int s_userSpecialCredit;
        public static void AwakeWork(){}
    }
    public class UserOptionData
    {
        public static int s_LastMusicId = 0;
        public static int s_LastDifficulty = 0;
        public static int s_DefaultGameSpeed = 1000;
        public static int s_CharacterIndex = 0;
        public static bool s_SavingGameSpeed;
        public static int[] s_fragmentIndex = new int[5]{-1, -1, -1, -1, -1};
        public static void AwakeWork()
        {
            GameManager.s_Multiply = s_DefaultGameSpeed;
            print(s_DefaultGameSpeed);
        }
    }
    public class UserSetting
    {
        public static int ScreenResolutionIndex = 4;
        public static bool isFullScreen;
        public static int FrameRateIndex = 3;
        public static int AntiAliasingIndex;
        public static void AwakeWork()
        {
            switch (ScreenResolutionIndex)
            {
                case 00:
                    Screen.SetResolution(640, 360, isFullScreen);
                    break;

                case 01:
                    Screen.SetResolution(800, 450, isFullScreen);
                    break;

                case 02:
                    Screen.SetResolution(960, 540, isFullScreen);
                    break;

                case 03:
                    Screen.SetResolution(1120, 630, isFullScreen);
                    break;

                case 04:
                    Screen.SetResolution(1280, 720, isFullScreen);
                    break;

                case 05:
                    Screen.SetResolution(1440, 810, isFullScreen);
                    break;

                case 06:
                    Screen.SetResolution(1600, 900, isFullScreen);
                    break;

                case 07:
                    Screen.SetResolution(1760, 990, isFullScreen);
                    break;

                case 08:
                    Screen.SetResolution(1920, 1080, isFullScreen);
                    break;

                case 09:
                    Screen.SetResolution(2560, 1440, isFullScreen);
                    break;

                case 10:
                    Screen.SetResolution(3840, 2160, isFullScreen);
                    break;
            }
            print(FrameRateIndex);
            switch (FrameRateIndex)
            {
                case 0:
                    Application.targetFrameRate = 60;
                    QualitySettings.vSyncCount = 0;
                    break;

                case 1:
                    Application.targetFrameRate = 75;
                    QualitySettings.vSyncCount = 0;
                    break;

                case 2:
                    Application.targetFrameRate = 120;
                    QualitySettings.vSyncCount = 0;
                    break;

                case 3:
                    Application.targetFrameRate = 144;
                    QualitySettings.vSyncCount = 0;
                    break;

                case 4:
                    Application.targetFrameRate = 240;
                    QualitySettings.vSyncCount = 0;
                    break;

                case 5:
                    Application.targetFrameRate = 240;
                    QualitySettings.vSyncCount = 1;
                    break;

                case 6:
                    Application.targetFrameRate = 1000;
                    QualitySettings.vSyncCount = 2;
                    break;
            }
            switch (AntiAliasingIndex)
            {
                case 0:
                    QualitySettings.antiAliasing = 0;
                    break;

                case 1:
                    QualitySettings.antiAliasing = 2;
                    break;

                case 2:
                    QualitySettings.antiAliasing = 4;
                    break;

                case 3:
                    QualitySettings.antiAliasing = 8;
                    break;
            }
            ShowFps.ApplyScreenSize();
        }
    }

    private void Start()
    {
        LoadPlayerData();
        UserInfoData.AwakeWork();
        UserOptionData.AwakeWork();
        UserSetting.AwakeWork();
    }
    public static void SavePlayerData()
    {
        SaveUserData saveData = new SaveUserData();

        if (!Directory.Exists(Application.dataPath + "/_PlayData/")) 
            { Directory.CreateDirectory(Application.dataPath + "/_PlayData/"); }

        string path = Application.dataPath + "/_PlayData/Remind.json";

        saveData.LastMusicId = UserOptionData.s_LastMusicId;
        saveData.LastDifficulty = UserOptionData.s_LastDifficulty;
        saveData.DefaultGameSpeed = UserOptionData.s_DefaultGameSpeed;
        saveData.CharacterIndex = UserOptionData.s_CharacterIndex;
        saveData.SavingGameSpeed = UserOptionData.s_SavingGameSpeed;
        saveData.fragmentIndex = UserOptionData.s_fragmentIndex;

        saveData.ScreenResolutionIndex = UserSetting.ScreenResolutionIndex;
        saveData.isFullScreen = UserSetting.isFullScreen;
        saveData.FrameRateIndex = UserSetting.FrameRateIndex;
        saveData.AntiAliasingIndex = UserSetting.AntiAliasingIndex;

        File.WriteAllText(path, Utils.EncryptAES(JsonUtility.ToJson(saveData)));
    }
    public static void LoadPlayerData()
    {
        SaveUserData saveData = new SaveUserData();
        string path = Application.dataPath + "/_PlayData/Remind.json";

        if (!File.Exists(path)) return;
        try
        {
            saveData = JsonUtility.FromJson<SaveUserData>
                (Utils.DecryptAES(File.ReadAllText(path)));
        }
        catch { return; }

        UserOptionData.s_LastMusicId = saveData.LastMusicId;
        UserOptionData.s_LastDifficulty = saveData.LastDifficulty;
        UserOptionData.s_DefaultGameSpeed = saveData.DefaultGameSpeed;
        UserOptionData.s_CharacterIndex = saveData.CharacterIndex;
        UserOptionData.s_SavingGameSpeed = saveData.SavingGameSpeed;
        UserOptionData.s_fragmentIndex = saveData.fragmentIndex;

        UserSetting.ScreenResolutionIndex = saveData.ScreenResolutionIndex;
        UserSetting.isFullScreen = saveData.isFullScreen;
        UserSetting.FrameRateIndex = saveData.FrameRateIndex;
        UserSetting.AntiAliasingIndex = saveData.AntiAliasingIndex;
    }
}
class SaveUserData
{
    public int LastMusicId;
    public int LastDifficulty;
    public int DefaultGameSpeed;
    public int CharacterIndex = 0;
    public bool SavingGameSpeed;
    public int[] fragmentIndex = new int[5];
    public int ScreenResolutionIndex;
    public bool isFullScreen;
    public int FrameRateIndex;
    public int AntiAliasingIndex;
} 
