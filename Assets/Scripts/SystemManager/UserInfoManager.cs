using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInfoManager : MonoBehaviour
{
    private static UserData userData = new UserData();
    private const string playerDataPath = "PlayedData/PlayerData.json";
    public static string userId;
    public static string userPassword;
    public int characterIndex;
    /* Character List
    0 || 
    1 || 
    */
    public static string userName = "Defualt";
    public static int userLevel = 1234;
    public static float userRating = 12.34f;
    public static float userLevelProgress = 12.34f;
    public static float userSpecialProgress = 12.34f;
    public static int userCredit;
    public static int userSpecialCredit;
    public static void SavePlayerData(){
        string path = Path.Combine(Application.dataPath, playerDataPath);
    }
    public static void LoadPlayerData(){
        string path = Path.Combine(Application.dataPath, playerDataPath);
    }
}
class UserData{
    public string dataUserName;
    public int dataUserLevel;
    public float dataUserRating;
    public float dataUserLevelProgress;
    public float dataUserSpecialProgress;
    public int dataUserCredit;
    public int dataUserSpecialCredit;
} 
