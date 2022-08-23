using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GamePlaySystem : MonoBehaviour
{
    public static int s_gameMs;
    [SerializeField] private JudgeSystem[] judgeSystems;
    public static IEnumerator LoadMusicFile()
    {
        string path;
        path = Application.dataPath + "/_NoteBox/" 
            + string.Format("{0:D4}", GameManager.s_OnGameMusic.MusicID) + "/" 
            + string.Format("{0:D4}", GameManager.s_OnGameDifficultyIndex);
    }
}