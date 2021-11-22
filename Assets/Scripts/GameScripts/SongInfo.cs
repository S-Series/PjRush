using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongInfo : MonoBehaviour
{
    SongData songData = new SongData();

    public int[] NoteCount;

    public string SongName;
    public string WhoMade;

    public float bpm;
    public float StartDelay;

    public int[] difficulty;

    public int endMs; // (60 * M + S + 5) * 1000

    public Sprite MusicJacket;

    public AudioClip GameMusic;
    public AudioClip GameMusicPerview;
    //----------------------------------
    public int[] MaxCombo;
    public int[] MaxPure;
    public int[] HighScore;
    public double[] ClearRate;
    public bool[] is_SongOwned;
    public bool[] is_Secret;

    private void Start()
    {
        newInfo();
        LoadDataFromJson();
    }

    [ContextMenu("Save")]
    public void SaveDataToJson()
    {
        string name;
        name = "PlayedData/" + SongName + ".json";

        songData.MaxCombo = MaxCombo;
        songData.MaxPure = MaxPure;
        songData.HighScore = HighScore;
        songData.ClearRate = ClearRate;
        songData.is_SongOwned = is_SongOwned;
        songData.is_Secret = is_Secret;

        string jsonData = JsonUtility.ToJson(songData, true);
        string path = Path.Combine(Application.dataPath, name);
        File.WriteAllText(path, jsonData);
    }

    [ContextMenu("Load")]
    void LoadDataFromJson()
    {
        newInfo();
        string name;
        name = "PlayedData/" + SongName + ".json";
        string path = Path.Combine(Application.dataPath, name);
        string jsonData = File.ReadAllText(path);
        songData = JsonUtility.FromJson<SongData>(jsonData);

        MaxCombo = songData.MaxCombo;
        MaxPure = songData.MaxPure;
        HighScore = songData.HighScore;
        ClearRate = songData.ClearRate;
        is_SongOwned = songData.is_SongOwned;
        is_Secret = songData.is_Secret;
    }

    private void newInfo()
    {
        songData.MaxCombo = new int[4];
        songData.MaxPure = new int[4];
        songData.HighScore = new int[4];
        songData.ClearRate = new double[4];
        songData.is_SongOwned = new bool[4];
        songData.is_Secret = new bool[4];
    }
}

[System.Serializable]
public class SongData
{
    public int[] MaxCombo;
    public int[] MaxPure;
    public int[] HighScore;
    public double[] ClearRate;
    public bool[] is_SongOwned;
    public bool[] is_Secret;
}
