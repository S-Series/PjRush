using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManage : MonoBehaviour
{
    public static ScoreManage scoreManage; 

    public double systemScore;
    public int visualScore;

    public int NoteCount;
    public int semiPerfect;

    [SerializeField]
    TextMeshPro[] ScoreText;

    [SerializeField]
    private int[] ScoreNum = new int[8];

    [SerializeField]
    private int[] ScoreNumPart = new int[9];

    [SerializeField]
    private int[] visualScorePart = new int[9];

    private int ScoreColorUpdateNum;
    private bool isScoreColorUpdate;

    private string ScoreGap;

    MainJudgeManage mainJudge;

    private void Awake()
    {
        scoreManage = this;
    }

    void Start()
    {
        ScoreColorUpdateNum = 1;
        isScoreColorUpdate = true;

        systemScore = 0;
        visualScore = 0;

        for (int i = 0; i < 9; i++)
        {
            ScoreNum[i] = 0;
            ScoreNumPart[i] = 0;
            visualScorePart[i] = 0;
            ScoreText[i].text = "0";
            ScoreText[i].color = new Color32(255, 255, 255, 100);
        }
        mainJudge = MainJudgeManage.mainJudge;

        StartCoroutine(Scoring());
    }

    private void Update()
    {
        try
        {
            systemScore = 100000000 / NoteCount * (mainJudge.PerfectRushPlus +
            mainJudge.PerfectRush_Fast + mainJudge.PerfectRush_Late +
            (mainJudge.GreatRush_Fast + mainJudge.GreatRush_Late) * 0.5);
        }
        catch { systemScore = 0; }

        semiPerfect = MainJudgeManage.mainJudge.PerfectRushPlus;

        if (isScoreColorUpdate == true)
        {
            switch (ScoreColorUpdateNum)
            {
                case 1:
                    if (visualScore >= 1)
                    {
                        ScoreText[8].color = new Color32(255, 255, 255, 255);
                        ScoreColorUpdateNum++;
                    }
                    break;
                case 2:
                    if (visualScore >= 10)
                    {
                        ScoreText[7].color = new Color32(255, 255, 255, 255);
                        ScoreColorUpdateNum++;
                    }
                    break;
                case 3:
                    if (visualScore >= 100)
                    {
                        ScoreText[6].color = new Color32(255, 255, 255, 255);
                        ScoreColorUpdateNum++;
                    }
                    break;
                case 4:
                    if (visualScore >= 1000)
                    {
                        ScoreText[5].color = new Color32(255, 255, 255, 255);
                        ScoreColorUpdateNum++;
                    }
                    break;
                case 5:
                    if (visualScore >= 10000)
                    {
                        ScoreText[4].color = new Color32(255, 255, 255, 255);
                        ScoreColorUpdateNum++;
                    }
                    break;
                case 6:
                    if (visualScore >= 100000)
                    {
                        ScoreText[3].color = new Color32(255, 255, 255, 255);
                        ScoreColorUpdateNum++;
                    }
                    break;
                case 7:
                    if (visualScore >= 1000000)
                    {
                        ScoreText[2].color = new Color32(255, 255, 255, 255);
                        ScoreColorUpdateNum++;
                    }
                    break;
                case 8:
                    if (visualScore >= 10000000)
                    {
                        ScoreText[1].color = new Color32(255, 255, 255, 255);
                        ScoreColorUpdateNum++;
                    }
                    break;
                case 9:
                    if (visualScore >= 100000000)
                    {
                        ScoreText[0].color = new Color32(255, 255, 255, 255);
                        ScoreColorUpdateNum++;
                    }
                    break;
                case 10:
                    isScoreColorUpdate = false;
                    break;

                default:
                    ScoreColorUpdateNum = 1;
                    break;
            }
        }
    }

    private void VisualScore()
    {
        double A = (systemScore - visualScore) - (systemScore - visualScore) % 1;

        if (A < 0) { A = 0; }

        ScoreGap = A.ToString();
        int ScoreGapLength = ScoreGap.Length;

        for (int i = 0; i < ScoreGapLength; i++)
        {
            ScoreNum[8 - i] = int.Parse(ScoreGap.Substring(ScoreGapLength - 1 - i, 1));
        }

        for (int i = 0; i < 9; i++)
        {
            if (ScoreNumPart[i] != visualScorePart[i]) { visualScorePart[i]++; }

            if (visualScorePart[i] >= 10) { visualScorePart[i] -= 10; }
        }

        visualScore =
            visualScorePart[0] * 100000000 +
            visualScorePart[1] * 10000000 +
            visualScorePart[2] * 1000000 +
            visualScorePart[3] * 100000 +
            visualScorePart[4] * 10000 +
            visualScorePart[5] * 1000 +
            visualScorePart[6] * 100 +
            visualScorePart[7] * 10 +
            visualScorePart[8] * 1;
    }

    private void ScoreOutput()
    {
        string VisualScoreText = (visualScore - visualScore % 1).ToString();
        int VisualScoreTextLength = VisualScoreText.Length;

        for (int i = 0; i < VisualScoreTextLength; i++)
        {
            ScoreNum[8 - i] = int.Parse(VisualScoreText.Substring(VisualScoreTextLength - 1 - i, 1));

            if (ScoreNum[8 - i] >= 10)
            {
                ScoreNum[8 - i] -= 10;
            }

            ScoreText[8 - i].text = (ScoreNum[8 - i]).ToString();
        }
    }

    private void getScore()
    {
        string SystemScoreText = (systemScore - systemScore % 1).ToString();
        int SystemScoreTextLength = SystemScoreText.Length;

        for (int i = 0; i < SystemScoreTextLength; i++)
        {
            ScoreNumPart[8 - i] = int.Parse(SystemScoreText.Substring(SystemScoreTextLength - 1 - i, 1));

            if (ScoreNumPart[8 - i] >= 10)
            {
                ScoreNumPart[8 - i] -= 10;
            }
        }
    }

    IEnumerator Scoring()
    {
        yield return new WaitForSeconds(2.0f);

        var wait = new WaitForSeconds(0.025f);
        //0.01초가 보장될 필요 X
        while (true)
        {
            yield return wait;
            getScore();
            ScoreOutput();
            VisualScore();
        }
    }
}
