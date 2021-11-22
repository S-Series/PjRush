using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameStartMenu : MonoBehaviour
{
    public static GameStartMenu gameStartMenu;

    public bool isGameStartSelectActAvailable;

    public string edit_songName;
    public double edit_highSpeed;
    public int edit_difficulty;
    public int edit_difficultyNum;

    [SerializeField]
    Sprite[] dif;

    MainSystem mainSystem;

    private void Awake()
    {
        mainSystem = MainSystem.mainSystem;
        gameStartMenu = this;
    }

    private void Start()
    {
        mainSystem = MainSystem.mainSystem;
        edit_highSpeed = 1;
        isGameStartSelectActAvailable = false;
    }

    void Update()
    {
        if (isGameStartSelectActAvailable == true)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (edit_difficulty > 0) edit_difficulty--;
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (edit_difficulty < 3) edit_difficulty++;
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                {
                    edit_highSpeed++;
                }
                else edit_highSpeed += 0.1;

                retouchSpeed();
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                {
                    edit_highSpeed--;
                }
                else edit_highSpeed -= 0.1;

                retouchSpeed();
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                MusicSelectAct.musicSelectAct.isMusicSelectActAvailable = true;
                isGameStartSelectActAvailable = false;
                MusicSelectAct.musicSelectAct.NowOnDifficulty = edit_difficultyNum;
            }
            if (Input.GetKeyDown(KeyCode.Return))
            {                
                mainSystem.songName = edit_songName;
                mainSystem.highSpeed = edit_highSpeed;
                mainSystem.difficulty = edit_difficulty;
                mainSystem.difficultyNum = edit_difficultyNum;

                mainSystem.AudioFadeOut[0].SetTrigger("FadeOut");
                foreach (Animator animate in mainSystem.Animator)
                {
                    try
                    {
                        animate.SetTrigger("Load_Start");
                    }
                    catch { }
                }

                StartCoroutine(startChange());
            }
        }
    }

    IEnumerator startChange()
    {
        int index;
        int indexNum;
        index = MusicSelectAct.musicSelectAct.firstIndex;
        indexNum = MusicSelectAct.musicSelectAct.NowOnIndex;

        SongInfo songInfo;
        SongFrame songFrame;
        songInfo = MusicBox.musicBox.MusicInfoObjectList[index + indexNum].GetComponent<SongInfo>();
        songFrame = DisplayMusic.displayMusic.MusicDisplayBox[indexNum].GetComponent<SongFrame>();

        mainSystem.SceneAnimateJacket[0].sprite = songFrame.MusicJacket.sprite;
        mainSystem.SceneAnimateJacket[1].sprite = dif[mainSystem.difficultyNum];
        mainSystem.SceneAnimateJacket[2].sprite = dif[mainSystem.difficultyNum + 4];

        if (songInfo.difficulty[mainSystem.difficultyNum] < 10)
        {
            mainSystem.SceneAnimateTMPro[0].text = "0" + songInfo.difficulty[mainSystem.difficultyNum].ToString();
        }
        else
        {
            mainSystem.SceneAnimateTMPro[0].text = songInfo.difficulty[mainSystem.difficultyNum].ToString();
        }
        mainSystem.gameEndMS = songInfo.endMs;
        mainSystem.songName = songInfo.SongName;
        mainSystem.SceneAnimateTMPro[1].text = songInfo.SongName;
        mainSystem.SceneAnimateTMPro[2].text = songInfo.WhoMade;

        yield return new WaitForSeconds(10.0f);
        SceneManager.LoadScene("GameField");
    }

    private void retouchSpeed()
    {
        if (edit_highSpeed > 10) edit_highSpeed = 10;
        if (edit_highSpeed < 1) edit_highSpeed = 1;
    }
}
