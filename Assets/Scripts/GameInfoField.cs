using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameInfoField : MonoBehaviour
{
    public static GameInfoField gameInfoField;
    public Animator GameInfoSettingAnimator;
    public TextMeshPro[] ScoreText;
    public TextMeshPro PureScoreText;

    [SerializeField] SpriteRenderer infoJacket;
    [SerializeField] TextMeshPro[] infoGameInfoTMP;
    [SerializeField] Sprite[] difficultySprite;
    [SerializeField] GameObject difficultyObject;

    public static int s_score;
    public int maxCount;
    private float judgeCount;
    private int semiCount;

    private void Awake()
    {
        gameInfoField = this;
        GameInfoSettingAnimator = GetComponent<Animator>();
    }
    public void InfoSetting()
    {
        GameInfoSettingAnimator.SetTrigger("Play");
        int index = GameManager.s_OnGameDifficultyIndex;
        Music music = GameManager.s_OnGameMusic;

        infoJacket.sprite = music.sprJacket;

        infoGameInfoTMP[0].text = music.MusicName;
        infoGameInfoTMP[1].text = music.MusicArtist;

        difficultyObject.GetComponent<SpriteRenderer>().sprite 
            = difficultySprite[index];
        difficultyObject.GetComponentInChildren<TextMeshPro>().text 
            = music.Difficulty[index].ToString();
        StartCoroutine(IUpdateScoreInfo());
    }
    public static void AddScore(int _judgeType)
    {
        print("A");
        if (_judgeType == 0) 
        {
            gameInfoField.StartCoroutine(gameInfoField.IAddScore(20, true));
        }
        else if (_judgeType == 1) 
        {
            gameInfoField.StartCoroutine(gameInfoField.IAddScore(20));
        }
        else if (_judgeType == 2) 
        {
            gameInfoField.StartCoroutine(gameInfoField.IAddScore(10));
        }
        else { throw new System.Exception("Wrong JudgeType"); }
    }
    private void UpdateScoreInfo()
    {
        s_score = Convert.ToInt32((judgeCount / maxCount * 5000000.0f)) + semiCount;

        char[] _scoreChar;
        _scoreChar =  (string.Format("{0:D9}", s_score)).ToCharArray();
        for (int i = 0; i < 9; i ++) { ScoreText[i].text = _scoreChar[i].ToString(); }

        GameManager.s_GameScore = s_score;
    }
    private IEnumerator IAddScore(int _count, bool _isSemiJudge = false)
    {
        if (_isSemiJudge) { semiCount++; }
        for (int i = 0; i < _count; i++)
        {
            judgeCount++;
            yield return null;
        }
    }
    private IEnumerator IUpdateScoreInfo()
    {
        while(true)
        {
            UpdateScoreInfo();
            yield return null;
            yield return null;
        }
    }
}
