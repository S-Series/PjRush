using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameResult : MonoBehaviour
{
    [SerializeField] private SpriteRenderer[] resultRenderer;
    [SerializeField] private TextMeshPro[] infoTmp;
    [SerializeField] private TextMeshPro[] resultTmp;
    [SerializeField] private TextMeshPro[] scoreTmp;
    public static GameResult gameResult;
    private void Awake() { gameResult = this; }
    public void DisplayResult()
    {
        //* 점수 정보 세팅
        char[] _scoreChar;
        _scoreChar = (string.Format("{0:D9}", GameManager.s_GameScore)).ToCharArray();
        for (int i = 0; i < 9; i++) { scoreTmp[0].text = _scoreChar[i].ToString(); }

        //* 음악 정보 세팅
        resultRenderer[0].sprite = GameManager.s_OnGameMusic.sprJacket;
        resultRenderer[1].sprite = SpriteManager.getDifficultySprite(GameManager.s_OnGameDifficultyIndex);
        infoTmp[0].text = GameManager.s_OnGameMusic.MusicName;
        infoTmp[1].text = GameManager.s_OnGameMusic.MusicArtist;
        infoTmp[2].text = GameManager.s_OnGameMusic.MusicName;

        //* 판정 정보 세팅
        int _MaxCombo;
        _MaxCombo = GameManager.s_DetailPerfectJudgeCount
            + GameManager.s_PerfectJudgeCount[0] + GameManager.s_PerfectJudgeCount[1]
            + GameManager.s_IndirectJudgeCount[0] + GameManager.s_IndirectJudgeCount[1]
            + GameManager.s_LostedJudgeCount[0] + GameManager.s_LostedJudgeCount[1];
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
}
