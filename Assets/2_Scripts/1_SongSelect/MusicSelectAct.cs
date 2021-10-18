using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MusicSelectAct : MonoBehaviour
{
    public static MusicSelectAct musicSelectAct;

    public bool isMusicSelectActAvailable;

    public bool[] isAvailable;

    public int firstIndex;
    public int NowOnIndex;

    public int NowOnDifficulty;

    [SerializeField]
    SpriteRenderer MusicSelectFrameSprite;

    [SerializeField] // 0 = song name, 1 = who made
    TextMeshPro[] tmpSongInfo;

    [SerializeField]
    TextMeshPro tmpBpm;

    [SerializeField]
    TextMeshPro tmpClearRate;

    [SerializeField]
    TextMeshPro[] tmpScore;

    [SerializeField]
    SpriteRenderer songJacket;

    [SerializeField]
    SpriteRenderer songJacketPlus;

    [SerializeField]
    SpriteRenderer[] tmpSongDifficulty;

    private void Awake()
    {
        musicSelectAct = this;
    }

    void Start()
    {
        isMusicSelectActAvailable = true;
        isAvailable = new bool[8];
        for (int i = 0; i < 8; i++) isAvailable[i] = true;
        firstIndex = 0;
        NowOnIndex = 0;
        NowOnDifficulty = 0;
    }

    void Update()
    {
        if(isMusicSelectActAvailable == true)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                NowOnIndex++;
                checkingAvailable(1);
                changeFramePosition();
                getAudioClip();
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                NowOnIndex--;
                checkingAvailable(2);
                changeFramePosition();
                getAudioClip();
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                NowOnIndex -= 4;
                checkingAvailable(3);
                changeFramePosition();
                getAudioClip();
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                NowOnIndex += 4;
                checkingAvailable(4);
                changeFramePosition();
                getAudioClip();
            }
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                if (NowOnDifficulty > 0)
                {
                    NowOnDifficulty--;
                }
            }
            if (Input.GetKeyDown(KeyCode.RightShift))
            {
                if (NowOnDifficulty < 3)
                {
                    NowOnDifficulty++;
                }
            }
            if (Input.GetKeyUp(KeyCode.Return))
            {
                isMusicSelectActAvailable = false;
                GameStartMenu.gameStartMenu.isGameStartSelectActAvailable = true;
                GameStartMenu.gameStartMenu.edit_difficultyNum = NowOnDifficulty;
            }
            DisplayMusic.displayMusic.DisplayMusicInfo();
        }
    }

    private void checkingAvailable(int type)
    {
        if(NowOnIndex > 7)
        {
            if (firstIndex + 8 <= MusicBox.musicBox.MusicInfoObjectList.Count)
            {
                NowOnIndex -= 4;
                firstIndex += 4;
            }
            else
            {
                switch (type)
                {
                    case 1:
                        NowOnIndex--;
                        break;

                    case 4:
                        NowOnIndex -= 4;
                        break;
                }
            }
        }
        else if(NowOnIndex < 0)
        {
            if (firstIndex - 4 >= 0)
            {
                NowOnIndex += 4;
                firstIndex -= 4;
            }
            else
            {
                switch (type)
                {
                    case 2:
                        NowOnIndex++;
                        break;

                    case 3:
                        NowOnIndex += 4;
                        break;
                }
            }
        }

        switch (NowOnIndex)
        {
            case 0:
                if (isAvailable[0] == false)
                {
                    NowOnIndex--;
                    checkingAvailable(2);
                }
                break;
            case 1:
                if (isAvailable[1] == false)
                {
                    NowOnIndex--;
                    checkingAvailable(2);
                }
                break;
            case 2:
                if (isAvailable[2] == false)
                {
                    NowOnIndex--;
                    checkingAvailable(2);
                }
                break;
            case 3:
                if (isAvailable[3] == false)
                {
                    NowOnIndex--;
                    checkingAvailable(2);
                }
                break;
            case 4:
                if (isAvailable[4] == false)
                {
                    NowOnIndex--;
                    checkingAvailable(2);
                }
                break;
            case 5:
                if (isAvailable[5] == false)
                {
                    NowOnIndex--;
                    checkingAvailable(2);
                }
                break;
            case 6:
                if (isAvailable[6] == false)
                {
                    NowOnIndex--;
                    checkingAvailable(2);
                }
                break;
            case 7:
                if (isAvailable[7] == false)
                {
                    NowOnIndex--;
                    checkingAvailable(2);
                }
                break;
        }

        while (firstIndex + NowOnIndex > MusicBox.musicBox.MusicInfoObjectList.Count - 1)
        {
            NowOnIndex--;
        }
    }

    private void changeFramePosition()
    {
        switch (NowOnIndex) 
        {
            case 0:
                MusicSelectFrameSprite.transform.localPosition =
                        new Vector3(-8.0f, +0.8f, 0);
                break;

            case 1:
                MusicSelectFrameSprite.transform.localPosition =
                        new Vector3(-4.25f, +0.8f, 0);
                break;

            case 2:
                MusicSelectFrameSprite.transform.localPosition =
                        new Vector3(-0.5f, +0.8f, 0);
                break;

            case 3:
                MusicSelectFrameSprite.transform.localPosition =
                        new Vector3(+3.25f, +0.8f, 0);
                break;

            case 4:
                MusicSelectFrameSprite.transform.localPosition =
                        new Vector3(-8.0f, -3.2f, 0);
                break;

            case 5:
                MusicSelectFrameSprite.transform.localPosition =
                        new Vector3(-4.25f, -3.2f, 0);
                break;

            case 6:
                MusicSelectFrameSprite.transform.localPosition =
                        new Vector3(-0.5f, -3.2f, 0);
                break;

            case 7:
                MusicSelectFrameSprite.transform.localPosition =
                        new Vector3(+3.25f, -3.2f, 0);
                break;
        }
    }

    public void getAudioClip()
    {
        try
        {
            AudioSource previewAudio = this.gameObject.GetComponent<AudioSource>();

            previewAudio.clip = MusicBox.musicBox
                .MusicInfoObjectList[NowOnIndex].GetComponent<SongInfo>().GameMusicPerview;

            previewAudio.Play();
        }
        catch { }
    }

    private void UpdateTopBoxInfo()
    {

    }
}
