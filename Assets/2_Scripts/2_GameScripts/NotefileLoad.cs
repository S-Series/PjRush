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

        MainJudgeManage.mainJudge.GameEndMS = savedData.endMs;

        yield return new WaitForSeconds(1.5f);

        int count = savedData.Note_ms.Count;

        for (int i = 0; i < count; i++)
        {
            GameObject copy;

            if (savedData.Note_line[i] != 5)
            {
                if (savedData.Note_legnth[i] == 0)
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
                if (savedData.Note_legnth[i] == 0)
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

            switch (savedData.Note_line[i])
            {
                default:
                    //이펙트 혹은 조정노트 자리
                    break;

                case 1:
                    copy.transform.localPosition = new Vector3(-300,
                        Convert.ToSingle(savedData.Note_ms[i] * savedData.bpm * MainSystem.mainSystem.highSpeed / 150), 0);
                    Judge1.judge1.Note1.Add
                        ((copy, savedData.Note_legnth[i], savedData.Note_ms[i]));
                    break;

                case 2:
                    copy.transform.localPosition = new Vector3(-100,
                        Convert.ToSingle(savedData.Note_ms[i] * savedData.bpm * MainSystem.mainSystem.highSpeed / 150), 0);
                    Judge2.judge2.Note2.Add
                        ((copy, savedData.Note_legnth[i], savedData.Note_ms[i]));
                    break;

                case 3:
                    copy.transform.localPosition = new Vector3(+100,
                        Convert.ToSingle(savedData.Note_ms[i] * savedData.bpm * MainSystem.mainSystem.highSpeed / 150), 0);
                    Judge3.judge3.Note3.Add
                        ((copy, savedData.Note_legnth[i], savedData.Note_ms[i]));
                    break;

                case 4:
                    copy.transform.localPosition = new Vector3(+300,
                        Convert.ToSingle(savedData.Note_ms[i] * savedData.bpm * MainSystem.mainSystem.highSpeed / 150), 0);
                    Judge4.judge4.Note4.Add
                        ((copy, savedData.Note_legnth[i], savedData.Note_ms[i]));
                    break;

                case 5:
                    copy.transform.localPosition = new Vector3(0,
                         Convert.ToSingle(savedData.Note_ms[i] * savedData.bpm * MainSystem.mainSystem.highSpeed / 150), 0);
                    JudgeBottom.judge_bt.NoteBt.Add
                        ((copy, savedData.Note_legnth[i], savedData.Note_ms[i]));
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
        for (int i = 0; i < savedData.Note_ms.Count; i++)
        {
            if (savedData.Note_legnth[i] == 0)
            {
                count++;
            }
            else
            {
                count += savedData.Note_legnth[i];
            }
        }
        return count;
    }
}

[System.Serializable]
public class NoteSavedData
{
    public float bpm;
    public int endMs;

    public List<int> Note_legnth;
    public List<int> Note_ms;
    public List<int> Note_line;
}
