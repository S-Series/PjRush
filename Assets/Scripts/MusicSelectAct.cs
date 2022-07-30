using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MusicSelectAct : MonoBehaviour
{
    public static MusicSelectAct musicSelectAct;
    //** General Sorting == Date Sorting;
    public enum SortingState{General, Name, Cleared, Difficulty, Score, Rate}
    public static bool isSortingReverse = false;
    public static SortingState sotringState = SortingState.General;
    public static int SelectDifficultyIndex;
    public static Music SelectingMusic;
    private static List<Music> selectMusicList = new List<Music>();
    private static List<SongFrame> MusicFrame = new List<SongFrame>();

    //** -------------------------------------
    [SerializeField] private GameObject FramePrefab;
    [SerializeField] private Transform FrameParentTransform;

    private void Awake() { musicSelectAct = this; }
    public static void LoadSelectMusic()
    {
        selectMusicList = MusicManager.musicList;
        for (int i = 0; i < selectMusicList.Count; i++)
        {
            print(i);
            GameObject copy;
            copy = Instantiate(musicSelectAct.FramePrefab, musicSelectAct.FrameParentTransform);
            SongFrame copyFrame;
            copyFrame = copy.GetComponent<SongFrame>();
            MusicFrame.Add(copyFrame);
            float posX;
            float posY;
            posX = -2.65f + 5.0f * (i % 3.0f);
            posY = 4.5f + -2.25f * Mathf.FloorToInt(i / 3.0f);
            copy.transform.localPosition = new Vector3(posX, posY, 0.0f);
        }
        SelectingMusic = selectMusicList[0];
        UpdateFrameInfo();
    }
    public static void UpdateFrameInfo()
    {
        //** Sorting MusicMaanger List By Order
        switch(sotringState)
        {
            //** General == 수록일
            case SortingState.General:
                selectMusicList
                    = MusicManager.musicList.OrderBy(item => item.MusicID).ToList();
                break;

            case SortingState.Name:
                selectMusicList
                    = MusicManager.musicList.OrderBy(item => item.MusicName).ToList();
                break;

            case SortingState.Cleared:
                //** 보류중
                break;

            case SortingState.Difficulty:
                selectMusicList
                    = MusicManager.musicList.OrderBy(item => item.Difficulty[SelectDifficultyIndex])
                    .ThenBy(item => item.HighScore[SelectDifficultyIndex]).ToList();
                break;

            case SortingState.Score:
                selectMusicList
                    = MusicManager.musicList.OrderBy(item => item.HighScore[SelectDifficultyIndex])
                    .ThenBy(item => item.Difficulty).ThenBy(item => item.MusicID).ToList();
                break;

            case SortingState.Rate:
                //** 보류중
                break;
        }
        if (isSortingReverse)
        { selectMusicList = Enumerable.Reverse(MusicManager.musicList).ToList(); }

        int nowIndex;
        int lastId = SelectingMusic.MusicID;

        if (selectMusicList.Count != MusicFrame.Count)
            { throw new Exception("Problem at Music List Generation"); }  

        for (int i = 0; i < MusicManager.musicList.Count; i++)
            { MusicFrame[i].SetFrame(MusicManager.musicList[i]); }

        if (selectMusicList.Exists(item => item.MusicID == lastId))
            { nowIndex = selectMusicList.FindIndex(item => item.MusicID == lastId); }
        else { nowIndex = SelectDifficultyIndex; }
    }
}