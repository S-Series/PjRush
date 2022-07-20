using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    //** Data UnChange
    public Sprite sprJacket;
    public AudioClip audMusicFile;
    public AudioClip audPreMusicFile;
    private bool[] isAvailable = new bool[5];
    public int MusicID;
    public float LowBPM;
    public float HighBPM;
    public string MusicName;
    public string MusicArtist;
    public string[] Effecter = new string[5];
    public int[] Difficulty = new int[5];
    public int[] NoteCount = new int[5];

    //** Data Change
    public int[] PerfectCount = new int[5];
    public int[] MaxCombo = new int[5];
    public int[] HighScore = new int[5];

    public bool[] isOwned = new bool[5];
    public bool isSecret;
}
