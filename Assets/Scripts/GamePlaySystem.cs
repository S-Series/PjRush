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
        JudgeSystem.s_longDelay = Mathf.RoundToInt(15.0f / GameManager.s_bpm);
        gameBpm = GameManager.s_bpm;
        SpeedMs = 0;
        EffectMs = 0;
        SpeedPos = 0;
        EffectPos = 0;
        isEffect = false;
        SpeedMultiply = (GameManager.s_Multiply / 100.0f);
        yield return new WaitForSeconds(1.0f);
        JudgeSystem.s_isTesting = true;
        foreach (JudgeSystem judge in judgeSystems) { judge.ActivateTest(); }
        yield return new WaitForSeconds(GameManager.s_delay / 1000.0f);
        s_GameMusic.Play();
    }
}
