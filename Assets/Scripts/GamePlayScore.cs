using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayScore : MonoBehaviour
{
    private int scoreNoteCount;
    private int scoreMaxNoteCount;
    public void AddScore(int type){
        int count = 0;
        switch(type){
            case 0:
                count = 60;
                break;

            case 1:
                count = 30;
                break;

            default: 
                return;
        }
        StartCoroutine(IAddScore(count));
    }
    private IEnumerator IAddScore(int count){
        for (int i = 0; i < count; i++){
            scoreNoteCount ++;
            yield return null;
            int calScore = (int)(100000000d * (Convert.ToDouble(scoreNoteCount) / scoreMaxNoteCount));
            GameInfoField.score = calScore;
            GameManager.GamePlayScore = calScore;
        }
    }
    public void setNoteScore(int noteCount){
        print(noteCount);
        GameInfoField.score = 0;
        scoreNoteCount = 0;
        scoreMaxNoteCount = noteCount * 60;
    }
}
