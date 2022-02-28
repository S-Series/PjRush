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
    [SerializeField]
    public Music music;

    #region FrameObject
    [SerializeField]
    SpriteRenderer MusicJacket;

    [SerializeField]
    SpriteRenderer[] MusicBlock;

    [SerializeField]
    SpriteRenderer[] Difficulty;

    [SerializeField]
    TextMeshPro SongNameTmp;

    [SerializeField]
    TextMeshPro SongArtistTmp;

    [SerializeField]
    public SpriteRenderer Rank;
    #endregion

    public void SetInfo(int difficulty)
    {
        if (music.isOwned[difficulty])
        {
            MusicBlock[0].enabled = false;
            MusicBlock[1].enabled = false;
            MusicJacket.sprite = music.sprJacket;
            SongNameTmp.text = music.MusicName;
            SongArtistTmp.text = music.MusicArtist;

            for (int i = 0; i < 4; i++)
            {
                if (music.Difficulty[i] == 0)
                {
                    Difficulty[i].gameObject.SetActive(false);
                }
                else
                {
                    Difficulty[i].gameObject.SetActive(true);
                    Difficulty[i].GetComponentInChildren<TextMeshPro>().text
                        = music.Difficulty[i].ToString();
                }
            }

            Rank.sprite = MainSystem.mainSystem.Rank(-1);
        }
        else
        {
            if (music.isSecret[difficulty])
            {
                MusicBlock[0].enabled = false;
                MusicBlock[1].enabled = true;
                MusicJacket.sprite = music.sprJacket;
                SongNameTmp.text = "????";
                SongArtistTmp.text = "????";

                for (int i = 0; i < 5; i++)
                {
                    Difficulty[i].gameObject.SetActive(true);
                    Difficulty[i].GetComponentInChildren<TextMeshPro>().text = "??";
                }

            }
            else
            {
                MusicBlock[0].enabled = true;
                MusicBlock[1].enabled = false;
                MusicJacket.sprite = music.sprJacket;
                SongNameTmp.text = music.MusicName;
                SongArtistTmp.text = music.MusicArtist;

                for (int i = 0; i < 4; i++)
                {
                    if (music.Difficulty[i] == 0)
                    {
                        Difficulty[i].gameObject.SetActive(false);
                    }
                    else
                    {
                        Difficulty[i].gameObject.SetActive(true);
                        Difficulty[i].GetComponentInChildren<TextMeshPro>().text
                            = string.Format("{0:D2}",music.Difficulty[i]);
                    }
                }
            }

            Rank.sprite = MainSystem.mainSystem.Rank(-1);
        }

        SetDifficultyBox(difficulty);
    }

    private void SetDifficultyBox(int difficulty)
    {
        if (music.Difficulty[difficulty] == 0)
        {
            if (difficulty == 0) return;
            SetDifficultyBox(difficulty - 1);
            return;
        }

        for (int i = 0; i < 5; i++)
        {
            Difficulty[i].gameObject.SetActive(false);
        }
        Difficulty[difficulty].gameObject.SetActive(true);
    }
}
