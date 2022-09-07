using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameResult : MonoBehaviour
{
    [SerializeField] private SpriteRenderer[] resultRenderer;
    [SerializeField] private SpriteRenderer[] gameResultRenderer;
    [SerializeField] private TextMeshPro[] infoTmp;
    [SerializeField] private TextMeshPro[] userTmp;
    [SerializeField] private TextMeshPro[] judgeTmp;
    [SerializeField] private TextMeshPro[] scoreTmp;
    [SerializeField] private TextMeshPro[] plusScoreTmp;
    public static GameResult gameResult;
    private void Awake() { gameResult = this; }
    public void DisplayResult()
    {
        int _difficultyIndex;
        _difficultyIndex = GameManager.s_OnGameDifficultyIndex;
        Music _nowMusic;
        _nowMusic = GameManager.s_OnGameMusic;

        //* 점수 정보 세팅
        char[] _scoreChar;
        _scoreChar = (string.Format("{0:D9}", GameManager.s_GameScore)).ToCharArray();
        for (int i = 0; i < 9; i++) { scoreTmp[i].text = _scoreChar[i].ToString(); }

        int _plusScore;
        _plusScore = GameManager.s_GameScore - _nowMusic.HighScore[_difficultyIndex];
        char[] _plusScoreChar;
        _plusScoreChar = (string.Format("{0:D9}", GameManager.s_GameScore)).ToCharArray();
        for (int i = 0; i < 9; i++) { plusScoreTmp[i].text = (Mathf.Abs(_plusScoreChar[i])).ToString(); }
        if (_plusScore >= 0) 
            { plusScoreTmp[9].text = "+"; plusScoreTmp[10].color = new Color32(255, 255, 255, 255); }
        else 
            { plusScoreTmp[9].text = "-"; plusScoreTmp[10].color = new Color32(255, 255, 255, 100); }

        //* 음악 정보 세팅
        resultRenderer[0].sprite = GameManager.s_OnGameMusic.sprJacket;
        resultRenderer[1].sprite = SpriteManager.getDifficultySprite(_difficultyIndex);

        infoTmp[0].text = _nowMusic.MusicName;
        infoTmp[1].text = _nowMusic.MusicArtist;
        infoTmp[2].text = string.Format("{0:D2}", _nowMusic.Difficulty[_difficultyIndex]);
        infoTmp[3].text = SpriteManager.getDifficultyText(_difficultyIndex);

        //* 게임 결과 세팅
        foreach(SpriteRenderer renderer in gameResultRenderer) { renderer.enabled = false; }
        if (GameManager.s_isDetailPerfect) { gameResultRenderer[0].enabled = true; }
        else if (GameManager.s_isPerfect) { gameResultRenderer[1].enabled = true; }
        else if (GameManager.s_isMaximum) { gameResultRenderer[2].enabled = true; }
        else if (GameManager.s_isComplete) 
        { 
            if (GameManager.s_isHardMode) { gameResultRenderer[3].enabled = true; }
            else { gameResultRenderer[4].enabled = true; }
        }
        else { gameResultRenderer[5].enabled = true; }
        resultRenderer[2].sprite = SpriteManager.getRankSprite(getScoreIndex(GameManager.s_GameScore));

        //* 판정 정보 세팅
        int _MaxCombo;
        _MaxCombo = GameManager.s_DetailPerfectJudgeCount
            + GameManager.s_PerfectJudgeCount[0] + GameManager.s_PerfectJudgeCount[1]
            + GameManager.s_IndirectJudgeCount[0] + GameManager.s_IndirectJudgeCount[1]
            + GameManager.s_LostedJudgeCount[0] + GameManager.s_LostedJudgeCount[1];
        judgeTmp[0].text = string.Format("{0:D4}", GameManager.s_DetailPerfectJudgeCount);
        judgeTmp[1].text  = string.Format("{0:D4}",
            GameManager.s_PerfectJudgeCount[0] + GameManager.s_PerfectJudgeCount[1]);
        judgeTmp[2].text  = string.Format("{0:D4}",
            GameManager.s_IndirectJudgeCount[0] + GameManager.s_IndirectJudgeCount[1]);
        judgeTmp[3].text  = string.Format("{0:D4}",
            GameManager.s_LostedJudgeCount[0] + GameManager.s_LostedJudgeCount[1]);
        judgeTmp[4].text = GameManager.s_MaxCombo.ToString();
        judgeTmp[5].text = _MaxCombo.ToString();

        //* 유저 정보 세팅
        // resultRenderer[3].sprite = SpriteManager.getCharacterSprite(UserManager.characterIndex);
        // resultRenderer[4].sprite = SpriteManager.getCharacterIconSprite(UserManager.characterIndex);

        userTmp[0].text = UserManager.UserInfoData.s_userName;
        userTmp[1].text = "LV." + string.Format("{0:D4}", UserManager.UserInfoData.s_userLevel);

        StartCoroutine(IStartInput());
    }
    private int getScoreIndex(int score)
    {
        if (score >= 99500000) { return 00; }               // S+
        else if (score >= 99000000) { return 01; }          // S
        else if (score >= 97500000) { return 02; }          // AA+
        else if (score >= 95000000) { return 03; }          // AA
        else if (score >= 92500000) { return 04; }          // A+
        else if (score >= 90000000) { return 05; }          // A
        else if (score >= 85000000) { return 06; }          // B
        else if (score >= 80000000) { return 07; }          // C
        else { return 08; }                                 // D
    }
    private IEnumerator IStartInput()
    {
        yield return new WaitForSeconds(2.0f);
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
            {
                MainSystem.LoadSelectScene();
                yield break;
            }
            if (Input.GetKeyDown(KeyCode.F5))
            {
                MainSystem.LoadGameScene();
                yield break;
            }
            yield return null;
        }
    }
}
