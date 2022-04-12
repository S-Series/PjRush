using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MusicManager : MonoBehaviour
{
    MusicSaveData musicSave = new MusicSaveData();

    private void Awake(){
        Music.MusicList = new List<Music>();
    }
    public void SetMusicInfo(string webResult){
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

            for (int j = 0; j < 5; j++){
                target.playPure[j] = Convert.ToInt32(_playPure[j]);
                target.playMaxCombo[j] = Convert.ToInt32(_playMaxCombo[j]);
                target.playHighScore[j] = Convert.ToInt32(_playHighScore[j]);

                if (_isOwned[j] == "1") { target.isOwned[j] = true; }
                else { target.isOwned[j] = false; }
            }
            SaveDataToJson(i);
        }
    }

    #region Offline Save Load
    public void SaveDataToJson(int index){
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
    public void LoadDataFromJson(int index){
        Music music;
        music = Music.MusicList[index];

        string name;
        name = "PlayedData/" + music.MusicName + ".json";
        string path = Path.Combine(Application.dataPath, name);
        try{
            string jsonData = File.ReadAllText(path);
            musicSave = JsonUtility.FromJson<MusicSaveData>(jsonData);
        }
        catch{
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
