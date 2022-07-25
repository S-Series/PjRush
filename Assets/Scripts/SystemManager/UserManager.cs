using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserManager : MonoBehaviour
{
    private static UserData userData = new UserData();
    private const string playerDataPath = "UserData/PlayerData.json";
    public static string userId;
    public static string userPassword;
    public static string userName = "Defualt";
    public static int userLevel = 1;
    public static int characterIndex = 0;
    public static int userCredit;
    public static int userSpecialCredit;
    public static float userRating = 0.00f;
    public static float userLevelProgress = 0.00f;
    public static float userSpecialProgress = 0.00f;

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
