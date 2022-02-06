using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteMove : MonoBehaviour
{
    public static NoteMove noteMove;

    public float GameSpeed;

    public float startDelay;

    public bool NoteMovingTrigger;

    [SerializeField]
    AudioSource gameAudio;

    private void Awake()
    {
        noteMove = this;
        NoteMovingTrigger = false;
    }

    private void Start()
    {
        StartCoroutine(noteMoveStart());
    }

    void FixedUpdate()
    {
        if (NoteMovingTrigger == true)
        {
            NotefileLoad.noteLoad.NoteMoveField.gameObject.transform.localPosition
                = new Vector3(0, -GameSpeed * MainJudgeManage.mainJudge.GameMS / 150, -592.5955f);
        }
    }

    public IEnumerator noteMoveStart()
    {
        yield return new WaitForSeconds(3.0f);

        foreach(Animator animate in MainSystem.mainSystem.GameAnimator)
        {
            try
            {
                animate.SetTrigger("GameStart");
            }
            catch { }
        }

        yield return new WaitForSeconds(10.0f);
        GameSpeed = Convert.ToSingle(MainSystem.mainSystem.bpm * MainSystem.mainSystem.highSpeed);
        NoteMovingTrigger = true;

        yield return new WaitForSeconds(startDelay / 100);
        gameAudio.Play();

        LineTilt.lineTilt.TiltSpeed = Convert.ToSingle(MainSystem.mainSystem.bpm / 100.0);
    }
}
