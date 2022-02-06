using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MusicManager : MonoBehaviour
{
    MusicSaveData musicSave = new MusicSaveData();
    private const string Ver = "0.1";

    IEnumerator CheckVersion(WWWForm form)
    {
        using (UnityWebRequest www = UnityWebRequest.Post(ScriptLink, form))
        {
            yield return www.SendWebRequest();

            if (www.isDone)
            {
                if (www.downloadHandler.text == "true") 
                {
                    print("버전 맞음");
                    LoadPlayData();
                    yield break; 
                }
                else
                {
                    print("버전 안맞음");
                    MainSystem.mainSystem.isUserOnline = false;
                }
            }
            else
            {
                print("연결 실패");
            }
        }
    //----------------------------------------------------
    }

    public string ScriptLink;
    //----------------------------------------------------

    [SerializeField]

    private void Awake()
    {
        Music.MusicList = new List<Music>();
    }

    private void Start()
    {
        Music.MusicCount = 0;
        for (int i = 0; i < transform.childCount; i++)
        {
            Music music;
            music = transform.GetChild(i).GetComponent<Music>();
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

        if (MainSystem.mainSystem.isUserOnline)
        {
            WWWForm www = new WWWForm();
            www.AddField("order", "version");
            www.AddField("version", Ver);
            StartCoroutine(CheckVersion(www));
        }
    }

    public void SavePlayData()
    {
        // Online Save
        if (MainSystem.mainSystem.isUserOnline
            && MainSystem.UID != 0)
        {
            WWWForm www = new WWWForm();
            www.AddField("order", "save");
            www.AddField("SongID", 1 /*MusicSelectAct.musicSelect.MusicID*/);
            www.AddField("UID", MainSystem.UID);

            StartCoroutine(OnlineSave(www));
        }
        // Offline Save
        else
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                SaveDataToJson(i);
            }
        }
    }

    public void LoadPlayData()
    {
        if (MainSystem.mainSystem.isUserOnline
            && MainSystem.UID != 0)
        {
            WWWForm www = new WWWForm();
            www.AddField("order", "load");
            www.AddField("UID", MainSystem.UID);

            print(MainSystem.UID);

            StartCoroutine(OnlineLoad(www));
        }
        else
        {
            for (int i = 0; i < Music.MusicCount; i++)
            {
                LoadDataFromJson(i);
            }
        }
    }

    IEnumerator OnlineSave(WWWForm form)
    {
        using (UnityWebRequest www = UnityWebRequest.Post(ScriptLink, form))
        {
            yield return www.SendWebRequest();

            if (www.isDone)
            {
                if (www.downloadHandler.text == "Done")
                {
                    // Save Complete
                }
                else
                {
                    // ReTry
                }
            }
            else
            {
                // ReTry
            }
        }
    }

    IEnumerator OnlineLoad(WWWForm form)
    {
        using (UnityWebRequest www = UnityWebRequest.Post(ScriptLink, form))
        {
            yield return www.SendWebRequest();

            if (www.isDone)
            {
                SetMusicInfo(www.downloadHandler.text);
            }
            else { }
        }
    }

    private void SetMusicInfo(string webResult)
    {
        print(webResult);

        string[] music;
        music = webResult.Split('%');

        for (int i = 0; i < music.Length; i++)
        {
            Music target;
            target = Music.MusicList[i];

            string[] info;
            info = music[i].Split('|');

            string[] _playPure;
            string[] _playMaxCombo;
            string[] _playHighScore;
            string[] _isOwned;

            _playPure = info[0].Split(',');
            _playMaxCombo = info[1].Split(',');
            _playHighScore = info[2].Split(',');
            _isOwned = info[3].Split(',');

            for (int j = 0; j < 5; j++)
            {
                target.playPure[j] = Convert.ToInt32(_playPure[j]);
                target.playMaxCombo[j] = Convert.ToInt32(_playMaxCombo[j]);
                target.playHighScore[j] = Convert.ToInt32(_playHighScore[j]);

                if (_isOwned[j] == "1") { target.isOwned[j] = true; }
                else { target.isOwned[j] = false; }
            }
        }

        MusicSelectAct.musicSelect.GenerateMusicBox();
    }

    #region Offline Save Load
    [ContextMenu("Save")]
    public void SaveDataToJson(int index)
    {
        Music music;
        music = Music.MusicList[index];

        string name;
        name = "PlayedData/" + music.MusicName + ".json";

        musicSave.playPure = music.playPure;
        musicSave.playMaxCombo = music.playMaxCombo;
        musicSave.playHighScore = music.playHighScore;

        musicSave.isOwned = music.isOwned;

        string jsonData = JsonUtility.ToJson(musicSave, true);
        string path = Path.Combine(Application.dataPath, name);
        File.WriteAllText(path, jsonData);
    }

    [ContextMenu("Load")]
    void LoadDataFromJson(int index)
    {
        Music music;
        music = Music.MusicList[index];

        string name;
        name = "PlayedData/" + music.MusicName + ".json";
        string path = Path.Combine(Application.dataPath, name);
        try
        {
            string jsonData = File.ReadAllText(path);
            musicSave = JsonUtility.FromJson<MusicSaveData>(jsonData);
        }
        catch
        {
            music.playPure = new int[5] { 0, 0, 0, 0, 0 };
            music.playMaxCombo = new int[5] { 0, 0, 0, 0, 0 };
            music.playHighScore = new int[5] { 0, 0, 0, 0, 0 };

            music.isOwned = musicSave.isOwned;
            SaveDataToJson(index);
            return;
        }
        music.playPure = musicSave.playPure;
        music.playMaxCombo = musicSave.playMaxCombo;
        music.playHighScore = musicSave.playHighScore;

        music.isOwned = musicSave.isOwned;
    }
    #endregion
}

[System.Serializable]
public class MusicSaveData
{
    public int[] playPure;
    public int[] playMaxCombo;
    public int[] playHighScore;

    public bool[] isOwned;
}
