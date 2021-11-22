using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotefileLoad : MonoBehaviour
{
    public static NotefileLoad noteLoad;

    [SerializeField]
    NoteSavedData savedData = new NoteSavedData();

    public string Name;

    public List<GameObject> NoteLoadObject;

    [SerializeField]
    List<GameObject> NotePrefabs;

    [SerializeField]
    public GameObject NoteMoveField;

    [ContextMenu("Load")]
    void LoadDataFromJson()
    {
        string name;
        name = "NoteBox/" + MainSystem.mainSystem.songName + "/"
            + (MainSystem.mainSystem.difficultyNum + 1).ToString() + ".json";
        string path = Path.Combine(Application.dataPath, name);
        string jsonData = File.ReadAllText(path);
        savedData = JsonUtility.FromJson<NoteSavedData>(jsonData);
    }

    private void Awake()
    {
        if (noteLoad == null)
        {
            noteLoad = this;
        }
    }

    void Start()
    {
        StartCoroutine(LoadNote());
    }

    private IEnumerator LoadNote()
    {
        LoadDataFromJson();

        NoteMove.noteMove.startDelay = savedData.startDelayMs;

        MainJudgeManage.mainJudge.GameEndMS = MainSystem.mainSystem.gameEndMS;

        yield return new WaitForSeconds(1.5f);

        int count = savedData.NoteMs.Count;

        for (int i = 0; i < count; i++)
        {
            GameObject copy;

            if (savedData.NoteLine[i] != 5)
            {
                if (savedData.NoteLegnth[i] == 0)
                {
                    // NotePrefabs[0] == Chip
                    copy = Instantiate(NotePrefabs[0], NoteMoveField.transform);
                }
                else
                {
                    // NotePrefabs[1] == Long
                    copy = Instantiate(NotePrefabs[1], NoteMoveField.transform);
                }
            }
            else
            {
                if (savedData.NoteLegnth[i] == 0)
                {
                    // NotePrefabs[2] == Bt_Chip
                    copy = Instantiate(NotePrefabs[2], NoteMoveField.transform);
                }
                else
                {
                    // NotePrefabs[3] == Bt_Long
                    copy = Instantiate(NotePrefabs[3], NoteMoveField.transform);
                }
            }

            NoteSetting noteInfo;

            // public List<int> Note_type;
            // public List<int> Note_legnth;
            // public List<int> Note_ms;
            // public List<int> Note_line;

            // List<(GameObject note, int type, int legnth, int ms)>

            // type
            // 0 = normal
            // 1 = key_down
            // 2 = key_up
            // 3 = long
            // if (type != 3) legnth = 0;

            switch (savedData.NoteLine[i])
            {
                default:
                    //이펙트 혹은 조정노트 자리
                    break;

                case 1:
                    break;

                case 2:
                    break;

                case 3:
                    break;

                case 4:
                    break;

                case 5:
                    break;
            }
        }

        ScoreManage.scoreManage.NoteCount = NoteCount();
    }

    private IEnumerator SortingList()
    {
        yield return new WaitForSeconds(1.5f);
        NoteLoadObject.Sort(delegate (GameObject A, GameObject B)
        {
            if (A.transform.localPosition.y > B.transform.localPosition.y) return 1;
            if (A.transform.localPosition.y < B.transform.localPosition.y) return -1;
            return 0;
        });

        yield return new WaitForSeconds(1.5f);

        MainSystem.mainSystem.gameSpeed = MainSystem.mainSystem.bpm * MainSystem.mainSystem.highSpeed;

        foreach (GameObject Note in NoteLoadObject)
        {
            Vector3 pos;
            pos = Note.transform.localPosition;
            pos = new Vector3(pos.x, pos.y * Convert.ToSingle(MainSystem.mainSystem.highSpeed), pos.z);
            Note.transform.localPosition = pos;

            if (Note.tag == "Ls" || Note.tag == "Lm" || Note.tag == "Le")
            {
                Note.transform.localScale = new Vector3(1.0f, 1.0f * Convert.ToSingle(MainSystem.mainSystem.highSpeed), 1.0f);
            }
        }
        Debug.Log(NoteLoadObject[NoteLoadObject.Count - 1].transform.localPosition.y);
        MainJudgeManage.mainJudge.GameEndMS
        = (int)((NoteLoadObject[NoteLoadObject.Count - 1].transform.localPosition.y * 75 / MainSystem.mainSystem.gameSpeed) + 5000);
    }

    private void LongNoteLegnth(GameObject gameObject, int legnth)
    {
        // 16 Note Legnth = 166.66f
        // 01 Note Legnth = 16.666f
        // X Note Legnth = 16.666f * X
        Vector3 pos;
        pos = gameObject.transform.localScale;
        pos.y = 16.666f * legnth;
        gameObject.transform.localScale = pos;
    }

    private int NoteCount()
    {
        int count;
        count = 0;
        for (int i = 0; i < savedData.NoteMs.Count; i++)
        {
            if (savedData.NoteLegnth[i] == 0)
            {
                count++;
            }
            else
            {
                count += savedData.NoteLegnth[i];
            }
        }
        return count;
    }
}

[System.Serializable]
public class NoteSavedData
{
    public float bpm;
    public int startDelayMs;

    public List<int> NoteLegnth;
    public List<int> NoteMs;
    public List<int> NoteLine;

    public List<int> EffectMs;
    public List<float> EffectForce;
    public List<int> EffectDuration;

    public List<int> SpeedMs;
    public List<float> SpeedBpm;
}
