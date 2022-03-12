using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainSystem : MonoBehaviour
{
    public static MainSystem mainSystem;
    private const string UserDataPath = "";
    UserInfo userInfo = new UserInfo();

    private void Awake()
    {
        if (mainSystem != null)
        {
            Destroy(this);
            return;
        }
        mainSystem = this;
        DontDestroyOnLoad(this);
        transform.localPosition = new Vector3(0, 0, 0);

        UserInfo.isBottomDisplay = true;
    }
    private void Start()
    {
        gameManager = GetComponentInChildren<GameManager>();
        musicManager = GetComponentInChildren<MusicManager>();
        inputManager = GetComponentInChildren<InputManager>();
        spriteManager = GetComponentInChildren<SpriteManager>();
        StartCoroutine(ILoadUserData());
    }

    #region Manager System
    public static Music NowOnMusic;
    public static int GuageType;
    public static GameManager gameManager;
    public static MusicManager musicManager;
    public static InputManager inputManager;
    public static SpriteManager spriteManager;
    #endregion

    [SerializeField]
    public Animator[] GameAnimator;

    [SerializeField]
    Sprite[] RankSprite;
    [SerializeField]
    Sprite[] Character;
    [SerializeField]
    Sprite[] CharacterIcon;

    [SerializeField]
    public static int gameSpeed;

    public int difficulty; // 0 ~ 15 계획
    public int difficultyNum; // 0 : Easy || 1 : Normal || 2 : Hard || 3 : Extra || 4 : Special

    public int gameEndMS;

    public static int UID = 100001;

    public bool isUserOnline;
    public bool isUserLogin;

    public string songName;

    public int gameType;
    /// <summary>
    /// 0 || !Error (GameType Missed)
    /// 1 || NormalMode
    /// 2 || ArcadeMode - 
    /// 3 || ArcadeMode - 
    /// 4 || ArcadeMode - 
    /// </summary>

    public Sprite Rank(int index)
    {
        if (index == -1) return null;
        return RankSprite[index];
    }

    public Sprite getCharacter(int index)
    {
        return Character[index];
    }

    public Sprite getCharacterIcon(int index)
    {
        return CharacterIcon[index];
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
            yield return new WaitForSeconds(1.0f);
            try
            {
                //UserInfo.defualtGameSpeed = gameSpeed;
                //gameManager.managerSetKey();
                StartCoroutine(GamePlaySystem.gamePlay.ILoadDataFromJson());
                //StartCoroutine(ISaveUserData());
                break;
            }
            catch { }
        }
    }

    // ------------------------------------------------------------------ //
    public IEnumerator ISaveUserData()
    {
        int retry = 0;
        while (true)
        {
            try
            {
                string path = Path.Combine(Application.dataPath, UserDataPath);
                File.WriteAllText(path, JsonUtility.ToJson(userInfo));
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
                userInfo = JsonUtility.FromJson<UserInfo>(jsonData);
                UID = UserInfo.UID;
                break;
            }
            catch
            {
                if (retry < 5) { retry++; }
                else
                {
                    UserInfo.UID = UID;
                    UserInfo.volume = new int[3] { 70, 70, 70 };
                    UserInfo.JudgeCorrection = new int[3] { 0, 0, 0 };
                    UserInfo.defualtGameSpeed = 100;
                    UserInfo.isBottomDisplay = false;
                    break;
                }
            }
            yield return new WaitForSeconds(1.0f);
        }
    }
}

[System.Serializable]
public class UserInfo
{
    public static int UID;

    public static int[] volume = new int[3];

    public static int[] JudgeCorrection = new int[3];

    public static int defualtGameSpeed;

    public static bool isBottomDisplay;
}
