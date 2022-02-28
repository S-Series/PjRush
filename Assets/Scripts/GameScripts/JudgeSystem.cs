using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JudgeSystem : MonoBehaviour
{
    public List<Note> gameNote;
    public List<Note> gameLongNote;

    public int noteIndex;
    public int longIndex;

    KeyCode MainKey;
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

    private IEnumerator ILongStart(int Index)
    {
        yield return new WaitForSeconds(0);
    }

    public void Setkey(KeyCode main, KeyCode sub)
    {
        MainKey = main;
        SubKey = sub;
    }
}
