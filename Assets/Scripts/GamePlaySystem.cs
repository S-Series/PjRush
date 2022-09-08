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
    public bool[] isTesting = {false, false};
    public GameObject[] notePrefab;
    public Transform[] noteGenerateField;
    public JudgeSystem[] judgeSystems;
    public List<SpeedNote> speedNotes = new List<SpeedNote>();
    public List<EffectNote> effectNotes = new List<EffectNote>();
    [SerializeField] private GameObject DefaultMovingObject;
    [SerializeField] private Animator GameEndAnimator;
    private readonly string[] GameEndAnimatorTriggers 
        = {"SPerfect", "Perfect", "Maximum", "Clear", "Fail"};
    private float gameBpm;
    private bool isEffect;
    private int SpeedMs;
    private float SpeedPos;
    private int EffectMs;
    private float EffectPos;
    private float SpeedMultiply = 1.0f;
    private Vector3 MovingPos = new Vector3(0.0f, 0.0f, 0.0f);
    private void Awake()
    {
        gamePlaySystem = this;
        s_GameMusic = GetComponent<AudioSource>();
    }
    private void FixedUpdate()
    {
        if (!JudgeSystem.s_isTesting) return;
        s_gameMs++;
    }
    private void Update()
    {
        if (!JudgeSystem.s_isTesting) return;
        if (isEffect) { MovingPos.y = EffectPos; }
        else { MovingPos.y = EffectPos + SpeedPos + (((s_gameMs * SpeedMultiply) - SpeedMs - EffectMs) * gameBpm) / 15000; }
        DefaultMovingObject.transform.localPosition = - MovingPos;
    }
    public void ClearNoteField()
    {
        speedNotes = new List<SpeedNote>();
        effectNotes = new List<EffectNote>();
        foreach (Transform noteTransform in noteGenerateField)
        {
            for (int i = 0; i < noteTransform.transform.childCount; i++)
            {
                Destroy(noteTransform.GetChild(0).gameObject);
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
        PlayerInputManager.SetKeyOption();
        judgeSystems[0].ChangeKey(PlayerInputManager.s_Line1);
        judgeSystems[1].ChangeKey(PlayerInputManager.s_Line2);
        judgeSystems[2].ChangeKey(PlayerInputManager.s_Line3);
        judgeSystems[3].ChangeKey(PlayerInputManager.s_Line4);
        judgeSystems[4].ChangeKey(PlayerInputManager.s_Line5);
        judgeSystems[5].ChangeKey(PlayerInputManager.s_Line6);
        GameInfoField.s_score = 0;
        GameInfoField.gameInfoField.InfoSetting();
        s_GameMusic.time = 0;
        yield return new WaitForSeconds(1.0f);
        JudgeSystem.s_isTesting = true;
        foreach (JudgeSystem judge in judgeSystems) { judge.ActivateTest(); }
        yield return new WaitForSeconds(GameManager.s_delay / 1000.0f);
        print(GameManager.s_delay);
        s_GameMusic.Play();
    }
    public static void CheckGameEnd()
    {
        if (gamePlaySystem.judgeSystems[0].isTestAlive) { return; }
        if (gamePlaySystem.judgeSystems[1].isTestAlive) { return; }
        if (gamePlaySystem.judgeSystems[2].isTestAlive) { return; }
        if (gamePlaySystem.judgeSystems[3].isTestAlive) { return; }
        if (gamePlaySystem.judgeSystems[4].isTestAlive) { return; }
        if (gamePlaySystem.judgeSystems[5].isTestAlive) { return; }
        GamePlaySystem.gamePlaySystem.StartCoroutine(GamePlaySystem.gamePlaySystem.IEndGame());
    }
    private IEnumerator IEndGame()
    {
        /*if (GameManager.s_isDetailPerfect) { GameEndAnimator.SetTrigger(GameEndAnimatorTriggers[0]); }
        else if (GameManager.s_isDetailPerfect) { GameEndAnimator.SetTrigger(GameEndAnimatorTriggers[1]); }
        else if (GameManager.s_isDetailPerfect) { GameEndAnimator.SetTrigger(GameEndAnimatorTriggers[2]); }*/
        yield return new WaitForSeconds(5.0f);
        /*if (GameManager.s_isComplete) { GameEndAnimator.SetTrigger(GameEndAnimatorTriggers[3]); }
        else { GameEndAnimator.SetTrigger(GameEndAnimatorTriggers[4]); }*/
        JudgeSystem.s_isTesting = false;
        GameManager.s_isDetailPerfect = ComboSystem.s_isSemiPerfect;
        GameManager.s_isPerfect = ComboSystem.s_isPerfect;
        GameManager.s_isMaximum = ComboSystem.s_isMaximum;
        GameManager.s_MaxCombo = ComboSystem.s_playMaxCombo;
        yield return new WaitForSeconds(5.0f);
        MainSystem.LoadResultScene();
    }
}
