using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ResultDisplay : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer jacket;

    [SerializeField]
    SpriteRenderer Rank;

    [SerializeField]
    Sprite[] RankSprite;

    [SerializeField]
    SpriteRenderer Plus;

    [SerializeField]
    Sprite[] PlusSprite;

    [SerializeField]
    SpriteRenderer Difficulty;

    [SerializeField]
    Sprite[] DifficultyBoxSprite;

    [SerializeField]
    TextMeshPro SongName;

    [SerializeField]
    TextMeshPro difficultyNum;

    [SerializeField]
    TextMeshPro[] Score;

    [SerializeField]
    TextMeshPro[] Count;

    [SerializeField]
    TextMeshPro maxCount;

    [SerializeField]
    TextMeshPro prpCount;

    GameObject InfoObject;


    void Start()
    {
        InfoObject = MusicBox.musicBox.MusicInfoObjectList.Find
            (GameObject => GameObject.name == MainSystem.mainSystem.songName);

        var played = MainSystem.mainSystem.gamePlayed;

        jacket.sprite = InfoObject.GetComponent<SongInfo>().MusicJacket;

        Difficulty.sprite = DifficultyBoxSprite[MainSystem.mainSystem.difficultyNum];

        if (MainSystem.mainSystem.difficulty < 10)
        { difficultyNum.text = "0" + (MainSystem.mainSystem.difficulty).ToString(); }
        else { difficultyNum.text = (MainSystem.mainSystem.difficulty).ToString(); }


        string ScoreText = played.Score.ToString();
        int ScoreTextLength = ScoreText.Length;

        for (int i = 0; i < ScoreTextLength; i++)
        {
            Score[8 - i].text = ScoreText.Substring(ScoreTextLength - 1 - i, 1);
        }

        Count[0].text = played.LoF.ToString();
        Count[1].text = played.GRF.ToString();
        Count[2].text = played.PRF.ToString();
        Count[3].text = played.PRP.ToString();
        Count[4].text = played.PRL.ToString();
        Count[5].text = played.GRL.ToString();
        Count[6].text = played.LoL.ToString();

        maxCount.text = "max  " + played.Max.ToString() + "/"
            + InfoObject.GetComponent<SongInfo>().NoteCount[MainSystem.mainSystem.difficultyNum];

        prpCount.text = "prp  " + played.PRP.ToString() + "/"
            + InfoObject.GetComponent<SongInfo>().NoteCount[MainSystem.mainSystem.difficultyNum];

        ranking();

        SongName.text = MainSystem.mainSystem.songName;

        MainSystem.mainSystem.SavePlayedData();
    }

    void ranking()
    {
        var played = MainSystem.mainSystem.gamePlayed;

        try
        {
            int score = played.Score;

            if (score == 100000000)
            {
                Rank.sprite = RankSprite[0];

                if (played.PRP == InfoObject.GetComponent<SongInfo>().NoteCount[MainSystem.mainSystem.difficultyNum])
                {
                    Plus.sprite = PlusSprite[0];
                }
                else
                {
                    Plus.sprite = null;
                }
            }
            else if (played.Max == InfoObject.GetComponent<SongInfo>().NoteCount[MainSystem.mainSystem.difficultyNum])
            {
                Rank.sprite = RankSprite[1];
                Plus.sprite = PlusSprite[1];
            }
            else if (score >= 97500000)
            {
                Rank.sprite = RankSprite[2];
                Plus.sprite = PlusSprite[1];
            }
            else if (score >= 95000000)
            {
                Rank.sprite = RankSprite[2];
                Plus.sprite = null;
            }
            else if (score >= 92500000)
            {
                Rank.sprite = RankSprite[3];
                Plus.sprite = PlusSprite[1];
            }
            else if (score >= 90000000)
            {
                Rank.sprite = RankSprite[3];
                Plus.sprite = null;
            }
            else if (score >= 87500000)
            {
                Rank.sprite = RankSprite[4];
                Plus.sprite = PlusSprite[1];
            }
            else if (score >= 85000000)
            {
                Rank.sprite = RankSprite[4];
                Plus.sprite = null;
            }
            else if (score >= 82500000)
            {
                Rank.sprite = RankSprite[5];
                Plus.sprite = PlusSprite[1];
            }
            else if (score >= 80000000)
            {
                Rank.sprite = RankSprite[5];
                Plus.sprite = null;
            }
            else if (score >= 77500000)
            {
                Rank.sprite = RankSprite[6];
                Plus.sprite = PlusSprite[1];
            }
            else if (score >= 75000000)
            {
                Rank.sprite = RankSprite[6];
                Plus.sprite = null;
            }
            else
            {
                Rank.sprite = RankSprite[7];
                Plus.sprite = null;
            }
        }
        catch
        {

        }
    }
}
