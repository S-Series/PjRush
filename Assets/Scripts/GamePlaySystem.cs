using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GamePlaySystem : MonoBehaviour
{
    public static GamePlaySystem gamePlay;
    NoteSavedData noteSaved = new NoteSavedData();

    [SerializeField] JudgeSystem[] judgeSystem;

    public static bool isPlay;
    public static float testBpm;
    public static float gameSpeed;
    public GameInfoField gameInfo;
    public GamePlayScore gameScore;

    private List<float> GuidePos;

    [SerializeField]
    public static int playMs;
    public static int ComboCount;

    private float TestSpeedPos;
    private float TestSpeedMs;
    private float endMs;

    [SerializeField]
    GameObject NoteField;
    GameObject MovingNoteField;

    [SerializeField]
    GameObject[] PrefabObject;

    [SerializeField]
    TextMeshPro[] ComboText;

    [SerializeField]
    Animator ComboAnimate;
    [SerializeField]
    TextMeshPro[] ComboAnimationText;
    private int ComboAnimateCount;

    [SerializeField]
    TextMeshPro ScoreText;

    [SerializeField]
    Animator[] JudgeAnimate;
    private protected readonly Color32[] TextScoreColor = 
        { 
        // White    Index(0|1)
        new Color32(235, 245, 255, 255),
        new Color32(235, 245, 255, 100),
        // Green    Index(2|3)
        new Color32(100, 255, 175, 255),
        new Color32(100, 255, 175, 100),
        // Yellow   Index(4|5)
        new Color32(255, 255, 150, 255),
        new Color32(255, 255, 150, 100)
    };
    AudioSource gameMusic;
    private void Start(){

        gamePlay = this;
        MovingNoteField = NoteField.transform.parent.gameObject;

        gameMusic = GetComponent<AudioSource>();
        gameScore = GetComponent<GamePlayScore>();
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

            if (playMs >= endMs)
            {
                StartCoroutine(IGameEnd());
                isPlay = false;
            }
        }
    }

    [ContextMenu("Load")]
    public IEnumerator ILoadDataFromJson(){
        ResetGame();
        while(true){
            try{
                foreach (JudgeSystem judge in judgeSystem){
                    judge.gamePlaySystem = GetComponent<GamePlaySystem>();
                }
                break;
            }
            catch{}
            yield return null;
        }
        gameMusic.clip = MainSystem.NowOnMusic.audMusicFile;

        for (int i = 0; i < 5; i++){
            judgeSystem[i].Setkey
                (KeySetting.keys[(KeyActions)i], KeySetting.keys[(KeyActions)i + 5]);
        }

        ResetSavedData();
        try{
            string path = Path.Combine(Application.dataPath, "NoteBox/" + MainSystem.NowOnMusic.MusicName
                + "/" + (MainSystem.mainSystem.difficulty + 1).ToString() + ".json");
            Debug.Log(path);
            string jsonData = File.ReadAllText(path);
            print(path);
            noteSaved = JsonUtility.FromJson<NoteSavedData>(jsonData);
        }
        catch{
            Debug.LogError("NoteFile Note Founded");
            yield break;
        }

        yield return new WaitForSeconds(.5f);

        for (int i = 0; i < Note.noteObjectList.Count; i++){
            Destroy(Note.noteObjectList[i]);
        }

        testBpm = noteSaved.bpm;
        Speed.speedIndex = 0;
        Note.listNote = new List<Note>();
        Note.noteObjectList = new List<GameObject>();
        Speed.speedNote = new List<Speed>();

        for (int i = 0; i < noteSaved.NoteMs.Count; i++){
            Note note = new Note();
            JudgeSystem trgetJudgeSystem;

            GameObject copy;
            if (noteSaved.NoteLegnth[i] == 0){
                if (noteSaved.NoteLine[i] <= 4){
                    copy = Instantiate(PrefabObject[0], NoteField.transform);
                }
                else{
                    copy = Instantiate(PrefabObject[2], NoteField.transform);
                }
            }
            else{
                if (noteSaved.NoteLine[i] <= 4){
                    copy = Instantiate(PrefabObject[1], NoteField.transform);
                }
                else{
                    copy = Instantiate(PrefabObject[3], NoteField.transform);
                }
            }
            Note.noteObjectList.Add(copy);
            note.noteObject = copy;

            note.ms = noteSaved.NoteMs[i];
            note.pos = noteSaved.NotePos[i];
            note.line = noteSaved.NoteLine[i];
            switch (note.line)
            {
                case 1:
                    trgetJudgeSystem = judgeSystem[0];
                    copy.transform.localPosition = new Vector3(-300, noteSaved.NotePos[i], 0);
                    break;

                case 2:
                    trgetJudgeSystem = judgeSystem[1];
                    copy.transform.localPosition = new Vector3(-100, noteSaved.NotePos[i], 0);
                    break;

                case 3:
                    trgetJudgeSystem = judgeSystem[2];
                    copy.transform.localPosition = new Vector3(+100, noteSaved.NotePos[i], 0);
                    break;

                case 4:
                    trgetJudgeSystem = judgeSystem[3];
                    copy.transform.localPosition = new Vector3(+300, noteSaved.NotePos[i], 0);
                    break;

                case 5:
                    trgetJudgeSystem = judgeSystem[4];
                    if (UserSetting.isBottomDisplay)
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
                    trgetJudgeSystem = judgeSystem[4];
                    if (UserSetting.isBottomDisplay){
                        copy.transform.localPosition = new Vector3(+100, noteSaved.NotePos[i], 0);
                        copy.transform.localScale = new Vector3(0.75f, 2.75f, 1);
                    }
                    else{
                        copy.transform.localPosition = new Vector3(0, noteSaved.NotePos[i], 0);
                        copy.transform.localScale = new Vector3(1, 2.75f, 1);
                    }
                    break;

                default:
                    Debug.LogError("NoteFlie has UnAvailable Line Index");
                    yield break;
            }

            note.legnth = noteSaved.NoteLegnth[i];
            if (note.legnth != 0){
                Vector3 scale;
                scale = copy.transform.localScale;
                scale.y = note.legnth;
                copy.transform.localScale = scale;
            }

            Note.listNote.Add(note);
            trgetJudgeSystem.gameNote.Add(note);
        }
        for (int i = 0; i < noteSaved.SpeedMs.Count; i++){
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
                            notePos = Note.noteObjectList[j].transform.localPosition;
                            notePos.y -= minusSpeedPos;
                            Note.noteObjectList[j].transform.localPosition = notePos;
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
        int noteCount = 0;
        for (int i = 0; i < noteSaved.NoteLegnth.Count; i++){
            if (noteSaved.NoteLegnth[i] == 0) { noteCount++; }
            else { 
                noteCount += noteSaved.NoteLegnth[i];
            }
        }

        yield return new WaitForSeconds(.5f);
        SpeedSetting();
        gameScore.setNoteScore(noteCount);
        yield return new WaitForSeconds(.5f);
        gameInfo.InfoSetting();
        MainSystem.mainSystem.GameAnimator[0].SetTrigger("GameStart");
        endMs = Note.listNote[Note.listNote.Count - 1].ms + 200;
        yield return new WaitForSeconds(5.0f);
        gameInfo.GameInfoSettingAnimator.SetTrigger("Play");
        yield return new WaitForSeconds(2.0f);
        isPlay = true;
        JudgeSystem.isOnPlay = true;
        StartCoroutine(IPlayMusic());
    }
    private IEnumerator IGameEnd()
    {
        if (GameManager.isAllPerfect) {

        }
        else if (GameManager.isFullCombo) {
            
        }
        
        yield return new WaitForSeconds(4.0f);

        MainSystem.mainSystem.GameAnimator[1].SetTrigger("ChangeIn");
        MainSystem.mainSystem.RunIResultStart();

        yield return new WaitForSeconds(4.0f);

        gamePlay = null;
        SceneManager.LoadScene("Result");
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
    public void SpeedSetting(){
        gameSpeed = MainSystem.gameSpeed / 100.0f;
        print(gameSpeed);
        print(MainSystem.gameSpeed);

        for (int i = 0; i < Note.listNote.Count; i++){
            Note note;
            note = Note.listNote[i];

            Vector3 notePosSet;
            notePosSet = note[i].transform.localPosition;
            notePosSet.y = note.pos * gameSpeed;
            Note.noteObjectList[i].transform.localPosition = notePosSet;

            if (Note.listNote[i].legnth != 0){
                Vector3 noteScale;
                noteScale = note[i].transform.localScale;
                noteScale.y = note.legnth * gameSpeed;
                note[i].transform.localScale = noteScale;
            }
        }
        /*
        for (int i = 0; i < GuideLine.Count; i++){
            Vector3 guidePosSet;
            guidePosSet = GuideLine[i].transform.localPosition;
            guidePosSet.y = GuidePos[i] * gameSpeed;
            GuideLine[i].transform.localPosition = guidePosSet;
        }*/
        PlayerPrefs.SetFloat("speed", gameSpeed);
    }

    /*private void GuideGenerate(float num){
        for (int i = 0; i < PlayGuideParent.transform.childCount; i++)
        {
            Destroy(PlayGuideParent.transform.GetChild(0).gameObject);
        }

        GuideLine = new List<GameObject>();
        long count;
        // ms = 150 * 1600 / bpm
        count = Mathf.CeilToInt(num * bpm / 240000) + 2;

        for (int i = 0; i < count; i++){
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
        foreach (GameObject note in Note.noteObjectList)
        {
            note.SetActive(true);
        }
    }
    IEnumerator IPlayMusic()
    {
        yield return new WaitForSeconds(noteSaved.startDelayMs / 1000);
        gameMusic.Play();
    }
    private void AddCombo()
    {
        ComboCount++;
        string combo;
        if (ComboCount > 9999) { combo = "MAX+"; }
        else { combo = string.Format("{0:D4}", ComboCount); }
        for (int i = 0; i < 4; i++)
        {
            ComboText[i].text = combo[i].ToString();
        }

        if (ComboCount >= (ComboAnimateCount + 1) * 100)
        {
            if (ComboCount >= 1000)
            {
                ComboAnimationText[0].gameObject.SetActive(true);
                ComboAnimationText[0].text = ComboCount.ToString().Substring(0, 1);
                ComboAnimationText[1].text = ComboCount.ToString().Substring(1, 1);
            }
            else
            {
                ComboAnimationText[0].gameObject.SetActive(false);
                ComboAnimationText[1].text = ComboCount.ToString().Substring(0, 1);
            }
            ComboAnimateCount++;
            ComboAnimate.SetTrigger("Play");
        }
    }
    private void ResetCombo()
    {
        GameManager.isFullCombo = false;
        GameManager.isAllPerfect = false;
        if (ComboCount == 0) return;
        ComboCount = 0;
        ComboAnimateCount = 0;
        foreach (TextMeshPro text in ComboText)
        {
            text.color = TextScoreColor[1];
            text.text = "0";
        }
    }
    private void ColorSetting()
    {
        Color32[] color = new Color32[2];
        int settingColorRangeIndex;
        if (GameManager.isAllPerfect)
        {
            color[0] = TextScoreColor[4];
            color[1] = TextScoreColor[5];
        }
        else if (GameManager.isFullCombo)
        {
            color[0] = TextScoreColor[2];
            color[1] = TextScoreColor[3];
        }
        else
        {
            color[0] = TextScoreColor[0];
            color[1] = TextScoreColor[1];
        }

        if (ComboCount == 0) { settingColorRangeIndex = 0; }
        else if (ComboCount >= 1000) { settingColorRangeIndex = 4; }
        else if (ComboCount >= 100) { settingColorRangeIndex = 3; }
        else if (ComboCount >= 10) { settingColorRangeIndex = 2; }
        else { settingColorRangeIndex = 1; }

        for (int i = 0; i < settingColorRangeIndex; i++)
        {
            ComboText[3 - i].color = color[0];
        }

        for (int i = settingColorRangeIndex; i < 4; i++)
        {
            ComboText[3 - i].color = color[1];
        }
    }
    public void JudgeApply(int JudgeCase, int Line){
        int judgeLine = Line - 1;
        switch (JudgeCase)
        {
            case 0:
                AddCombo();
                ColorSetting();
                gameScore.AddScore(0);
                GameManager.Record[1]++;
                JudgeAnimate[judgeLine].SetTrigger("Perfect");
                break;

            case 1:
                AddCombo();
                ColorSetting();
                gameScore.AddScore(0);
                GameManager.Record[0]++;
                JudgeAnimate[judgeLine].SetTrigger("Perfect");
                break;

            case -1:
                AddCombo();
                ColorSetting();
                gameScore.AddScore(0);
                GameManager.Record[2]++;
                JudgeAnimate[judgeLine].SetTrigger("Perfect");
                break;

            case 2:
                AddCombo();
                ColorSetting();
                gameScore.AddScore(1);
                GameManager.Rough[0]++;
                JudgeAnimate[judgeLine].SetTrigger("Near");
                if (GameManager.isAllPerfect){
                    foreach(TextMeshPro text in ComboAnimationText)
                    {
                        text.color = TextScoreColor[2];
                    }
                }
                GameManager.isAllPerfect = false;
                break;

            case -2:
                AddCombo();
                ColorSetting();
                gameScore.AddScore(1);
                GameManager.Rough[1]++;
                JudgeAnimate[judgeLine].SetTrigger("Near");
                if (GameManager.isAllPerfect)
                {
                    foreach (TextMeshPro text in ComboAnimationText)
                    {
                        text.color = TextScoreColor[2];
                    }
                }
                GameManager.isAllPerfect = false;
                break;

            case 3:
                ResetCombo();
                GameManager.Lost[0]++;
                JudgeAnimate[judgeLine].SetTrigger("Lost");
                if (GameManager.isAllPerfect || GameManager.isFullCombo)
                {
                    foreach (TextMeshPro text in ComboAnimationText)
                    {
                        text.color = TextScoreColor[0];
                    }
                }
                GameManager.isFullCombo = false;
                GameManager.isAllPerfect = false;
                break;

            case -3:
                ResetCombo();
                GameManager.Lost[1]++;
                JudgeAnimate[judgeLine].SetTrigger("Lost");
                if (GameManager.isAllPerfect || GameManager.isFullCombo)
                {
                    foreach (TextMeshPro text in ComboAnimationText)
                    {
                        text.color = TextScoreColor[0];
                    }
                }
                GameManager.isFullCombo = false;
                GameManager.isAllPerfect = false;
                break;
        }
    }
    public void ResetGame(){
        ResetCombo();
        GameManager.isFullCombo = true;
        GameManager.isAllPerfect = true;
        foreach (TextMeshPro text in ComboText)
        {
            text.color = TextScoreColor[5];
            text.text = "0";
        }
        foreach (TextMeshPro text in ComboAnimationText)
        {
            text.color = TextScoreColor[4];
        }
        GameManager.ResetJudge();
    }
}

public class Note
{
    public static List<Note> listNote = new List<Note>();
    public static List<GameObject> noteObjectList = new List<GameObject>();

    public int line;
    public int legnth;
    public float ms;
    public float pos;
    public GameObject noteObject;

    public GameObject this[int index]
    {
        get { return noteObjectList[index]; }
        set { noteObjectList[index] = value; }
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
