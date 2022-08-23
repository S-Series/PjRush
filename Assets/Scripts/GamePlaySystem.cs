using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GamePlaySystem : MonoBehaviour
{
    public static GamePlaySystem gamePlaySystem;
    public GameObject[] notePrefab;
    public Transform[] noteGenerateField;
    [SerializeField] private JudgeSystem[] judgeSystems;
    private void Awake()
    {
        gamePlaySystem = this;
    }
    public void ClearNoteField()
    {
        foreach (Transform noteTransform in noteGenerateField)
        {
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                Destroy(noteTransform.GetChild(0).gameObject);
            }
        }
    }
}
