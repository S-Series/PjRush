﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SongFrame : MonoBehaviour
{
    public AudioClip gameMusic;
    public AudioClip gamePreMusic;
    // -----------------------------------------
    [SerializeField] private SpriteRenderer MusicJacket;
    [SerializeField] private SpriteRenderer[] MusicBlock;
    [SerializeField] private SpriteRenderer[] Difficulty;
    [SerializeField] private TextMeshPro SongNameTmp;
    [SerializeField] private TextMeshPro SongArtistTmp;
    [SerializeField] private SpriteRenderer Rank;
    public void SetFrame(Music music)
    {
        MusicJacket.sprite = music.sprJacket;
        int index;
        index = MusicSelectAct.SelectDifficultyIndex;
        foreach (SpriteRenderer sprite in Difficulty) { sprite.gameObject.SetActive(false); }
        foreach (SpriteRenderer sprite in MusicBlock) { sprite.enabled = false; }
        if (music.isSecret)
        {
            SongNameTmp.text = "????????";
            SongArtistTmp.text = "????????";
            for (int i = 0; i < 5; i++)
            {
                if (music.isAvailable[index])
                {
                    Difficulty[index].gameObject.SetActive(true);
                    Difficulty[index].GetComponentInChildren<TextMeshPro>().text = "??";
                }
                index--;
                if (index < 0) { index += 5; }
            }
            MusicBlock[1].enabled = true;
            gameMusic = null;
            gamePreMusic = null;
        }
        else
        {
            SongNameTmp.text = music.MusicName;
            SongArtistTmp.text = music.MusicArtist;
            int _displayDifficulty = 0;
            for (int i = 0; i < 5; i++)
            {
                _displayDifficulty = index - i;
                if (_displayDifficulty < 0) 
                { _displayDifficulty = Mathf.Abs(_displayDifficulty) + index; }

                if (music.isAvailable[_displayDifficulty])
                {
                    Difficulty[_displayDifficulty].gameObject.SetActive(true);
                    if (music.isOwned[4] && _displayDifficulty == 4)
                    {
                        Difficulty[index].GetComponentInChildren<TextMeshPro>().text = "??";
                        MusicBlock[0].enabled = true;
                    }
                    else
                    {
                        Difficulty[index].GetComponentInChildren<TextMeshPro>().text
                            = string.Format("{0:D2}", music.Difficulty[index]);
                        if (music.isOwned[index]) { MusicBlock[0].enabled = true; }
                    }
                    if (music.NoteCount[_displayDifficulty] == music.PerfectCount[_displayDifficulty])
                        { Rank.sprite = SpriteManager.getRankSprite(isPerfect:true); }
                    else if (music.NoteCount[_displayDifficulty] == music.MaxCombo[_displayDifficulty])
                        { Rank.sprite = SpriteManager.getRankSprite(isMax:true); }
                    else { Rank.sprite = SpriteManager.getRankSprite(score:music.HighScore[_displayDifficulty]); }
                    break;
                }
            }
            gameMusic = music.audMusicFile;
            gamePreMusic = music.audPreMusicFile;
        }
        //Difficulty[4].sprite = SpriteManager.getDreamSprite(music.status);
    }
}
