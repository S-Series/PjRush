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

    private void Awake()
    {
        GameInfoSettingAnimator = GetComponent<Animator>();
    }

    public void InfoSetting()
    {
        int index = MainSystem.mainSystem.difficulty;
        Music music = MainSystem.NowOnMusic;

        infoJacket.sprite = music.sprJacket;

        infoGameInfoTMP[0].text = music.MusicName;
        infoGameInfoTMP[1].text = music.MusicArtist;

        difficultyObject.GetComponent<SpriteRenderer>().sprite 
            = difficultySprite[index];
        difficultyObject.GetComponentInChildren<TextMeshPro>().text 
            = music.Difficulty[index].ToString();
    }
}
