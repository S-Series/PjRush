using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameResult : MonoBehaviour
{
    public static GameResult gameResult;

    [SerializeField] TextMeshPro[] GameResultTMP;

    [SerializeField] SpriteRenderer[] GameResultRenderer;

    [SerializeField] SpriteRenderer[] GameClearResultSprite;

    private void Awake()
    {
        gameResult = this;
    }

    public void ResultSetting()
    {
        Music music;
        music = MainSystem.NowOnMusic;

        MainSystem mainSystem;
        mainSystem = MainSystem.mainSystem;
        GameManager gameManager;
        gameManager = MainSystem.gameManager;
        SpriteManager spriteManager;
        spriteManager = MainSystem.spriteManager;

        int dif = mainSystem.difficultyNum;
        int score = MainSystem.gameManager.GamePlayScore;
        string scoreText = string.Format("{0:D9}", score);

        GameClearResultSprite[0].enabled = false;
        GameClearResultSprite[1].enabled = false;
        GameClearResultSprite[2].enabled = false;
        GameClearResultSprite[3].enabled = false;
        GameClearResultSprite[4].enabled = false;

        // Song Information ----------------------------------------------------------------
        GameResultTMP[00].text = music.MusicName;
        GameResultTMP[01].text = music.MusicArtist;
        // Difficulty       ----------------------------------------------------------------
        GameResultTMP[02].text = music.Difficulty[dif].ToString();
        // ScoreSetting     ----------------------------------------------------------------
        GameResultTMP[03].text = scoreText.Substring(0, 3);
        GameResultTMP[04].text = scoreText.Substring(3, 6);
        if (music.playHighScore[dif] <= score)
        {
            GameResultTMP[5].color = new Color32(255, 255, 255, 255);
            GameResultTMP[6].text 
                = "+" + string.Format("{D:09}", score - music.playHighScore[dif]);
        }
        else 
        { 
            GameResultTMP[5].color = new Color32(255, 255, 255, 100);
            GameResultTMP[6].text
                = "-" + string.Format("{D:09}", music.playHighScore[dif] - score);
        }
        // JudgeSetting     ----------------------------------------------------------------
        GameResultTMP[07].text = gameManager.Record[1].ToString();
        GameResultTMP[08].text = (gameManager.Record[0] + gameManager.Record[2]).ToString();
        GameResultTMP[09].text = (gameManager.Rough[0] + gameManager.Rough[1]).ToString();
        GameResultTMP[10].text = (gameManager.Lost[0] + gameManager.Lost[1]).ToString();
        GameResultTMP[11].text = gameManager.MaxSustain.ToString();
        GameResultTMP[12].text = music.NoteCount[dif].ToString();
        // User Information ----------------------------------------------------------------


        // Song Information ----------------------------------------------------------------
        GameResultRenderer[00].sprite = music.sprJacket;
        GameResultRenderer[01].sprite 
            = MainSystem.spriteManager.getDifficultySprite(dif);
        // Difficulty       ----------------------------------------------------------------
        GameResultRenderer[02].sprite = spriteManager.getRankSprite(dif);
        // ScoreSetting     ----------------------------------------------------------------
        GameResultRenderer[03].sprite = spriteManager.getRankSprite(getScoreIndex(score));
        if (GameManager.isAllPerfect) 
        { 
            if (gameManager.Record[0] == 0 && gameManager.Record[2] == 0)
            {
                GameClearResultSprite[0].enabled = true;    // Pure RecorD+
            }
            else
            {
                GameClearResultSprite[1].enabled = true;    // Pure Record
            }
        }
        else if (GameManager.isFullCombo) 
        { 
            GameClearResultSprite[2].enabled = true;        // Maximum Record
        }
        else if (GameManager.isHardGame)
        {
            if (gameManager.GamePlayClearRate == 0)
            {
                GameClearResultSprite[5].enabled = true;    // Record Recounce
            }
            else
            {
                GameClearResultSprite[3].enabled = true;    // exquisite Record
            }
        }
        else
        {
            if (gameManager.GamePlayClearRate >= 70.0f)
            {
                GameClearResultSprite[4].enabled = true;    // Record Complete
            }
            else
            {
                GameClearResultSprite[5].enabled = true;    // Record Recounce
            }
        }
        // JudgeSetting     ----------------------------------------------------------------
        // User Information ----------------------------------------------------------------
        GameResultRenderer[03].sprite = ;
    }

    private int getScoreIndex(int score)
    {
        if (GameManager.isAllPerfect) { return 00; } //P
        else if (GameManager.isFullCombo) { return 01; } //F
        else if (score >= 97500000) { return 02; } // S+
        else if (score >= 95000000) { return 03; } // S
        else if (score >= 92500000) { return 04; } // AA+
        else if (score >= 90000000) { return 05; } // AA
        else if (score >= 87500000) { return 06; } // A+
        else if (score >= 85000000) { return 07; } // A
        else if (score >= 82500000) { return 08; } // B+
        else if (score >= 80000000) { return 09; } // B
        else if (score >= 77500000) { return 10; } // C+
        else if (score >= 75000000) { return 11; } // C
        else { return 12; } // F
    }
}
