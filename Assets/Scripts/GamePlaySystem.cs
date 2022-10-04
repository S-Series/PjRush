using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GamePlaySystem : MonoBehaviour
{
    public static GamePlaySystem gamePlaySystem;
    public static AudioSource s_GameMusic;
    public static int s_gameMs;
    public static float gameBpm;
    public bool[] isTesting = {false, false};
    public GameObject[] notePrefab;
    public Transform[] noteGenerateField;
    public JudgeSystem[] judgeSystems;
    public AudioSource[] judgeSound;
    public Sprite[] LongSprite;
    public Sprite[] InputEffectSprite;
    public Sprite[] JudgeTextSprite;
    public List<SpeedNote> speedNotes = new List<SpeedNote>();
    public List<EffectNote> effectNotes = new List<EffectNote>();
    [SerializeField] private GameObject DefaultMovingObject;
    [SerializeField] private Animator GameEndAnimator;
    private readonly string[] GameEndAnimatorTriggers 
        = {"Perfect", "Maximum", "Clear", "Fail"};

    //* SpeedNote
    private SpeedNote nowSpeedNote;
    private bool isSpeedTesting;
    private int SpeedIndex; 
    private int SpeedMs;
    private float SpeedPos;

    //* EffectNote
    private EffectNote nowEffectNote;
    private bool isEffectTesting;
    private bool isEffect;
    private int EffectIndex;
    private int EffectMs;
    private float EffectPos;
    private float SpeedMultiply = 1.0f;

    //* Others
    private Vector3 MovingPos = new Vector3(0.0f, 0.0f, 0.0f);
    private IEnumerator restartCoroutine;

    private void Awake()
    {
        gamePlaySystem = this;
        s_GameMusic = GetComponent<AudioSource>();
        restartCoroutine = IQuitGame();
    }
    private void FixedUpdate()
    {
        if (!JudgeSystem.s_isTesting) return;
        s_gameMs++;
    }
    private void Update()
    {
        if (!JudgeSystem.s_isTesting) return;

        if (isSpeedTesting)
        {
            if (nowSpeedNote.ms <= s_gameMs)
            {
                SpeedMs = (int)nowSpeedNote.ms;
                SpeedPos = nowSpeedNote.pos;
                gameBpm = nowSpeedNote.bpm * nowSpeedNote.multiply;
                SpeedIndex++;
                JudgeSystem.s_longDelay = 15.0f / gameBpm;
                if (speedNotes.Count == SpeedIndex) { isSpeedTesting = false; }
                else { nowSpeedNote = speedNotes[SpeedIndex]; }
            }
        }

        if (isEffectTesting)
        {
            if (nowEffectNote.ms <= s_gameMs)
            {
                EffectIndex++;
                if (effectNotes.Count == EffectIndex) { isEffectTesting = false; }
                else { nowEffectNote = effectNotes[EffectIndex]; }
            }
        }

        if (isEffect) { MovingPos.y = EffectPos; }
        else { MovingPos.y = ((EffectPos + SpeedPos))
            + (((s_gameMs  - SpeedMs - EffectMs) * gameBpm) / 15000); }
        DefaultMovingObject.transform.localPosition = - MovingPos * SpeedMultiply;

        if (Input.GetKeyDown(KeyCode.F5))
        {
            StartCoroutine(IRestartGame());
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StopCoroutine(restartCoroutine);
            restartCoroutine = IQuitGame();
            StartCoroutine(restartCoroutine);
        }
        if (Input.GetKeyUp(KeyCode.Escape)) { StopCoroutine(restartCoroutine); }
    }
    public void ClearNoteField()
    {
        speedNotes = new List<SpeedNote>();
        effectNotes = new List<EffectNote>();
        foreach (Transform noteTransform in noteGenerateField)
        {
            for (int i = 0; i < noteTransform.transform.childCount; i++)
            {
                Destroy(noteTransform.GetChild(i).gameObject);
            }
        }
        foreach (JudgeSystem judge in judgeSystems)
        {
            judge.notes = new List<NormalNote>();
        }
    }
    public IEnumerator IStartGame()
    {
        GameManager.ResetGameData();
        JudgeSystem.s_bpm = GameManager.s_bpm;
        JudgeSystem.s_longDelay = 15.0f / GameManager.s_bpm;
        ComboSystem.ResetComboSystem();
        gameBpm = GameManager.s_bpm;
        SpeedMs = 0;
        EffectMs = 0;
        SpeedPos = 0;
        EffectPos = 0;
        MovingPos.y = 0;
        isEffect = false;
        s_gameMs = 0;
        SpeedMultiply = (GameManager.s_Multiply / 100.0f);

        speedNotes = NoteData.s_speedNotes;
        effectNotes = NoteData.s_effectNotes;

        if (speedNotes.Count == 0) { isSpeedTesting = false; }
        else { nowSpeedNote = speedNotes[0]; isSpeedTesting = true; }

        if (effectNotes.Count == 0) { isEffectTesting = false; }
        else { nowEffectNote = effectNotes[0]; isEffectTesting = true; }


        #region keySetting
        PlayerInputManager.SetKeyOption();
        judgeSystems[0].ChangeKey(PlayerInputManager.s_Line1);
        judgeSystems[1].ChangeKey(PlayerInputManager.s_Line2);
        judgeSystems[2].ChangeKey(PlayerInputManager.s_Line3);
        judgeSystems[3].ChangeKey(PlayerInputManager.s_Line4);
        judgeSystems[4].ChangeKey(PlayerInputManager.s_Line5);
        judgeSystems[5].ChangeKey(PlayerInputManager.s_Line6);
        #endregion

        GameInfoField.s_score = 0;
        GameInfoField.s_noteJudgeCount = 0;
        s_GameMusic.time = 0;
        yield return new WaitForSeconds(2.0f);
        GameInfoField.gameInfoField.InfoSetting();
        yield return new WaitForSeconds(2.0f);
        JudgeSystem.s_isTesting = true;
        foreach (JudgeSystem judge in judgeSystems) { judge.ActivateTest(); }
        yield return new WaitForSeconds(GameManager.s_delay / 1000.0f);
        s_GameMusic.Play();
    }
    public static void GameEnd()
    {
        gamePlaySystem.StartCoroutine(gamePlaySystem.IEndGame());
    }
    private IEnumerator IEndGame()
    {
        print("GameEnd");
        foreach(JudgeSystem judge in judgeSystems) { judge.EndGame(); }
        if (ComboSystem.s_isSemiPerfect || ComboSystem.s_isPerfect)
            { GameEndAnimator.SetTrigger(GameEndAnimatorTriggers[0]); }
        else if (ComboSystem.s_isMaximum) { GameEndAnimator.SetTrigger(GameEndAnimatorTriggers[1]); }
        yield return new WaitForSeconds(3.0f);
        /*if (GameManager.s_isComplete) { GameEndAnimator.SetTrigger(GameEndAnimatorTriggers[3]); }
        else { GameEndAnimator.SetTrigger(GameEndAnimatorTriggers[4]); }*/
        JudgeSystem.s_isTesting = false;
        GameManager.s_isDetailPerfect = ComboSystem.s_isSemiPerfect;
        GameManager.s_isPerfect = ComboSystem.s_isPerfect;
        GameManager.s_isMaximum = ComboSystem.s_isMaximum;
        GameManager.s_MaxCombo = ComboSystem.s_playMaxCombo;
        yield return new WaitForSeconds(3.0f);
        MainSystem.LoadResultScene();
    }
    private IEnumerator IRestartGame()
    {
        AnimatorManager.PlayAnimation(3, true);
        GameInfoField.s_score = 0;
        GameInfoField.s_noteJudgeCount = 0;
        JudgeSystem.s_isTesting = false;
        JudgeSystem.s_bpm = GameManager.s_bpm;
        JudgeSystem.s_longDelay = 15.0f / GameManager.s_bpm;
        ComboSystem.ResetComboSystem();
        gameBpm = GameManager.s_bpm;
        SpeedMs = 0;
        EffectMs = 0;
        SpeedPos = 0;
        EffectPos = 0;
        MovingPos.y = 0;
        isEffect = false;
        s_gameMs = 0;
        SpeedMultiply = (GameManager.s_Multiply / 100.0f);
        DefaultMovingObject.transform.localPosition = new Vector3(0, 0, 0);

        foreach (JudgeSystem _judgeSystem in judgeSystems)
        {
            JudgeSystem.s_isTesting = false;
            _judgeSystem.noteIndex = 0;
            _judgeSystem.Restart();
            _judgeSystem.StopAllCoroutines();
            s_GameMusic.Stop();
            foreach (NormalNote _note in _judgeSystem.notes)
            {
                _note.noteObject.GetComponent<SpriteRenderer>().enabled = true;
                _note.noteObject.transform.GetChild(0)
                    .GetComponent<SpriteRenderer>().enabled = true;
            }
        }

        yield return new WaitForSeconds(1.0f);
        AnimatorManager.PlayAnimation(3, false);
        yield return new WaitForSeconds(1.0f);
        
        JudgeSystem.s_isTesting = true;
        yield return new WaitForSeconds(GameManager.s_delay / 1000.0f);
        s_GameMusic.Play();
    }
    private IEnumerator IQuitGame()
    {
        yield return new WaitForSeconds(3.0f);
        JudgeSystem.s_isTesting = false;
        GameManager.s_isDetailPerfect = ComboSystem.s_isSemiPerfect;
        GameManager.s_isPerfect = ComboSystem.s_isPerfect;
        GameManager.s_isMaximum = ComboSystem.s_isMaximum;
        GameManager.s_MaxCombo = ComboSystem.s_playMaxCombo;
        MainSystem.LoadResultScene();
    }
}
