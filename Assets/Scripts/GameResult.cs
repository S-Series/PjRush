using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameResult : MonoBehaviour
{

    private int getScoreIndex(int score)
    {
        if (score >= 99500000) { return 00; }               // S+
        else if (score >= 99000000) { return 01; }          // S
        else if (score >= 97500000) { return 02; }          // AA+
        else if (score >= 95000000) { return 03; }          // AA
        else if (score >= 92500000) { return 04; }          // A+
        else if (score >= 90000000) { return 05; }          // A
        else if (score >= 85000000) { return 06; }          // B
        else if (score >= 80000000) { return 07; }          // C
        else { return 08; }                                 // D
    }
}
