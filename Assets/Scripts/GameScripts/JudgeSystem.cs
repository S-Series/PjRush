using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JudgeSystem : MonoBehaviour
{
    public List<Note> gameNote;
    public List<Note> gameLongNote;

    public int noteIndex;
    public int longIndex;

    [SerializeField]
    KeyCode MainKey;
    [SerializeField]
    KeyCode SubKey;

    private void Awake()
    {
        gameNote = new List<Note>();
        gameLongNote = new List<Note>();
    }

    void Update()
    {
        if (Input.GetKeyDown(MainKey) || Input.GetKeyDown(SubKey))
        {

        }

        if (Input.GetKeyUp(MainKey) || Input.GetKeyUp(SubKey))
        {

        }
    }

    private void LateUpdate()
    {
        if (gameLongNote[longIndex].ms <= GamePlaySystem.playMs)
        {
            StartCoroutine(ILongStart(longIndex));
            longIndex++;
        }
    }

    public void Setkey(KeyCode main, KeyCode sub)
    {
        MainKey = main;
        SubKey = sub;
    }

    private IEnumerator ILongStart(int Index)
    {
        float delay = GamePlaySystem.testBpm;
        for (int i = 0; i < gameLongNote[Index].legnth; i++)
        {
            if ()
            yield return new WaitForSeconds(delay);

        }
    }

    private void NoteJudge(float judge)
    {
        if (Mathf.Abs(judge) <= 22.5f)
        {

        }
        else if (Mathf.Abs(judge) <= 45.0f)
        {

        }
        else if (Mathf.Abs(judge) <= 90.0f)
        {

        }
        else if (judge < -90.0f)
        {

        }
    }
}
