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
    TextMeshPro[] DifficultyTmp;

    [SerializeField]
    TextMeshPro ScoreTmp;

    [SerializeField]
    TextMeshPro SongNameTmp;

    [SerializeField]
    TextMeshPro SongArtistTmp;

    [SerializeField]
    public SpriteRenderer Rank;

    [SerializeField]
    public SpriteRenderer RankPlus;
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
            if (music.playHighScore[difficulty] == 0)
            {
                ScoreTmp.text = "Non Played";
            }
            else
            {
                ScoreTmp.text = music.playHighScore[difficulty].ToString();
            }

            for (int i = 0; i < 4; i++)
            {
                if (music.Difficulty[i] == 0)
                {
                    Difficulty[i].gameObject.SetActive(false);
                    DifficultyTmp[i].gameObject.SetActive(false);
                }
                else
                {
                    Difficulty[i].gameObject.SetActive(true);
                    DifficultyTmp[i].gameObject.SetActive(true);
                    DifficultyTmp[i].text = music.Difficulty[i].ToString();
                }
            }

            Rank.sprite = MainSystem.mainSystem.Rank(-1);
            RankPlus.sprite = MainSystem.mainSystem.RankPlus(-1);
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
                ScoreTmp.text = "- - - - - - - - - - -";

                for (int i = 0; i < 4; i++)
                {
                    Difficulty[i].gameObject.SetActive(true);
                    DifficultyTmp[i].gameObject.SetActive(true);
                    DifficultyTmp[i].text = "??";
                }

            }
            else
            {
                MusicBlock[0].enabled = true;
                MusicBlock[1].enabled = false;
                MusicJacket.sprite = music.sprJacket;
                SongNameTmp.text = music.MusicName;
                SongArtistTmp.text = music.MusicArtist;
                ScoreTmp.text = "Not Own";

                for (int i = 0; i < 4; i++)
                {
                    if (music.Difficulty[i] == 0)
                    {
                        Difficulty[i].gameObject.SetActive(false);
                        DifficultyTmp[i].gameObject.SetActive(false);
                    }
                    else
                    {
                        Difficulty[i].gameObject.SetActive(true);
                        DifficultyTmp[i].gameObject.SetActive(true);
                        DifficultyTmp[i].text = music.Difficulty[i].ToString();
                    }
                }
            }

            Rank.sprite = MainSystem.mainSystem.Rank(-1);
            RankPlus.sprite = MainSystem.mainSystem.RankPlus(-1);
        }
    }
}
