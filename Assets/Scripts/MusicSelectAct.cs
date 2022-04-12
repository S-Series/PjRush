using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    private int MaxMusicIndex;
    private int MusicIndex;
    [SerializeField]
    private int MusicFrameIndex;
    [SerializeField]
    private int MusicFramePage;
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

    #region SettingBox
    [SerializeField]
    SpriteRenderer[] GameGuageTypeRenderer;

    [SerializeField]
    TextMeshPro[] JudgeControlTmp;

    [SerializeField]
    TextMeshPro[] SpeedControlTmp;
    #endregion

    #region PlayReady
    [SerializeField]
    AudioSource GameStartAudio;
    #endregion

    [SerializeField]
    AudioClip audSecretClip;

    [SerializeField]
    Animator MusicSelectedAnimator;

    private void Awake() {
        if (musicSelect != null) { 
            Destroy(this.gameObject); 
        }
        else {
            audioSource = GetComponent<AudioSource>();
            MusicList = new List<GameObject>();
            MaxMusicIndex = Music.MusicCount;
            MusicIndex = 0;
            MusicFrameIndex = 0;
            MusicFramePage = 0;
            difficultyNum = 0;
            foreach (SpriteRenderer renderer in GameGuageTypeRenderer) 
            { 
                renderer.enabled = false; 
                renderer.GetComponent<AudioSyncScale>().enabled = false;
            }
            isActiveSelect = false;
            isPlayReady = false;
            musicSelect = this;
        }
    }

    private void Update(){
        if (isActiveSelect){
            if (Input.GetKeyDown(KeyCode.LeftArrow)
                || Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                MusicIndex--;
                MusicFrameIndex--;
                CheckMusicIndex();
                SelectFramePosition();
                SetTopBoxInfo();
            }

            if (Input.GetKeyDown(KeyCode.RightArrow)
                || Input.GetAxis("Mouse ScrollWheel") < 0){
                MusicIndex++;
                MusicFrameIndex++;
                CheckMusicIndex();
                SelectFramePosition();
                SetTopBoxInfo();
            }

            if (Input.GetKeyDown(KeyCode.UpArrow)){
                MusicIndex -= 3;
                MusicFrameIndex -= 3;
                CheckMusicIndex();
                SelectFramePosition();
                SetTopBoxInfo();
            }

            if (Input.GetKeyDown(KeyCode.DownArrow)){
                MusicIndex += 3;
                MusicFrameIndex += 3;
                CheckMusicIndex();
                SelectFramePosition();
                SetTopBoxInfo();
            }

            // -----------------------------------------------------

            if (Input.GetKeyDown(KeyCode.Tab)){
                // Music Sorting Option
            }

            // -----------------------------------------------------

            if (Input.GetKeyDown(KeyCode.LeftShift)){
                difficultyNum--;
                if (difficultyNum < 0) { difficultyNum = 0; }

                SetTopBoxInfo();

                foreach(GameObject game in MusicList)
                {
                    game.GetComponent<SongFrame>().SetInfo(difficultyNum);
                }
            }

            if (Input.GetKeyDown(KeyCode.RightShift)){
                difficultyNum++;
                if (difficultyNum > 4) { difficultyNum = 4; }
          
                SetTopBoxInfo();

                foreach (GameObject game in MusicList)
                {
                    game.GetComponent<SongFrame>().SetInfo(difficultyNum);
                }
            }

            // -----------------------------------------------------

            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space)){
                MainSystem.NowOnMusic = Music.MusicList[MusicIndex];
                MusicSelectedAnimator.SetTrigger("Selected");
                isActiveSelect = false;
                isPlayReady = true;

                SpeedControlTmp[1].text = string.Format
                    ("{0:F2}", MainSystem.gameSpeed / MainSystem.NowOnMusic.HighBPM);
                SpeedControlTmp[0].text = MainSystem.NowOnMusic.HighBPM.ToString();
                SpeedControlTmp[2].text = MainSystem.gameSpeed.ToString();

                GameGuageTypeRenderer[MainSystem.GuageType].enabled = true;
                GameGuageTypeRenderer[MainSystem.GuageType].GetComponent<AudioSyncScale>().enabled = true;

            }
        }

        else if (isPlayReady){
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(IGameStartAnimate());
                isPlayReady = false;
            }
            if (Input.GetKeyDown(KeyCode.Tab)){
                int index;
                index = MainSystem.GuageType;
                GameGuageTypeRenderer[index].enabled = false;
                GameGuageTypeRenderer[index].GetComponent<AudioSyncScale>().enabled = false;
                index++;
                if (index > 2) index -= 3;
                GameGuageTypeRenderer[index].enabled = true;
                GameGuageTypeRenderer[index].GetComponent<AudioSyncScale>().enabled = true;
                MainSystem.GuageType = index;
            }
            if (Input.GetKeyDown(KeyCode.Escape)){
                MusicSelectedAnimator.SetTrigger("Deselected");
                isPlayReady = false;
                isActiveSelect = true;
            }

            if (Input.GetKeyDown(KeyCode.DownArrow) 
            || Input.GetKeyDown(KeyCode.LeftArrow)){
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)){
                    SetRecordSpeed(-10);
                }
                else{
                    SetRecordSpeed(-1);
                }
            }

            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)){
                    SetRecordSpeed(-10);
                }
                else{
                    SetRecordSpeed(-1);
                }
            }

            if (Input.GetKeyDown(KeyCode.UpArrow)
            || Input.GetKeyDown(KeyCode.RightArrow)){
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)){
                    SetRecordSpeed(+10);
                }
                else{
                    SetRecordSpeed(+1);
                }
            }

            if (Input.GetAxis("Mouse ScrollWheel") > 0){
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)){
                    SetRecordSpeed(+10);
                }
                else{
                    SetRecordSpeed(+1);
                }
            }
        }
    }

    #region Setting Function
    private void CheckMusicIndex(){
        if (MusicIndex < 0)
        {
            MusicIndex = MaxMusicIndex;
        }
        if (MusicIndex > MaxMusicIndex)
        {
            MusicIndex = 0;
        }
    }

    private void SelectFramePosition(){
        if (MusicFrameIndex < 0)
        {
            MusicFrameIndex += 3;
            MusicFramePage--;
        }
        if (MusicFrameIndex > 8)
        {
            MusicFrameIndex -= 3;
            MusicFramePage++;
        }

        if (MusicFramePage > Mathf.FloorToInt(Music.MusicCount / 3) - 3)
        {
            MusicFramePage = 0;
            MusicFrameIndex = MusicIndex;
        }
        if (MusicFramePage < 0)
        {
            MusicFramePage = Mathf.FloorToInt(Music.MusicCount / 3) - 2;
            MusicFrameIndex = 6 + MusicIndex % 3;
        }

        if (MusicFrameIndex < 0)
        {
            MusicFrameIndex += 3;
            MusicFramePage--;
        }
        if (MusicFrameIndex > 8)
        {
            MusicFrameIndex -= 3;
            MusicFramePage++;
        }

        MusicSelectBox.transform.localPosition
            = new Vector3(-2.625f + (5 * (MusicFrameIndex % 3))
            , 2.3f - (2.3f * ((MusicFrameIndex - (MusicFrameIndex % 3)) / 3)), 0);

        MusicFrameCopyField.transform.localPosition
            = new Vector3(0, 2.3f * MusicFramePage, 0);

        audioSource.clip = Music.MusicList[MusicIndex].audPreMusicFile;
        audioSource.Play();
    }
    private void SetTopBoxInfo(){
        Music music;
        music = Music.MusicList[MusicIndex];

        if (music.isSecret[difficultyNum])
        {
            GameInfo[0].text = "????????";
            GameInfo[1].text = "????????";
            GameInfo[8].text = "BPM  ???";
        }
        else
        {
            GameInfo[0].text = music.MusicName;
            GameInfo[1].text = music.MusicArtist;
            if (music.LowBPM == music.HighBPM)
            {
                GameInfo[8].text = "BPM  " + music.LowBPM.ToString();
            }
            else
            {
                GameInfo[8].text = "BPM  " + music.LowBPM.ToString()
                    + " - " + music.HighBPM.ToString();
            }
        }

        for (int i = 0; i < 5; i++)
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

        MusicJacekt.sprite = music.sprJacket;
        Secret.SetActive(music.isSecret[difficultyNum]);

        if (music.isSecret[difficultyNum])
        {
            audioSource.clip = audSecretClip;
            audioSource.Play();
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
    private void SetRecordSpeed(int change){
        int gameSpeed;
        gameSpeed = MainSystem.gameSpeed;
        gameSpeed += change;
        if (gameSpeed < 50) gameSpeed = 50;
        MainSystem.gameSpeed = gameSpeed;
        SpeedControlTmp[1].text = string.Format
            ("{0:F2}", gameSpeed / MainSystem.NowOnMusic.HighBPM);
        SpeedControlTmp[2].text = gameSpeed.ToString();
    }

    IEnumerator IGameStartAnimate(){
        GameStartAudio.Play();
        MainSystem.mainSystem.RunIgameStart();
        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene("GameField");
    }
    #endregion
    public void GenerateMusicBox(){
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

            pos.x = -2.625f + 5 * (i % 3);
            pos.y = 2.3f - (2.3f * ((i - i % 3) / 3));

            generate.transform.localPosition = pos;
            generate.transform.localScale = new Vector3(0.4f, 0.4f, 1);

            MusicList.Add(generate);
        }

        isActiveSelect = true;
        MaxMusicIndex = Music.MusicCount - 1;

        SetTopBoxInfo();
    }
}
