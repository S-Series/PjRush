﻿using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteData : MonoBehaviour
{
    private static NoteFile s_noteFile = new NoteFile();
    public static List<NormalNote> s_normalNotes = new List<NormalNote>();
    public static List<SpeedNote> s_speedNotes = new List<SpeedNote>();
    public static List<EffectNote> s_effectNotes = new List<EffectNote>();

    public static void getNoteData()
    {
        s_noteFile = new NoteFile();
        s_normalNotes = new List<NormalNote>();
        s_speedNotes = new List<SpeedNote>();
        s_effectNotes = new List<EffectNote>();
        GamePlaySystem.gamePlaySystem.ClearNoteField();
        string path;
        try
        {
            path = "NoteBox/" + string.Format("{0:D4}", GameManager.s_OnGameMusic.MusicID) 
            + "/" + string.Format("{0:D4}", GameManager.s_OnGameDifficultyIndex + 1);
            s_noteFile = JsonUtility.FromJson<NoteFile>((Resources.Load<TextAsset>(path)).ToString());
        }
        catch { throw new System.Exception("Music File None Exist"); }
        GenerateNotes();
    }
    private static void GenerateNotes()
    {
        GamePlaySystem.gamePlaySystem.ClearNoteField();
        GameManager.s_bpm = s_noteFile.bpm;
        GameManager.s_delay = s_noteFile.startDelayMs;
        GamePlaySystem.s_GameMusic.clip = GameManager.s_OnGameMusic.audMusicFile;

        int _noteChain = 0;
        for (int i = 0; i < s_noteFile.NoteMs.Count; i++)
        {
            NormalNote normalNote = new NormalNote();
            normalNote.noteObject = null;
            normalNote.ms = s_noteFile.NoteMs[i];
            normalNote.line = s_noteFile.NoteLine[i];
            normalNote.legnth = s_noteFile.NoteLegnth[i];
            try { normalNote.pos = s_noteFile.NotePos[i]; }
            catch { normalNote.pos = s_noteFile.bpm * normalNote.ms / 150.0f; }
            try { normalNote.isPowered = s_noteFile.NotePowered[i]; }
            catch { normalNote.isPowered = false; }
            s_normalNotes.Add(normalNote);

            if (normalNote.legnth == 0) { _noteChain++; }
            else { _noteChain += normalNote.legnth; }
        }
        GameInfoField.gameInfoField.maxCount = _noteChain;
        print(_noteChain);

        if (s_normalNotes.Count != 0)
        {
            if (s_normalNotes[0].pos < 1600.0f)
            {
                foreach (NormalNote _note in s_normalNotes) { _note.pos += 1600.0f; }
            }
        }
        for (int i = 0; i < s_noteFile.SpeedMs.Count; i++)
        {
            SpeedNote speedNote = new SpeedNote();
            speedNote.ms = s_noteFile.SpeedMs[i];
            speedNote.pos = s_noteFile.SpeedPos[i];
            speedNote.bpm = s_noteFile.SpeedBpm[i];
            speedNote.multiply = s_noteFile.SpeedNum[i];
            s_speedNotes.Add(speedNote);
        }
        for (int i = 0; i < s_noteFile.EffectMs.Count; i++)
        {
            EffectNote effectNote = new EffectNote();
            effectNote.ms = s_noteFile.EffectMs[i];
            effectNote.pos = s_noteFile.EffectPos[i];
            effectNote.isPause = s_noteFile.EffectIsPause[i];
            effectNote.value = s_noteFile.EffectForce[i];
            s_effectNotes.Add(effectNote);
        }
        //*--------------------------------------
        for (int i = 0; i < s_normalNotes.Count; i++)
        {
            NormalNote normalNote;
            GameObject copyObject;

            Vector3 autoPos = new Vector3(0, 0, 0);
            Vector3 autoScale = new Vector3(1, 1, 1);

            normalNote = s_normalNotes[i];
            if (normalNote.line >= 5)
            {
                copyObject = Instantiate(GamePlaySystem.gamePlaySystem.notePrefab[2],
                    GamePlaySystem.gamePlaySystem.noteGenerateField[normalNote.line]);
                if (normalNote.legnth == 0) 
                    { copyObject.transform.GetChild(0).gameObject.SetActive(false); }
                else
                {
                    copyObject.transform.GetChild(0).localScale = new Vector3
                    (1.0f, 0.44046875f * normalNote.legnth * (GameManager.s_Multiply / 100.0f), 1.0f);
                }
                if (normalNote.line == 6) { copyObject.GetComponent<SpriteRenderer>().flipX = true; }
            }
            else
            {
                if (normalNote.isPowered) 
                    { copyObject = Instantiate(GamePlaySystem.gamePlaySystem.notePrefab[1],
                    GamePlaySystem.gamePlaySystem.noteGenerateField[normalNote.line]); }
                else 
                    { copyObject = Instantiate(GamePlaySystem.gamePlaySystem.notePrefab[0],
                    GamePlaySystem.gamePlaySystem.noteGenerateField[normalNote.line]); }
                
                if (normalNote.legnth == 0) 
                    { copyObject.transform.GetChild(0).gameObject.SetActive(false); }
                else
                {
                    copyObject.transform.GetChild(0).localScale = new Vector3
                    (1.0f, 0.44046875f * normalNote.legnth * (GameManager.s_Multiply / 100.0f), 1.0f);
                }
            }

            autoPos.x = 0.0f;
            autoPos.y = normalNote.pos * (GameManager.s_Multiply / 100.0f) / 100.0f;
            autoPos.z = 0.0f;
            copyObject.transform.localPosition = autoPos;

            normalNote.noteObject = copyObject;
        }
        LineDivisionNotes();
    }
    private static void LineDivisionNotes()
    {
        foreach(JudgeSystem judgeSystem in GamePlaySystem.gamePlaySystem.judgeSystems)
            { judgeSystem.notes = new List<NormalNote>(); }
        for (int i = 0; i < s_normalNotes.Count; i++)
        {
            GamePlaySystem.gamePlaySystem
                .judgeSystems[s_normalNotes[i].line - 1].notes.Add(s_normalNotes[i]);
        }
    }
}

public class NormalNote
{
    public GameObject noteObject;
    public int line;
    public int legnth;
    public int ms;
    public float pos;
    public bool isPowered;
    public static void Sorting()
    {
        NoteData.s_normalNotes.Sort(delegate (NormalNote A, NormalNote B)
        {
            if (Mathf.Approximately(A.pos, B.pos))
            {
                if (!A.isPowered && B.isPowered) return +1;
                else if (A.isPowered && !B.isPowered) return -1;
            }
            if (A.pos > B.pos) return +1;
            else if (A.pos < B.pos) return -1;
            else
            {
                if (A.line > B.line) return +1;
                else if (A.line < B.line) return -1;
                else return 0;
            }
        });
    }
}
public class SpeedNote
{
    public float bpm;
    public float multiply;
    public float ms;
    public float pos;
    public static void Sorting()
    {
        NoteData.s_speedNotes.Sort(delegate (SpeedNote A, SpeedNote B)
        {
            if (A.pos > B.pos) return +1;
            else if (A.pos < B.pos) return -1;
            else {Debug.LogError("Note Overlap"); return 0;}
        });
    }
}
public class EffectNote
{
    public bool isPause;
    public float value;
    public float ms;
    public float pos;
    public static void Sorting()
    {
        NoteData.s_effectNotes.Sort(delegate (EffectNote A, EffectNote B)
        {
            //if (Mathf.Approximately(A.pos, B.pos)) {Debug.Log("Note Overlap"); return 0;}

            if (A.pos > B.pos) { return +1; }
            else if (A.pos < B.pos) { return -1; }
            else
            {
                if (A.isPause && !B.isPause) { return +1; }
                else if (!A.isPause && B.isPause) { return -1; }
                else return 0;
            }
        });
    }
}
public class NoteFile
{
    public string Version;
    public float bpm;
    public int startDelayMs;

    public List<int> NoteLegnth = new List<int>();
    public List<int> NoteMs = new List<int>();
    public List<float> NotePos = new List<float>();
    public List<int> NoteLine = new List<int>();
    public List<bool> NotePowered = new List<bool>();

    public List<float> EffectMs = new List<float>();
    public List<float> EffectPos = new List<float>();
    public List<float> EffectForce = new List<float>();
    public List<bool> EffectIsPause = new List<bool>();

    public List<float> SpeedMs = new List<float>();
    public List<float> SpeedPos = new List<float>();
    public List<float> SpeedBpm = new List<float>();
    public List<float> SpeedNum = new List<float>();
}