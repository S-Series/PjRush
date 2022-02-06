using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    public static int MusicCount = 0;

    public static List<Music> MusicList;

    public int MusicID;
    public float LowBPM;
    public float HighBPM;

    public string MusicName;
    public string MusicArtist;
    public string[] Effecter = new string[5];

    public int[] Difficulty = new int[5];
    public int[] NoteCount = new int[5];
    public int[] playPure = new int[5];
    public int[] playMaxCombo = new int[5];
    public int[] playHighScore = new int[5];

    public bool[] isOwned = new bool[5];
    public bool[] isSecret = new bool[5];

    public Sprite sprJacket;
    public AudioClip audMusicFile;
    public AudioClip audPreMusicFile;
}
