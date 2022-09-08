using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public class MusicManager : MonoBehaviour
{
    private static Transform musicTransform;
    private static MusicSave musicSave;
    public static List<Music> musicList;
    private void Awake() 
    {
        musicList = new List<Music>();
        musicTransform = this.transform;    
    }

    public static void SaveMusic(Music music)
    {
        musicSave = new MusicSave();

        string path = "";
        path = Application.dataPath + "/" + 
            "_NoteBox/" + String.Format("{0:D4}", music.MusicID) + "/PlayData.json";

        musicSave.PerfectCount = music.PerfectCount;
        musicSave.MaxCombo = music.MaxCombo;
        musicSave.HighScore = music.HighScore;
        musicSave.isOwned = music.isOwned;
        musicSave.isSecret = music.isSecret;

        File.WriteAllText(path, JsonUtility.ToJson(musicSave, true));
    }
    public static IEnumerator ILoadMusic()
    {
        Music target;
        for (int i = 0; i < musicTransform.childCount; i++)
        {
            target = musicTransform.GetChild(i).GetComponent<Music>();
            yield return ILoadMusicForEach(target);
        }
    }
    private static IEnumerator ILoadMusicForEach(Music music)
    {
        musicSave = new MusicSave();
        MusicDefault musicDefault = new MusicDefault();

        string path = "";
        string loadPath = "";
        string loadPlayed = "";
        path = "_NoteBox/" + String.Format("{0:D4}", music.MusicID);
        loadPath = Application.dataPath + "/" + path + "/Default.json";
        loadPlayed = Application.dataPath + "/" + path + "/PlayData.json";
        musicDefault = JsonUtility.FromJson<MusicDefault>(File.ReadAllText(loadPath));
        #region Load Default Data
        music.isAvailable = musicDefault.isAvailable;
        music.MusicID = musicDefault.MusicID;
        music.LowBPM = musicDefault.LowBPM;
        music.HighBPM = musicDefault.HighBPM;
        music.MusicName = musicDefault.MusicName;
        music.MusicArtist = musicDefault.MusicArtist;
        music.Effecter = musicDefault.Effecter;
        music.Difficulty = musicDefault.Difficulty;
        music.NoteCount = musicDefault.NoteCount;
        music.status = musicDefault.status;
        music.PerfectCount = musicDefault.PerfectCount;
        music.MaxCombo = musicDefault.MaxCombo;
        music.HighScore = musicDefault.HighScore;
        music.isOwned = musicDefault.isOwned;
        music.isSecret = musicDefault.isSecret;
        #endregion
        //** Played Data
        print(loadPlayed);
        if (File.Exists(loadPlayed))
        {
            try
            {
                musicSave = JsonUtility.FromJson<MusicSave>(File.ReadAllText(loadPlayed));
                music.PerfectCount = musicSave.PerfectCount;
                music.MaxCombo = musicSave.MaxCombo;
                music.HighScore = musicSave.HighScore;
                music.isOwned = musicSave.isOwned;
                music.isSecret = musicSave.isSecret;
            }
            catch { throw new Exception("Null Played Data Exist"); }
        }
        else
        {
            print("None Exist Played File");
        }
        musicList.Add(music);
        yield return null;
    }
    [ContextMenu("Save Default MusicData")]
    private void SaveMusicDefaultData()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Music music;
            MusicDefault musicDefault = new MusicDefault();
            music = transform.GetChild(i).GetComponent<Music>();
            if (music.MusicID != i + 1) 
            { throw new Exception(String.Format("Music Id {0:D4} is not Available", i + 1)); }
            #region saveing
            musicDefault.isAvailable = music.isAvailable;
            musicDefault.MusicID = music.MusicID;
            musicDefault.LowBPM = music.LowBPM;
            musicDefault.HighBPM = music.HighBPM;
            musicDefault.MusicName = music.MusicName;
            musicDefault.MusicArtist = music.MusicArtist;
            musicDefault.Effecter = music.Effecter;
            musicDefault.Difficulty = music.Difficulty;
            musicDefault.NoteCount = music.NoteCount;
            musicDefault.status = music.status;
            musicDefault.PerfectCount = music.PerfectCount;
            musicDefault.MaxCombo = music.MaxCombo;
            musicDefault.HighScore = music.HighScore;
            musicDefault.isOwned = music.isOwned;
            musicDefault.isSecret = music.isSecret;
            musicDefault.JacketIllustrator = music.JacketIllustrator;
            #endregion
            string path = "";
            string savePath = "";
            string jsonData = JsonUtility.ToJson(musicDefault, true);
            path = "/_NoteBox/" + String.Format("{0:D4}", music.MusicID) + "/Default.json";
            savePath = Application.dataPath + path;
            print(savePath);
            File.WriteAllText(savePath, jsonData);
        }
    }
}
public class MusicDefault
{
    public bool[] isAvailable = new bool[5];
    public int MusicID;
    public float LowBPM;
    public float HighBPM;
    public string MusicName;
    public string MusicArtist;
    public string JacketIllustrator;
    public string[] Effecter = new string[5];
    public int[] Difficulty = new int[5];
    public int[] NoteCount = new int[5];
    public Music.Status status;
    public int[] PerfectCount = new int[5];
    public int[] MaxCombo = new int[5];
    public int[] HighScore = new int[5];
    public bool[] isOwned = new bool[5];
    public bool isSecret;
}
public class MusicSave
{
    public int[] PerfectCount = new int[5];
    public int[] MaxCombo = new int[5];
    public int[] HighScore = new int[5];

    public bool[] isOwned = new bool[5];
    public bool isSecret;
}
