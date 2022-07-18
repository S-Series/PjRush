using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using TMPro;

public class MainSystem : MonoBehaviour
{
    public static MainSystem mainSystem;
    private const string UserDataPath = "";
    private const string Ver = "0.1";
    UserSetting userSetting = new UserSetting();
    public const string ScriptLink
        = "https://script.google.com/macros/s/AKfycbypRi5_bJGzDX_ajjD0RvCjYkwbr8ajSxvaYmJkXnlf7zd8mBDEATTwnNUPaFm6FnJmYQ/exec";
    private const string SendAlertTrigger = "Start";
    private const string EndAlertTrigger = "End";

    #region Manager System
    public static Music NowOnMusic;
    public static int GuageType;
    public static GameManager gameManager;
    public static MusicManager musicManager;
    public static InputManager inputManager;
    public static SpriteManager spriteManager;
    public static UserInfoManager userInfoManager;
    #endregion
    [SerializeField] public Animator DefualtAnimator;
    [SerializeField] public Animator[] GameAnimator;
    [SerializeField] Sprite[] RankSprite;
    [SerializeField] Sprite[] Character;
    [SerializeField] Sprite[] CharacterIcon;
    [SerializeField] TextMeshPro SystemMessage;
    [SerializeField] public static int gameSpeed;
    [SerializeField] private Animator AlertAnimator;
    [SerializeField] private TextMeshPro TmpAlertMessage;
    
    private void Awake()
    {
        /*
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            isUserOnline = false;
        }
        else
        {
            // 오픈단계에는 오프라인 버전만 출시할 예정
            // isUserOnline = true;
            isUserOnline = false;
        }
        */
        isUserOnline = false;

        UserInfoManager.userId = PlayerPrefs.GetString("id");
        // UserInfoManager.userId = "Test";
        UserInfoManager.userPassword = PlayerPrefs.GetString("pass");
        // UserInfoManager.userPassword = "TestPass";

        mainSystem = this;
        DontDestroyOnLoad(this);
        transform.localPosition = new Vector3(0, 0, 0);

        gameManager = GetComponentInChildren<GameManager>();
        musicManager = GetComponentInChildren<MusicManager>();
        inputManager = GetComponentInChildren<InputManager>();
        spriteManager = GetComponentInChildren<SpriteManager>();

        UserSetting.isBottomDisplay = true;
    }
    private void Start()
    {
        DefualtAnimator.SetTrigger("FadeIn");
        StartCoroutine(ILoadUserData());
        StartCoroutine(GameStartReady());
    }
    public int difficulty; // 0 ~ 15 계획
    public int difficultyNum; // 0 : Easy || 1 : Normal || 2 : Hard || 3 : Extra || 4 : Special
    public int gameEndMS;
    public static int UID = 100001;
    public static bool isUserOnline;
    public static bool isUserLogin;
    public string songName;
    public static string gameType = "Select";
    /// <summary>
    /// 0 || !Error (GameType Missed)
    /// 1 || NormalMode
    /// 2 || ArcadeMode - 
    /// 3 || ArcadeMode - 
    /// 4 || ArcadeMode - 
    /// </summary>
    #region  SceneChange //-------------------
    public void RunISelectScene()
    {
        GameAnimator[1].SetTrigger("ChangeIn");
        StartCoroutine(ISelectScene());
    }
    private IEnumerator ISelectScene()
    {
        while (true)
        {
            try
            {
                MusicSelectAct.musicSelect.GenerateMusicBox();
                break;
            }
            catch { }
            yield return null;
        }
        yield return new WaitForSeconds(1.0f);
        GameAnimator[1].SetTrigger("ChangeOut");
    }
    public void RunIgameStart()
    {
        GameAnimator[0].SetTrigger("LoadStart");
        StartCoroutine(IgameStart());
    }
    private IEnumerator IgameStart()
    {
        while (true)
        {
            print("Looping");
            try
            {
                //UserInfo.defualtGameSpeed = gameSpeed;
                //gameManager.managerSetKey();
                StartCoroutine(GamePlaySystem.gamePlay.ILoadDataFromJson());
                //StartCoroutine(ISaveUserData());
                break;
            }
            catch { }
            yield return null;
        }
    }
    public void RunIResultStart()
    {
        StartCoroutine(IResultStart());
    }
    private IEnumerator IResultStart()
    {
        /*while (true){
            print("Looping");
            try {
                GameResult.gameResult.ResultSetting();
                break;
            }
            catch {print("catch");}
            yield return null;
        }*/
        var wait = new WaitForSeconds(5.0f);
        yield return wait;
        GameResult.gameResult.ResultSetting();
        yield return new WaitForSeconds(1.0f);
        GameAnimator[1].SetTrigger("ChangeOut");
    }
    #endregion // ----------------------------
    private IEnumerator GameStartReady()
    {
        Music.MusicCount = 0;
        for (int i = 0; i < musicManager.transform.childCount; i++)
        {
            Music music;
            music = musicManager.transform.GetChild(i).GetComponent<Music>();
            Music.MusicList.Add(music);
            Music.MusicCount++;
        }

        Music.MusicList.Sort(delegate (Music A, Music B)
        {
            if (A.MusicID > B.MusicID) return +1;
            else if (A.MusicID < B.MusicID) return -1;
            else if (A.MusicID == B.MusicID) Debug.LogError("Same MusicID Detected!!!");
            return 0;
        });

        yield return new WaitForSeconds(2.0f);

        if (isUserOnline)
        {
            // 게임 버전 체크
            WWWForm formA = new WWWForm();
            formA.AddField("order", "version");
            formA.AddField("version", Ver);

            WWWForm formB = new WWWForm();
            formB.AddField("order", "login");
            formB.AddField("id", UserInfoManager.userId);
            formB.AddField("pass", UserInfoManager.userPassword);
            
            WWWForm formC = new WWWForm();
            formC.AddField("order", "load");
            formC.AddField("UID", MainSystem.UID);
            
            yield return ICheckVersion(formA);
            if (isUserOnline)
            {
                yield return ILogin(formB);
                yield return OnlineLoad(formC);
            }
            else
            {
                // 오프라인 모드
                yield return ILoadOfflineData();
            }            
        }
        else
        {
            // 오프라인 모드
            yield return ILoadOfflineData();
        }

        // 게임 Scene으로 전환 시작
        DefualtAnimator.SetTrigger("FadeOut");
        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene("Select");
        SystemMessage.text = "";
        while (true)
        {
            try
            {
                MusicSelectAct.musicSelect.GenerateMusicBox();
                break;
            }
            catch { }
            yield return null;
        }
        yield return new WaitForSeconds(1.0f);
        DefualtAnimator.SetTrigger("FadeIn");
    }
    // 
    // ------------------------------------------------------------------ //
    // 온라인을 위한 스크립트들
    public IEnumerator ISaveUserData()
    {
        int retry = 0;
        while (true)
        {
            try
            {
                string path = Path.Combine(Application.dataPath, UserDataPath);
                File.WriteAllText(path, JsonUtility.ToJson(userSetting));
                break;
            }
            catch
            {
                if (retry < 5) { retry++; }
                else { break; }
            }
            yield return new WaitForSeconds(1.0f);
        }
    }
    public IEnumerator ILoadUserData()
    {
        int retry = 0;
        while (true)
        {
            try
            {
                string path = Path.Combine(Application.dataPath, UserDataPath);
                string jsonData = File.ReadAllText(path);
                userSetting = JsonUtility.FromJson<UserSetting>(jsonData);
                break;
            }
            catch
            {
                if (retry < 5) { retry++; }
                else
                {
                    UserSetting.volume = new int[3] { 70, 70, 70 };
                    UserSetting.JudgeCorrection = new int[3] { 0, 0, 0 };
                    UserSetting.defualtGameSpeed = 100;
                    UserSetting.isBottomDisplay = false;
                    break;
                }
            }
            yield return new WaitForSeconds(1.0f);
        }
    }
    IEnumerator ILoadOfflineData()
    {
        for (int i = 0; i < Music.MusicCount; i++)
        {
            musicManager.LoadDataFromJson(i);
            yield return null;
        }
        UserInfoManager.LoadPlayerData();
        yield return null;
    }
    IEnumerator ICheckVersion(WWWForm form)
    {
        SystemMessage.text = "Checking Version...";
        using (UnityWebRequest www = UnityWebRequest.Post(MainSystem.ScriptLink, form))
        {
            yield return www.SendWebRequest();

            if (www.isDone)
            {
                if (www.downloadHandler.text == "true")
                {
                    isUserOnline = true;
                }
                else
                {
                    while (true)
                    {
                        if (Input.GetKeyDown(KeyCode.Return)
                        || Input.GetKeyDown(KeyCode.Space))
                        {

                        }
                        if (Input.GetKeyDown(KeyCode.Escape))
                        {

                        }
                        isUserOnline = false;
                    }
                }
            }
            else
            {
                SystemMessage.text = "Version UnAvailable";
                while (true)
                {
                    if (Input.GetKeyDown(KeyCode.None))
                    {

                    }
                }
            }
        }
        yield return new WaitForSeconds(1.0f);
    }
    IEnumerator ILogin(WWWForm form)
    {
        SystemMessage.text = "Connecting Login Server...";
        char seperator = '|';
        string[] textLines;
        using (UnityWebRequest www = UnityWebRequest.Post(ScriptLink, form))
        {
            yield return www.SendWebRequest();
            if (www.isDone)
            {
                if (www.downloadHandler.text == "false")
                {
                    PlayerPrefs.SetString("id", null);
                    PlayerPrefs.SetString("pass", null);
                    isUserLogin = false;
                    isUserOnline = false;
                }
                else
                {
                    isUserLogin = true;
                    SystemMessage.text = "Login Complete";
                    PlayerPrefs.SetString("id", UserInfoManager.userId);
                    PlayerPrefs.SetString("pass", UserInfoManager.userPassword);
                    textLines = www.downloadHandler.text.Split(seperator);
                    UserInfoManager.userRating = Convert.ToSingle(textLines[0]);
                    UserInfoManager.userName = textLines[1];
                    UserInfoManager.userCredit = Convert.ToInt32(textLines[2]);
                    UserInfoManager.userSpecialCredit = Convert.ToInt32(textLines[3]);
                    UserInfoManager.userLevel = Convert.ToInt32(textLines[4]);
                    yield return new WaitForSeconds(1.0f);
                    SystemMessage.text = "Welcome \"" + UserInfoManager.userName + "\"";
                    yield return new WaitForSeconds(2.0f);
                }
            }
            else
            {

            }
        }
    }
    IEnumerator OnlineLoad(WWWForm form)
    {
        SystemMessage.text = "Loading PlayData...";
        using (UnityWebRequest www = UnityWebRequest.Post(MainSystem.ScriptLink, form))
        {
            yield return www.SendWebRequest();
            if (www.isDone)
            {
                SystemMessage.text = "Loading Complete";
                musicManager.SetMusicInfo(www.downloadHandler.text);
            }
            else { }
        }
    }
    //* ------------------------------- 
    public static void SendAlert(string message = "")
    {

    }
    private IEnumerator ISendAlert(string message)
    {
        bool isSelectable = false;
        TmpAlertMessage.text = message;
        AlertAnimator.SetTrigger(SendAlertTrigger);
        while(!isSelectable)
        {
            if (Input.anyKeyDown) { isSelectable = true; }
            yield return null;
        }
        AlertAnimator.SetTrigger(EndAlertTrigger);
    }
}
public class UserSetting
{
    [SerializeField] private Animator AlertAnimator;
    public static int lastDate = 0;
    public static int[] volume = new int[3];
    public static int[] JudgeCorrection = new int[3];
    public static int defualtGameSpeed;
    public static bool isBottomDisplay;
}
