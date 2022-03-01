using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public JudgeSystem[] judgeSystems = new JudgeSystem[5];

    GamePlaySystem gamePlaySystem;

    public int[] Rush = new int[3] { 0, 0, 0 };
    public int[] Step = new int[2] { 0, 0 };
    public int[] Lost = new int[2] { 0, 0 };

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void ResetJudge()
    {
        Rush = new int[3] { 0, 0, 0 };
        Step = new int[2] { 0, 0 };
        Lost = new int[2] { 0, 0 };
    }
}
