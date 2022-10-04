using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TopBoxSetting : MonoBehaviour
{
    [SerializeField] private SpriteRenderer topJacket;
    [SerializeField] private SpriteRenderer topRank;
    [SerializeField] private SpriteRenderer topPlus;
    [SerializeField] private TextMeshPro topSongName;
    [SerializeField] private TextMeshPro topWhoMade;
    [SerializeField] private TextMeshPro[] topScore;
    [SerializeField] private TextMeshPro topBpm;
    [SerializeField] private TextMeshPro topClearRate;
    [SerializeField] private SpriteRenderer[] topDifBox;
    [SerializeField] private TextMeshPro[] topDifText;
    [SerializeField] private SpriteRenderer[] topBlockSprite;
    public void SetInfo(Music music, int index)
    {
        topJacket.sprite = music.sprJacket;
        //** topRank.sprite = SpriteManager.getRankSprite(music.HighScore[index]);
        //** topPlus.sprite = SpriteManager.getRankSprite(music.HighScore[index]);
        topSongName.text = music.MusicName;
        topWhoMade.text = music.MusicArtist;
        if (music.HighScore[index] == 0)
        {
            for (int i = 0; i < 9; i++) { topScore[i].enabled = false; }
            topScore[9].enabled = true;
        }
        else
        {
            topScore[9].enabled = false;
            string scoreArr = string.Format("{0:D9}", music.HighScore[index]);
            for (int i = 0; i < 9; i++)
            {
                topScore[i].enabled = true;
                if (music.HighScore[index] >= Mathf.Pow(10, 8 - i))
                    { topScore[i].color = new Color32(255, 255, 255, 255); }
                else { topScore[i].color = new Color32(255, 255, 255, 100); }
                topScore[i].text = scoreArr[i].ToString();
            }
        }
        if (music.HighBPM == music.LowBPM) { topBpm.text = "BPM : " + music.HighBPM.ToString(); }
        else { topBpm.text = "BPM : " + music.LowBPM.ToString() + " - " + music.HighBPM.ToString(); }
        for (int i = 0; i < 5; i++)
        {
            if (!music.isAvailable[i]) 
            {
                topDifBox[i].gameObject.SetActive(false);
                topDifText[i].enabled = false;
                topDifText[i].text = "--";
            }
            else
            {
                topDifBox[i].gameObject.SetActive(true);
                topDifText[i].enabled = true;
                topDifText[i].text = string.Format("{0:D2}", music.Difficulty[i]);
            }
        }
        if (music.isAvailable[4]) 
            { topDifBox[4].sprite = SpriteManager.getDreamSprite(music.status); }
        if (music.isSecret) 
        {
            topBlockSprite[0].enabled = true;
            topBlockSprite[1].enabled = false;
        }
        else if (!music.isOwned[index])
        {
            topBlockSprite[0].enabled = false;
            topBlockSprite[1].enabled = true;
        }
        else
        {
            topBlockSprite[0].enabled = false;
            topBlockSprite[1].enabled = false;
        }
    }
}
