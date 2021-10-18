using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainJudgeManage : MonoBehaviour
{
    public static MainJudgeManage mainJudge;

    public bool NoteJudgeTrigger;

    public int GameMS;

    public int GameEndMS;

    public int PerfectRushPlus;

    public int PerfectRush_Fast;
    public int PerfectRush_Late;

    public int GreatRush_Fast;
    public int GreatRush_Late;

    public int Lost_Fast;
    public int Lost_Late;

    [SerializeField]
    GameObject JudgeSystem;

    private void Awake()
    {
        if (mainJudge != this)
        {
            mainJudge = this;
        }
    }

    void Start()
    {
        GameMS = 0;
        NoteJudgeTrigger = true;
        JudgeSystem = this.gameObject.transform.GetChild(0).gameObject;
        ResetJudge();
    }

    void OnEnable()
    {
        GameMS = 0;
        NoteJudgeTrigger = true;
        JudgeSystem = this.gameObject.transform.GetChild(0).gameObject;
        ResetJudge();
    }

    void FixedUpdate() // 1000 update per second
    {

        if (NoteJudgeTrigger == true)
        {
            JudgeSystem.SetActive(true);
        }
        else
        {
            JudgeSystem.SetActive(false);
        }

        if (NoteMove.noteMove.NoteMovingTrigger == true)
        {
            GameMS++;

            if (GameMS >= GameEndMS)
            {
                //MainSystem.mainSystem.SceneAnimate.SetTrigger("ChangeIn");
                GameEnd();
                NoteJudgeTrigger = false;
                GameEndMS = 999999999;
            }
        }
    }

    private void ResetJudge()
    {
        PerfectRushPlus = 0;
        PerfectRush_Fast = 0;
        PerfectRush_Late = 0;
        GreatRush_Fast = 0;
        GreatRush_Late = 0;
        Lost_Fast = 0;
        Lost_Late = 0;
    }

    private void GameEnd()
    {
        Debug.Log("종료실행");
        GamePlayed gamePlayed;
        gamePlayed = MainSystem.mainSystem.gamePlayed;

        gamePlayed.PRP = PerfectRushPlus;
        gamePlayed.PRF = PerfectRush_Fast;
        gamePlayed.PRL = PerfectRush_Late;
        gamePlayed.GRF = GreatRush_Fast;
        gamePlayed.GRL = GreatRush_Late;
        gamePlayed.LoF = Lost_Fast;
        gamePlayed.LoL = Lost_Late;

        MainSystem.mainSystem.gamePlayed.Score
            = Convert.ToInt32(ScoreManage.scoreManage.systemScore);
        MainSystem.mainSystem.gamePlayed.Max
            = ComboManager.comboManager.maxCombo;

        StartCoroutine(LoadSceneToResult());
    }

    IEnumerator LoadSceneToResult()
    {
        yield return new WaitForSeconds(5.0f);

        SceneManager.LoadScene("Result");
    }
}
