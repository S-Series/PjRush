﻿using System;
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
    [SerializeField] private TopBoxSetting topBox;
    private readonly string[] DifficultyName = {"Day", "Midday", "Night", "Midnight", "Dream"};
    private IEnumerator keepDown_U;
    private IEnumerator keepDown_D;
    private IEnumerator keepDown_L;
    private IEnumerator keepDown_R;
    private IEnumerator[] option = new IEnumerator[2];

    private void Awake() 
    { 
        musicSelectAct = this; 
        PreMusicPlayer = GetComponent<AudioSource>();
        keepDown_U = IKeepDown_U();
        keepDown_D = IKeepDown_D();
        keepDown_L = IKeepDown_L();
        keepDown_R = IKeepDown_R();
        option[0] = IOption(false);
        option[1] = IOption(true);
    }
    private void Update()
    {
        if (!isSelectable) { return; }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            keepDown_U = IKeepDown_U();
            SelectChange(KeyCode.UpArrow);
            StartCoroutine(keepDown_U);
        }
        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            StopCoroutine(keepDown_U);
        }
        
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            keepDown_D = IKeepDown_D();
            SelectChange(KeyCode.DownArrow);
            StartCoroutine(keepDown_D);
        }
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            StopCoroutine(keepDown_D);
        }
        
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            keepDown_L = IKeepDown_L();
            SelectChange(KeyCode.LeftArrow);
            StartCoroutine(keepDown_L);
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            StopCoroutine(keepDown_L);
        }
        
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            keepDown_R = IKeepDown_R();
            SelectChange(KeyCode.RightArrow);
            StartCoroutine(keepDown_R);
        }
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            StopCoroutine(keepDown_R);
        }
        
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            StartCoroutine(option[0]);
        }
        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            StartCoroutine(option[1]);
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            // Todo: Sorting 기능 구현하여 넣을것
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            isSelectable = false;
            //* index = 0   || MusicName
            //* index = 1   || MusicArtist
            //* index = 2   || Bpm
            //* index = 3   || Difficulty
            //* index = 4   || Difficulty Name
            //* index = 5   || NoteEffecter
            //* index = 6   || Jacket Illustrator
            string[] info = new string[7];
            info[0] = SelectingMusic.MusicName;
            info[1] = SelectingMusic.MusicArtist;
            if (SelectingMusic.HighBPM == SelectingMusic.LowBPM) 
                { info[2] = string.Format("{0:F2}", SelectingMusic.LowBPM); }
            else { info[2] = string.Format("{0:F2}", SelectingMusic.LowBPM) 
                + " - " + string.Format("{0:F2}", SelectingMusic.HighBPM);}
            info[3] = string.Format("{0:D2}", SelectingMusic.Difficulty[SelectDifficultyIndex]);
            info[4] = DifficultyName[SelectDifficultyIndex];
            info[5] = SelectingMusic.Effecter[SelectDifficultyIndex];
            info[6] = SelectingMusic.JacketIllustrator;

            GameManager.s_OnGameMusic = SelectingMusic;
            GameManager.s_OnGameDifficultyIndex = SelectDifficultyIndex;

            AnimatorManager.ChangeMusicInfo(info);
            AnimatorManager.ChangeJacket
                (SelectingMusic.sprJacket, SelectDifficultyIndex, SelectingMusic.status);
            MainSystem.LoadGameScene();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isSelectable = false;
            MainSystem.LoadMainScene();
        }
    }
    public static void LoadSelectMusic()
    {
        //foreach (SongFrame frame in MusicFrame) { Destroy(frame.gameObject); }
        MusicFrame = new List<SongFrame>();
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
            posY = 2.25f -2.25f * Mathf.FloorToInt(i / 3.0f);
            copy.transform.localPosition = new Vector3(posX, posY, 0.0f);
        }
        SelectingMusic = selectMusicList[0];
        musicSelectIndex = 0;
        musicSelectMaxIndex = selectMusicList.Count - 1;
        UpdateFrameInfo();
        SelectingMusic = selectMusicList[musicSelectIndex];
        PreMusicPlayer.clip = SelectingMusic.audPreMusicFile;
        musicSelectAct.topBox.SetInfo(SelectingMusic, SelectDifficultyIndex);
        PreMusicPlayer.Play();
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
                if (musicSelectIndex < 3)
                {
                    musicSelectIndex 
                        = (Mathf.CeilToInt((musicSelectMaxIndex + 1) / 3.0f) - 1) * 3
                        + musicSelectIndex;
                }
                else { musicSelectIndex -= 3; }
                break;

            case KeyCode.DownArrow:
                if (musicSelectIndex > musicSelectMaxIndex - 3)
                {
                    if (Mathf.CeilToInt((musicSelectIndex + 1) / 3.0f)
                    == Mathf.CeilToInt((musicSelectMaxIndex + 1) / 3.0f))
                        { musicSelectIndex = musicSelectMaxIndex; }
                    else { musicSelectIndex = musicSelectIndex % 3; }
                }
                else { musicSelectIndex += 3; }
                break;

            case KeyCode.LeftArrow:
                if (musicSelectIndex == 0) { musicSelectIndex = musicSelectMaxIndex; }
                else { musicSelectIndex--; }
                break;

            case KeyCode.RightArrow:
                if (musicSelectIndex == musicSelectMaxIndex) { musicSelectIndex = 0; }
                else { musicSelectIndex++; }
                break;
        }
        SelectingMusic = selectMusicList[musicSelectIndex];
        PreMusicPlayer.clip = SelectingMusic.audPreMusicFile;
        UpdateFramePosition();
        //SelectEffectAudio.Play();
        for (int i = 0; i < 5; i++)
        {
            if (SelectingMusic.isAvailable[SelectDifficultyIndex]) break;
            else
            {
                SelectDifficultyIndex--;
                if (SelectDifficultyIndex < 0) { SelectDifficultyIndex += 5; }
            }
        }
        musicSelectAct.topBox.SetInfo(SelectingMusic, SelectDifficultyIndex);
        PreMusicPlayer.Play();
    }
    private void UpdateFramePosition()
    {
        SelectFrameObject.transform.localPosition
            = new Vector3(-2.65f + 5.0f * Mathf.CeilToInt(musicSelectIndex % 3.0f), 2.25f, 0.0f);
        FrameParentTransform.localPosition
            = new Vector3(0.0f, (2.25f * (Mathf.CeilToInt((musicSelectIndex + 1) / 3.0f) - 1)), 0.0f);
    }
    private void DifficultySetting(bool isLeft)
    {
        int _change = 1;
        if (!isLeft) _change = 1;
        for (int i = 0; i < 5; i++)
        {
            SelectDifficultyIndex += _change;
            if (SelectDifficultyIndex < 0) { SelectDifficultyIndex += 5; }
            else if (SelectDifficultyIndex > 4) { SelectDifficultyIndex -= 5; }
            if ( SelectingMusic.isAvailable[SelectDifficultyIndex] )
            {
                UpdateFrameInfo();
                break;
            }
        }
    }
    private IEnumerator IKeepDown_U()
    {
        KeyCode inputKey = KeyCode.UpArrow;
        int count = 0;
        var wait = new WaitForSeconds(.125f);
        var shortWait = new WaitForSeconds(.0625f);
        yield return new WaitForSeconds(1f);
        while(true)
        {
            if (count < 4) { yield return wait; }
            else { yield return shortWait; }
            musicSelectAct.SelectChange(inputKey);
            count++;
        }
    }
    private IEnumerator IKeepDown_D()
    {
        KeyCode inputKey = KeyCode.DownArrow;
        int count = 0;
        var wait = new WaitForSeconds(.125f);
        var shortWait = new WaitForSeconds(.0625f);
        yield return new WaitForSeconds(1f);
        while(true)
        {
            if (count < 4) { yield return wait; }
            else { yield return shortWait; }
            musicSelectAct.SelectChange(inputKey);
            count++;
        }
    }
    private IEnumerator IKeepDown_L()
    {
        KeyCode inputKey = KeyCode.LeftArrow;
        int count = 0;
        var wait = new WaitForSeconds(.125f);
        var shortWait = new WaitForSeconds(.0625f);
        yield return new WaitForSeconds(1f);
        while(true)
        {
            if (count < 4) { yield return wait; }
            else { yield return shortWait; }
            musicSelectAct.SelectChange(inputKey);
            count++;
        }
    }
    private IEnumerator IKeepDown_R()
    {
        KeyCode inputKey = KeyCode.RightArrow;
        int count = 0;
        var wait = new WaitForSeconds(.125f);
        var shortWait = new WaitForSeconds(.0625f);
        yield return new WaitForSeconds(0.5f);
        while(true)
        {
            if (count < 4) { yield return wait; }
            else { yield return shortWait; }
            musicSelectAct.SelectChange(inputKey);
            count++;
        }
    }
    private IEnumerator IOption(bool isLeft)
    {
        print("AAA");
        yield return null;
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.RightShift))
        {

        }
        else { DifficultySetting(isLeft); }
    }
}