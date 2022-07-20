using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MusicManager : MonoBehaviour
{
    private static Transform musicTransform;
    private static MusicSave musicSave;
    public static List<Music> musicList = new List<Music>();
    private void Awake() 
    {
        musicTransform = this.transform;    
    }

    public static void SaveMusic(int index)
    {

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
        string savePath = "";
        try
        {
            savePath = Path.Combine(Application.dataPath, "");
        }
        catch
        {
            savePath = Path.Combine(Application.dataPath, "");
        }
        yield return null;
    }
}

public class MusicSave
{
    public int[] PerfectCount = new int[5];
    public int[] MaxCombo = new int[5];
    public int[] HighScore = new int[5];

    public bool[] isOwned = new bool[5];
    public bool[] isSecret = new bool[5];
}
