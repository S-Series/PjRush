using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MusicManager : MonoBehaviour
{
    private static MusicSave musicSave = new MusicSave();
    public static List<Music> musicList = new List<Music>();

    public static void SaveMusic(int index)
    {

    }
    public static IEnumerator ILoadMusic()
    {
        string savePath = "";
        savePath = Path.Combine(Application.dataPath, );
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
