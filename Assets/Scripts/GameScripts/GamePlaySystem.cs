using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePlaySystem : MonoBehaviour
{
    public static GamePlaySystem gamePlay;
    NoteSavedData noteSaved = new NoteSavedData();

    [SerializeField]
    JudgeSystem[] judgeSystem;

    public static bool isPlay;
    public static float testBpm;
    public static float gameSpeed;
    public GameInfoField gameInfo;

    private List<float> GuidePos;

    public static int playMs;
    float bpm;
    [SerializeField]
    float TestSpeedPos;
    [SerializeField]
    float TestSpeedMs;

    public int[] Rush = new int[3] { 0, 0, 0 };
    public int[] Step = new int[2] { 0, 0 };
    public int[] Lost = new int[2] { 0, 0 };

    [SerializeField]
    GameObject NoteField;
    GameObject MovingNoteField;

    [SerializeField]
    GameObject[] PrefabObject;

    AudioSource gameMusic;

    private void Awake()
    {
        gamePlay = this;
        MovingNoteField = NoteField.transform.parent.gameObject;
        gameMusic = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        if (isPlay)
        {
            playMs++;
        }
    }

    private void Update()
    {
        if (isPlay)
        {
            try
            {
                if (Speed.speedNote[Speed.speedIndex].ms <= playMs)
                {
                    testBpm = Speed.speedNote[Speed.speedIndex].gameSpeed;
                    //LongDelay = 15 / SpeedBpm[SpeedIndex];
                    TestSpeedMs = Speed.speedNote[Speed.speedIndex].ms;
                    TestSpeedPos = Speed.speedNote[Speed.speedIndex].pos;
                    Speed.speedIndex++;
                }
            }
            catch { }

            float posY;
            posY = TestSpeedPos + ((playMs - TestSpeedMs) * testBpm / 150);
            MovingNoteField.transform.localPosition = new Vector3(0, -posY * gameSpeed, 0);

            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                Debug.Log("down");
                gameSpeed--;
                SpeedSetting();
            }

            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                Debug.Log("up");
                gameSpeed++;
                SpeedSetting();
            }
        }
    }

    [ContextMenu("Load")]
    public IEnumerator ILoadDataFromJson()
    {
        gameMusic.clip = MainSystem.NowOnMusic.audMusicFile;

        for (int i = 0; i < 5; i++)
        {
            judgeSystem[i].Setkey
                (KeySetting.keys[(KeyActions)i], KeySetting.keys[(KeyActions)i + 5]);
        }

        ResetSavedData();
        try
        {
            string path = Path.Combine(Application.dataPath, "NoteBox/" + MainSystem.NowOnMusic.MusicName
                + "/" + (MainSystem.mainSystem.difficulty + 1).ToString() + ".json");
            Debug.Log(path);
            string jsonData = File.ReadAllText(path);
            print(path);
            noteSaved = JsonUtility.FromJson<NoteSavedData>(jsonData);
        }
        catch
        {
            Debug.LogError("NoteFile Note Founded");
            yield break;
        }

        yield return new WaitForSeconds(.5f);

        for (int i = 0; i < Note.noteObject.Count; i++)
        {
            Destroy(Note.noteObject[i]);
        }

        testBpm = noteSaved.bpm;
        Speed.speedIndex = 0;
        Note.listNote = new List<Note>();
        Note.noteObject = new List<GameObject>();
        Speed.speedNote = new List<Speed>();

        for (int i = 0; i < noteSaved.NoteMs.Count; i++)
        {
            Note note = new Note();

            GameObject copy;
            if (noteSaved.NoteLegnth[i] == 0)
            {
                if (noteSaved.NoteLine[i] <= 4)
                {
                    copy = Instantiate(PrefabObject[0], NoteField.transform);
                }
                else
                {
                    copy = Instantiate(PrefabObject[2], NoteField.transform);
                }
            }
            else
            {
                if (noteSaved.NoteLine[i] <= 4)
                {
                    copy = Instantiate(PrefabObject[1], NoteField.transform);
                }
                else
                {
                    copy = Instantiate(PrefabObject[3], NoteField.transform);
                }
            }
            Note.noteObject.Add(copy);

            note.ms = noteSaved.NoteMs[i];
            note.pos = noteSaved.NotePos[i];
            note.line = noteSaved.NoteLine[i];
            switch (note.line)
            {
                case 1:
                    copy.transform.localPosition = new Vector3(-300, noteSaved.NotePos[i], 0);
                    break;

                case 2:
                    copy.transform.localPosition = new Vector3(-100, noteSaved.NotePos[i], 0);
                    break;

                case 3:
                    copy.transform.localPosition = new Vector3(+100, noteSaved.NotePos[i], 0);
                    break;

                case 4:
                    copy.transform.localPosition = new Vector3(+300, noteSaved.NotePos[i], 0);
                    break;

                case 5:
                    if (UserInfo.isBottomDisplay)
                    {
                        copy.transform.localPosition = new Vector3(-100, noteSaved.NotePos[i], 0);
                        copy.transform.localScale = new Vector3(0.75f, 2.75f, 1);
                    }
                    else
                    {
                        copy.transform.localPosition = new Vector3(0, noteSaved.NotePos[i], 0);
                        copy.transform.localScale = new Vector3(1, 2.75f, 1);
                    }
                    break;

                case 6:
                    if (UserInfo.isBottomDisplay)
                    {
                        copy.transform.localPosition = new Vector3(+100, noteSaved.NotePos[i], 0);
                        copy.transform.localScale = new Vector3(0.75f, 2.75f, 1);
                    }
                    else
                    {
                        copy.transform.localPosition = new Vector3(0, noteSaved.NotePos[i], 0);
                        copy.transform.localScale = new Vector3(1, 2.75f, 1);
                    }
                    break;
            }

            note.legnth = noteSaved.NoteLegnth[i];
            if (note.legnth != 0)
            {
                Vector3 scale;
                scale = copy.transform.localScale;
                scale.y = note.legnth;
                copy.transform.localScale = scale;
            }

            Note.listNote.Add(note);
        }
        for (int i = 0; i < noteSaved.SpeedMs.Count; i++)
        {
            Speed speed = new Speed();
            speed.ms = noteSaved.SpeedMs[i];
            speed.bpm = noteSaved.SpeedBpm[i];
            speed.pos = noteSaved.SpeedPos[i];
            speed.gameSpeed = speed.bpm * noteSaved.SpeedNum[i];
            Speed.speedNote.Add(speed);
        }

        yield return new WaitForSeconds(.5f);

        for (int i = 0; i < noteSaved.SpeedMs.Count; i++)
        {
            if (Speed.speedNote[i].gameSpeed <= 0)
            {
                float minusSpeedPos;
                minusSpeedPos = (noteSaved.SpeedPos[i + 1] - noteSaved.SpeedPos[i]) * 2;
                for (int j = 0; j < noteSaved.NoteMs.Count; j++)
                {
                    if (noteSaved.NoteMs[j] >= noteSaved.SpeedMs[i])
                    {
                        for (; j < noteSaved.NoteMs.Count; j++)
                        {
                            Vector3 notePos;
                            notePos = Note.noteObject[j].transform.localPosition;
                            notePos.y -= minusSpeedPos;
                            Note.noteObject[j].transform.localPosition = notePos;
                            Note.listNote[j].pos = notePos.y;
                        }
                        break;
                    }
                }
                for (int j = i + 1; j < Speed.speedNote.Count; j++)
                {
                    Speed.speedNote[j].pos -= minusSpeedPos;
                }
            }
        }

        yield return new WaitForSeconds(.5f);
        SpeedSetting();
        yield return new WaitForSeconds(.5f);

        isPlay = true;
        StartCoroutine(IPlayMusic());
    }

    public void ResetJudge()
    {
        Rush = new int[3] { 0, 0, 0 };
        Step = new int[2] { 0, 0 };
        Lost = new int[2] { 0, 0 };
    }

    private void ResetSavedData()
    {
        playMs = 0;

        GuidePos = new List<float>();

        noteSaved.bpm = new float();
        noteSaved.startDelayMs = new int();

        noteSaved.NoteLegnth = new List<int>();
        noteSaved.NoteMs = new List<float>();
        noteSaved.NoteLine = new List<int>();

        noteSaved.EffectMs = new List<float>();
        noteSaved.EffectForce = new List<float>();
        noteSaved.EffectDuration = new List<int>();

        noteSaved.SpeedMs = new List<float>();
        noteSaved.SpeedPos = new List<float>();
        noteSaved.SpeedBpm = new List<float>();
        noteSaved.SpeedNum = new List<float>();
    }

    public void SpeedSetting()
    {
        gameSpeed = MainSystem.gameSpeed / 100.0f;
        print(gameSpeed);
        print(MainSystem.gameSpeed);

        for (int i = 0; i < Note.listNote.Count; i++)
        {
            Note note;
            note = Note.listNote[i];

            Vector3 notePosSet;
            notePosSet = note[i].transform.localPosition;
            notePosSet.y = note.pos * gameSpeed;
            Note.noteObject[i].transform.localPosition = notePosSet;

            if (Note.listNote[i].legnth != 0)
            {
                Vector3 noteScale;
                noteScale = note[i].transform.localScale;
                noteScale.y = note.legnth * gameSpeed;
                note[i].transform.localScale = noteScale;
            }
        }
        /*
        for (int i = 0; i < GuideLine.Count; i++)
        {
            Vector3 guidePosSet;
            guidePosSet = GuideLine[i].transform.localPosition;
            guidePosSet.y = GuidePos[i] * gameSpeed;
            GuideLine[i].transform.localPosition = guidePosSet;
        }*/
    }

    /*private void GuideGenerate(float num)
    {
        for (int i = 0; i < PlayGuideParent.transform.childCount; i++)
        {
            Destroy(PlayGuideParent.transform.GetChild(0).gameObject);
        }

        GuideLine = new List<GameObject>();
        long count;
        // ms = 150 * 1600 / bpm
        count = Mathf.CeilToInt(num * bpm / 240000) + 2;

        for (int i = 0; i < count; i++)
        {
            GameObject copy;
            copy = Instantiate(PlayGuide, PlayGuideParent.transform);
            copy.transform.localPosition = new Vector3(0, 1600 * i, 0);
            copy.transform.GetChild(0).GetComponent<TextMeshPro>().text
                = string.Format("{0:D3}", i + 1);
            GuideLine.Add(copy);
        }
    }*/

    private void ResetNote()
    {
        foreach (GameObject note in Note.noteObject)
        {
            note.SetActive(true);
        }
    }

    IEnumerator IPlayMusic()
    {
        yield return new WaitForSeconds(noteSaved.startDelayMs / 1000);
        gameMusic.Play();
    }
}

public class Note
{
    public static List<Note> listNote = new List<Note>();
    public static List<GameObject> noteObject = new List<GameObject>();

    public int line;
    public int legnth;
    public float ms;
    public float pos;

    public GameObject this[int index]
    {
        get { return noteObject[index]; }
        set { noteObject[index] = value; }
    }
}

public class Speed
{
    public static int speedIndex;
    public static List<Speed> speedNote = new List<Speed>();

    public float ms;
    public float bpm;
    public float pos;
    public float gameSpeed;
}

[System.Serializable]
public class NoteSavedData
{
    public float bpm;
    public float startDelayMs;

    public List<int> NoteLegnth;
    public List<float> NoteMs;
    public List<float> NotePos;
    public List<int> NoteLine;

    public List<float> EffectMs;
    public List<float> EffectForce;
    public List<int> EffectDuration;

    public List<float> SpeedMs;
    public List<float> SpeedPos;
    public List<float> SpeedBpm;
    public List<float> SpeedNum;
}
