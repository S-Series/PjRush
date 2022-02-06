using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MusicSelectAct : MonoBehaviour
{
    public static MusicSelectAct musicSelect;

    #region SelectAct
    private static List<GameObject> MusicList;

    [SerializeField]
    GameObject MusicFramePrefab;

    [SerializeField]
    GameObject MusicFrameCopyField;

    [SerializeField]
    GameObject MusicSelectBox;

    [SerializeField]
    private int MaxMusicIndex;
    [SerializeField]
    private int MusicIndex;

    private int difficultyNum;

    public bool isActiveSelect;
    public bool isPlayReady;

    private GameObject AudioSync;
    #endregion

    #region LeftInfoBox
    private float GameSpeed;

    [SerializeField]
    TextMeshPro[] GameInfo;
    /// <summary>
    /// 0 || MusicName
    /// 1 || MusicArtist
    /// 2 || Difficulty Easy
    /// 3 || Difficulty Normal
    /// 4 || Difficulty Hard
    /// 5 || Difficulty Expert
    /// 6 || PlayedBestScore
    /// 7 || BPM
    /// 5 || 
    /// 6 || 
    /// </summary>

    [SerializeField]
    SpriteRenderer[] GameSpriteRenderer;
    /// <summary>
    /// 0 || Jacket
    /// 1 || Difficulty Easy
    /// 2 || Difficulty Normal
    /// 3 || Difficulty Hard
    /// 4 || Difficulty Expert
    /// 5 || PlayedBestRank
    /// 6 || ++
    /// 4 || 
    /// 5 || 
    /// </summary>

    public AudioSource audioSource;

    [SerializeField]
    Sprite[] DifficultySprite;

    [SerializeField]
    GameObject[] Difficulty;

    [SerializeField]
    SpriteRenderer MusicJacekt;
    [SerializeField]
    GameObject Secret;

    [SerializeField]
    TextMeshPro[] ScoreText;
    #endregion

    #region PlayReady
    [SerializeField]
    GameObject TurnOffAnimator;
    #endregion

    [SerializeField]
    AudioClip audSecretClip;

    private void Awake()
    {
        if (musicSelect != null)
        { 
            Destroy(gameObject); 
        }
        else
        {
            TurnOffAnimator.SetActive(true);
            audioSource = GetComponent<AudioSource>();
            MusicList = new List<GameObject>();
            MaxMusicIndex = Music.MusicCount;
            MusicIndex = 0;
            difficultyNum = 0;
            isActiveSelect = false;
            isPlayReady = false;
            musicSelect = this;
        }
    }

    private void Update()
    {
        if (isActiveSelect)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow)
                || Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                MusicIndex--;
                CheckMusicIndex();
                SelectFramePosition();
                SetTopBoxInfo();
                ScaleSync();
            }

            if (Input.GetKeyDown(KeyCode.RightArrow)
                || Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                MusicIndex++;
                CheckMusicIndex();
                SelectFramePosition();
                SetTopBoxInfo();
                ScaleSync();
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                MusicIndex -= 4;
                CheckMusicIndex();
                SelectFramePosition();
                SetTopBoxInfo();
                ScaleSync();
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                MusicIndex += 4;
                CheckMusicIndex();
                SelectFramePosition();
                SetTopBoxInfo();
                ScaleSync();
            }

            // -----------------------------------------------------

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                // Music Sorting Option
            }

            // -----------------------------------------------------

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                difficultyNum--;
                if (difficultyNum < 0) { difficultyNum = 0; }

                SetTopBoxInfo();
                ScaleSync();

                foreach(GameObject game in MusicList)
                {
                    game.GetComponent<SongFrame>().SetInfo(difficultyNum);
                }
            }

            if (Input.GetKeyDown(KeyCode.RightShift))
            {
                difficultyNum++;
                if (difficultyNum > 4) { difficultyNum = 4; }
          
                SetTopBoxInfo();
                ScaleSync();

                foreach (GameObject game in MusicList)
                {
                    game.GetComponent<SongFrame>().SetInfo(difficultyNum);
                }
            }

            // -----------------------------------------------------

            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
            {

            }
        }

        else if (isPlayReady)
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
            {
                PlayerPrefs.SetFloat("PlaySpeed", GameSpeed);
                StartCoroutine(GameStartAnimate());
                isPlayReady = false;
            }
        }
    }

    #region Setting Function
    private void CheckMusicIndex()
    {
        if (MusicIndex < 0)
        {
            MusicIndex = MaxMusicIndex;
        }
        if (MusicIndex > MaxMusicIndex)
        {
            MusicIndex = 0;
        }
    }

    private void SelectFramePosition()
    {
        MusicSelectBox.transform.localPosition
            = new Vector3(-3.25f + 3.75f * (MusicIndex % 4), 0, 0);

        MusicFrameCopyField.transform.localPosition
            = new Vector3(0, 3.75f * ((MusicIndex - MusicIndex % 4) / 4), 0);

        audioSource.clip = Music.MusicList[MusicIndex].audPreMusicFile;
        audioSource.Play();
    }

    private void SetTopBoxInfo()
    {
        Music music;
        music = Music.MusicList[MusicIndex];

        GameInfo[0].text = music.MusicName;
        GameInfo[1].text = music.MusicArtist;
        for (int i = 0; i < 4; i++)
        {
            if (music.Difficulty[i] == 0)
            {
                Difficulty[i].SetActive(false);
            }
            else
            {
                Difficulty[i].SetActive(true);

                if (music.isSecret[i])
                {
                    GameInfo[i + 2].text = "??";
                }
                else
                {
                    GameInfo[i + 2].text = music.Difficulty[i].ToString();
                }
            }
        }
        GameInfo[6].text = music.playHighScore[difficultyNum].ToString();
        if (music.LowBPM == music.HighBPM)
        {
            GameInfo[7].text = "BPM  " + music.LowBPM.ToString();
        }
        else
        {
            GameInfo[7].text = "BPM  " + music.LowBPM.ToString()
                + " - " + music.HighBPM.ToString();
        }

        MusicJacekt.sprite = music.sprJacket;
        Secret.SetActive(music.isSecret[difficultyNum]);

        if (music.isSecret[difficultyNum])
        {
            //audio.clip = audSecretClip;
            //audio.Play();
            audioSource.Stop();
        }
        else
        {
            audioSource.clip = Music.MusicList[MusicIndex].audPreMusicFile;
            audioSource.Play();
        }

        int score = music.playHighScore[difficultyNum];
        print(score);
        if (score == 0)
        {
            for (int i = 0; i < 9; i++)
            {
                ScoreText[i].enabled = false;
            }
            ScoreText[9].enabled = true;
            return;
        }

        for (int i = 0; i < 9; i++)
        {
            ScoreText[i].enabled = true;
        }
        ScoreText[9].enabled = false;

        ScoreText[0].text = ((score % 10)).ToString();
        for (int i = 1; i < 9; i++)
        {
            double calculate1 = Convert.ToDouble(score / Mathf.Pow(10, i));
            print(calculate1);
            double calculate2 = (calculate1) - ((calculate1) % 1);
            print(calculate2);
            ScoreText[i].text = ((int)(calculate2 % 10)).ToString();
            ScoreText[i].color = new Color32(255, 255, 255, 255);
        }
        for (int i = 0; i < 9; i++)
        {
            if (score < Mathf.Pow(10,8 - i))
            {
                ScoreText[8 - i].color = new Color32(255, 255, 255, 150);
            }
            else { break; }
        }
    }

    private void ScaleSync()
    {
        GameObject tag = AudioSync;
        tag.GetComponent<AudioSyncScale>().enabled = false;

        AudioSync = MusicList[MusicIndex];
        AudioSync.GetComponent<AudioSyncScale>().enabled = true;

        tag.transform.localScale = new Vector3(0.35f, 0.35f, 1);
    }

    IEnumerator GameStartAnimate()
    {
        MainSystem.mainSystem.GameAnimator[0].SetTrigger("LoadStart");
        yield return new WaitForSeconds(1.5f);
        TurnOffAnimator.SetActive(false);
    }
    #endregion

    // Activate in MusicManager
    public void GenerateMusicBox()
    {
        for (int i = 0; i < Music.MusicCount; i++)
        {
            GameObject generate;
            generate = Instantiate(MusicFramePrefab, MusicFrameCopyField.transform);

            SongFrame frame;
            frame = generate.GetComponent<SongFrame>();

            frame.music = Music.MusicList[i];

            frame.SetInfo(difficultyNum);

            Vector3 pos;
            pos = new Vector3(-3.25f, 0, 0);

            pos.x = -3.25f + 3.75f * (i % 4);
            pos.y = -3.75f * ((i - i % 4) / 4);

            generate.transform.localPosition = pos;
            generate.transform.localScale = new Vector3(0.35f, 0.35f, 1);

            MusicList.Add(generate);
        }

        isActiveSelect = true;
        MaxMusicIndex = Music.MusicCount - 1;

        AudioSync = MusicList[0];
        AudioSync.GetComponent<AudioSyncScale>().enabled = true;
        SetTopBoxInfo();
    }
}
