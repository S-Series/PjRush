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
    public bool[] isTesting = {false, false};
    public GameObject[] notePrefab;
    public Transform[] noteGenerateField;
    public JudgeSystem[] judgeSystems;
    public List<SpeedNote> speedNotes = new List<SpeedNote>();
    public List<EffectNote> effectNotes = new List<EffectNote>();
    [SerializeField] private GameObject DefaultMovingObject;
    private bool isEffect;
    private void Awake()
    {
        gamePlaySystem = this;
        s_GameMusic = GetComponent<AudioSource>();
    }
    private void Update()
    {
        if (!JudgeSystem.s_isTesting) return;
        if (!isEffect) { DefaultMovingObject.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f); }
        else { ; }
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
    public IEnumerator StartGame()
    {
        JudgeSystem.s_isTesting = true;
        yield return new WaitForSeconds(GameManager.s_delay / 1000.0f);
        s_GameMusic.Play();
    }
}
