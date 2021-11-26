using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainSystem : MonoBehaviour
{
    public static MainSystem mainSystem;

    PlayedData playedData = new PlayedData();

    [SerializeField]
    public GamePlayed gamePlayed;

    [SerializeField]
    public List<Animator> Animator;

    [SerializeField]
    public SpriteRenderer[] SceneAnimateJacket;

    [SerializeField]
    public TextMeshPro[] SceneAnimateTMPro;

    [SerializeField]
    public Animator[] AudioFadeIn;

    [SerializeField]
    public Animator[] AudioFadeOut;

    public int difficulty; // 0 ~ 15 계획
    public int difficultyNum; // 0 : Easy || 1 : Normal || 2 : Hard || 3 : Extra || 4 : Special

    public int gameEndMS;

    public string songName;

    public double bpm;
    public double highSpeed;
    public double gameSpeed;

    public string GameType;

    private void Awake()
    {
        mainSystem = this;
    }

    void Start()
    {
        mainSystem = this;
    }

    private void Update()
    {
        if (mainSystem != this)
        {
            mainSystem = this;
        }
    }

    public void SavePlayedData()
    {
        playedData.MaxCombo = new int[4];
        playedData.MaxPure = new int[4];
        playedData.HighScore = new int[4];
        playedData.ClearRate = new double[4];
        playedData.is_SongOwned = new bool[4];
        playedData.is_Secret = new bool[4];

        string name;
        name = "PlayedData/" + songName + ".json";
        string path = Path.Combine(Application.dataPath, name);
        string jsonData = File.ReadAllText(path);
        playedData = JsonUtility.FromJson<PlayedData>(jsonData);

        int dif;
        dif = mainSystem.difficultyNum;

        if (playedData.MaxCombo[dif] < ComboManager.comboManager.maxCombo)
        playedData.MaxCombo[dif] = ComboManager.comboManager.maxCombo;

        if (playedData.MaxPure[dif] < gamePlayed.PRP)
            playedData.MaxPure[dif] = gamePlayed.PRP;

        if (playedData.HighScore[dif] < (int)ScoreManage.scoreManage.systemScore)
            playedData.HighScore[dif] = (int)ScoreManage.scoreManage.systemScore;

        /*if (playedData.ClearRate[dif] < )
            playedData.ClearRate[dif] = ;*/

        string saveJsonData = JsonUtility.ToJson(playedData, true);
        string savePath = Path.Combine(Application.dataPath, name);

        File.WriteAllText(savePath, saveJsonData);
    }
}

public class PlayedData
{
    public int[] MaxCombo;
    public int[] MaxPure;
    public int[] HighScore;
    public double[] ClearRate;
    public bool[] is_SongOwned;
    public bool[] is_Secret;
}
