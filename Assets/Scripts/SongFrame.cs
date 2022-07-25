using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SongFrame : MonoBehaviour
{
    [SerializeField]
    static AudioSource Audio;
    [SerializeField]
    static AudioClip SecretAudio;
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
        SongNameTmp.text = music.MusicName;
        SongArtistTmp.text = music.MusicArtist;
        for (int i = 0; i < 5; i++)
        {
            Difficulty[i].gameObject.SetActive(music.isAvailable[i]);
            Difficulty[i].GetComponentInChildren<TextMeshPro>().text 
                = string.Format("{d:02}", music.Difficulty[i]);
        }
        switch (music.status) //** Setting Dream Difficulty Sprite
        {
            case Music.Status.Null:
                return;
            case Music.Status.Hexagon:
                Difficulty[4].sprite = SpriteManager.getDreamSprite(0);
                break;
            case Music.Status.Butterfly:
                Difficulty[4].sprite = SpriteManager.getDreamSprite(1);
                break;
        }
        

    }
}
