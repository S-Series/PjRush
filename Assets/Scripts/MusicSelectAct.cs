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
    public static SortingState sotringState = SortingState.General;
    public static int SelectDifficultyIndex;
    private static int musicSelectIndex = 0;
    private static int musicSelectMaxIndex = 0;
    public static bool isSelectable = false;
    public static bool isSortingReverse = false;
    public static Music SelectingMusic;
    public static AudioSource PreMusicPlayer;
    private static List<Music> selectMusicList = new List<Music>();
    private static List<SongFrame> MusicFrame = new List<SongFrame>();

    //** -------------------------------------
    [SerializeField] private GameObject SelectFrameObject;
    [SerializeField] private GameObject FramePrefab;
    [SerializeField] private Transform FrameParentTransform;
    [SerializeField] private AudioSource SelectEffectAudio;


    private void Awake() 
    { 
        musicSelectAct = this; 
        PreMusicPlayer = GetComponent<AudioSource>();
    }
    private void Update()
    {
        if (!isSelectable) { return; }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            SelectChange(KeyCode.UpArrow);
            StartCoroutine(IKeepDown(KeyCode.UpArrow));
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            SelectChange(KeyCode.DownArrow);
            StartCoroutine(IKeepDown(KeyCode.DownArrow));
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            SelectChange(KeyCode.LeftArrow);
            StartCoroutine(IKeepDown(KeyCode.LeftArrow));
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            SelectChange(KeyCode.RightArrow);
            StartCoroutine(IKeepDown(KeyCode.RightArrow));
        }
        //if (Input.GetKeyDown(KeyCode.LeftShift))
    }
    public static void LoadSelectMusic()
    {
        selectMusicList = MusicManager.musicList;
        for (int i = 0; i < selectMusicList.Count; i++)
        {
            GameObject copy;
            copy = Instantiate(musicSelectAct.FramePrefab, musicSelectAct.FrameParentTransform);
            SongFrame copyFrame;
            copyFrame = copy.GetComponent<SongFrame>();
            MusicFrame.Add(copyFrame);
            float posX;
            float posY;
            posX = -2.65f + 5.0f * (i % 3.0f);
            posY = 1.75f + -2.25f * Mathf.FloorToInt(i / 3.0f);
            copy.transform.localPosition = new Vector3(posX, posY, 0.0f);
        }
        SelectingMusic = selectMusicList[0];
        musicSelectIndex = 0;
        musicSelectMaxIndex = selectMusicList.Count;
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
    private void SelectChange(KeyCode inputKey)
    {
        PreMusicPlayer.Stop();
        //** Algorithm of Selecting Input Movement
        switch(inputKey)
        {
            case KeyCode.UpArrow:
                SelectVertical--;
                if (SelectVertical < 0)
                { 
                    SelectVertical = Mathf.CeilToInt(MaxCount / 3) - 1;
                    while ((SelectVertical * 3) + (SelectHorizon + 1) > MaxCount) { SelectHorizon--; }
                }
                break;

            case KeyCode.DownArrow:
                SelectVertical++;
                int max = Mathf.CeilToInt(MaxCount) - 1;
                if (SelectVertical > max) { SelectVertical = 0; }
                else if (SelectVertical == max)
                    { while ((SelectVertical * 3) + (SelectHorizon + 1) > MaxCount) { SelectHorizon--; } }
                break;

            case KeyCode.LeftArrow:
                SelectHorizon--;
                if (SelectHorizon < 0)
                {
                    SelectHorizon = 2;
                    SelectVertical--;
                    if (SelectVertical < 0)
                    {
                        SelectVertical = Mathf.CeilToInt(MaxCount / 3) - 1;
                        while ((SelectVertical * 3) + (SelectHorizon + 1) > MaxCount) { SelectHorizon--; }
                    }
                }
                break;

            case KeyCode.RightArrow:
                SelectHorizon++;
                if (SelectHorizon > 2) { SelectHorizon = 0; SelectVertical++; }
                if ((SelectVertical * 3) + (SelectHorizon + 1) > MaxCount)
                    { SelectHorizon = 0; SelectVertical = 0; }
                break;

        }
        int index;
        index = SelectVertical * 3 + SelectHorizon;
        print(SelectVertical + "," + SelectHorizon);
        SelectingMusic = selectMusicList[index];
        PreMusicPlayer.clip = SelectingMusic.audPreMusicFile;
        UpdateFramePosition();
        //SelectEffectAudio.Play();
        PreMusicPlayer.Play();
    }
    private void UpdateFramePosition()
    {
        SelectFrameObject.transform.localPosition
            = new Vector3(-2.65f + 5.0f * SelectHorizon, 1.75f, 0.0f);
        FrameParentTransform.localPosition
            = new Vector3(0.0f, 1.75f - 2.25f * SelectVertical, 0.0f);
    }
    private IEnumerator IKeepDown(KeyCode inputKey)
    {
        int count = 0;
        var wait = new WaitForSeconds(1.0f);
        var shortWait = new WaitForSeconds(.25f);
        while(true)
        {
            if (!Input.GetKey(inputKey)) break;
            if (count < 5) { yield return wait; }
            else { yield return shortWait; }
            SelectChange(inputKey);
            count++;
        }
    }
}