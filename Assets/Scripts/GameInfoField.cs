using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameInfoField : MonoBehaviour
{
    public Animator GameInfoSettingAnimator;

    [SerializeField]
    SpriteRenderer infoJacket;

    [SerializeField]
    TextMeshPro[] infoGameInfoTMP;

    [SerializeField]
    Sprite[] difficultySprite;

    [SerializeField]
    GameObject difficultyObject;

    public TextMeshPro[] ScoreText;
    public TextMeshPro PureScoreText;

    public static int score = 0;

    private void Awake()
    {
        GameInfoSettingAnimator = GetComponent<Animator>();
    }

    private void LateUpdate() {
        char[] scoreText;
        scoreText = (string.Format("{0:D9}", score)).ToCharArray();
        for (int i = 0; i < 9; i++){
            ScoreText[i].text = scoreText[i].ToString();
        }
    }

    public void InfoSetting()
    {
        int index = 0;//MainSystem.mainSystem.difficulty;
        Music music = null;//MainSystem.NowOnMusic;

        infoJacket.sprite = music.sprJacket;

        infoGameInfoTMP[0].text = music.MusicName;
        infoGameInfoTMP[1].text = music.MusicArtist;

        difficultyObject.GetComponent<SpriteRenderer>().sprite 
            = difficultySprite[index];
        difficultyObject.GetComponentInChildren<TextMeshPro>().text 
            = music.Difficulty[index].ToString();
    }
}
