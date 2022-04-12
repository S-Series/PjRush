using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JudgeSystem : MonoBehaviour
{
    public List<Note> gameNote;
    public GamePlaySystem gamePlaySystem;
    public GamePlayScore gamePlayScore;
    int noteIndex;
    int longIndex;
    public static bool isOnPlay;
    private bool isLongJudge;
    [SerializeField] KeyCode MainKey;
    [SerializeField] KeyCode SubKey;
    private void Awake()
    {
        isLongJudge = false;
        gameNote = new List<Note>();
    }
    void Update(){
        if (isOnPlay)
        {
            try
            {
                if (gameNote[longIndex].ms <= GamePlaySystem.playMs){
                    if (gameNote[longIndex].legnth != 0){
                        StartCoroutine(ILongStart(longIndex, gameNote[longIndex].legnth, gameNote[0].line));
                    }
                    longIndex++;
                }
            }
            catch { }

            float msDif;
            try { 
                msDif = gameNote[noteIndex].ms - GamePlaySystem.playMs; 
            }
            catch { return; }

            if (Input.GetKeyDown(MainKey) || Input.GetKeyDown(SubKey))
            {
                isLongJudge = true;
                try
                {
                    if (msDif <= 110.0f && msDif >= -90.0f)
                    {
                        gameNote[noteIndex].noteObject.SetActive(false);
                        NoteJudge(msDif, gameNote[noteIndex].line);
                        noteIndex++;
                    }
                }
                catch { }
            }

            if (Input.GetKeyUp(MainKey) || Input.GetKeyUp(SubKey))
            {
                isLongJudge = false;
            }

            if (msDif < -90.0f){
                gamePlaySystem.JudgeApply(0, gameNote[noteIndex].line);
                print("running");
                noteIndex++;
            }
        }
    }
    public void Setkey(KeyCode main, KeyCode sub)
    {
        MainKey = main;
        SubKey = sub;
    }
    private IEnumerator ILongStart(int Index, int legnth, int line)
    {
        float delay = 15 / GamePlaySystem.testBpm;
        yield return new WaitForSeconds(delay);
        for (int i = 1; i < legnth; i++)
        {
            if (isLongJudge){
                gamePlaySystem.JudgeApply(0, line);
            }
            else{
                gamePlaySystem.JudgeApply(0, line);
            }
            yield return new WaitForSeconds(delay);
        }
    }
    private void NoteJudge(float judge, int line)
    {
        if (Mathf.Abs(judge) <= 22.5f)
        {
            gamePlaySystem.JudgeApply(0, line);
        }
        else if (Mathf.Abs(judge) <= 45.0f)
        {
            if (judge > 0)
            {
                gamePlaySystem.JudgeApply(1, line);
            }
            else
            {
                gamePlaySystem.JudgeApply(-1, line);
            }
        }
        else if (Mathf.Abs(judge) <= 90.0f)
        {
            if (judge > 0)
            {
                gamePlaySystem.JudgeApply(2, line);
            }
            else
            {
                gamePlaySystem.JudgeApply(2, line);
            }
        }
        else
        {
            gamePlaySystem.JudgeApply(3, line);
        }
    }
}
