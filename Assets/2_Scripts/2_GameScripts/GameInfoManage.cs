using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameInfoManage : MonoBehaviour
{
    public static GameInfoManage gameInfo;

    [SerializeField]
    GameObject MusicBoxObject;

    [SerializeField]
    GameObject InfoObject;

    [SerializeField]
    AudioSource GameMusic;

    [SerializeField]
    SpriteRenderer GameJacket;

    [SerializeField]
    SpriteRenderer GameJacketShadow;

    [SerializeField]
    SpriteRenderer DifficultyBox;

    [SerializeField]
    TextMeshPro LevelNumText;

    [SerializeField]
    TextMeshPro[] SongInfo;

    [SerializeField]
    Sprite[] difficultyBoxSprite;

    [SerializeField]
    public ParticleSystem particle;

    private void Awake()
    {
        gameInfo = this;
    }

    private void Start()
    {
        StartCoroutine(getInfo());
    }

    private void Update()
    {
        
    }

    IEnumerator getInfo()
    {
        yield return new WaitForSeconds(1.0f);
        InfoObject = MusicBox.musicBox.MusicInfoObjectList.Find
            (GameObject => GameObject.name == MainSystem.mainSystem.songName);

        GameMusic.clip = InfoObject.GetComponent<SongInfo>().GameMusic;
        GameJacket.sprite = InfoObject.GetComponent<SongInfo>().MusicJacket;

        SongInfo[0].text = InfoObject.GetComponent<SongInfo>().SongName;
        SongInfo[1].text = InfoObject.GetComponent<SongInfo>().WhoMade;

        GameJacketShadow.sprite = difficultyBoxSprite[MainSystem.mainSystem.difficultyNum];
        DifficultyBox.sprite = difficultyBoxSprite[MainSystem.mainSystem.difficultyNum + 4];

        NoteMove.noteMove.startDelay = InfoObject.GetComponent<SongInfo>().StartDelay;

        MainSystem.mainSystem.bpm = InfoObject.GetComponent<SongInfo>().bpm;

        if (MainSystem.mainSystem.difficulty < 10)
                { LevelNumText.text = "0" + (MainSystem.mainSystem.difficulty).ToString(); }
        else { LevelNumText.text = (MainSystem.mainSystem.difficulty).ToString(); }

        var main = particle.main;
        float num = Convert.ToSingle(MainSystem.mainSystem.bpm * MainSystem.mainSystem.highSpeed) / 100;

        Debug.Log(num);

        //main.simulationSpeed = 3.5f * num;
    }
}
