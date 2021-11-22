using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayMusic : MonoBehaviour
{
    public static DisplayMusic displayMusic;

    public int sorting;
    public bool isReverse;
    /// <summary>
    /// 1 ~ 4 -> easy, normal, hard, extra의 난이도 등급 순으로 정렬
    /// </summary>

    [SerializeField]
    Sprite[] SpRank;

    [SerializeField]
    Sprite[] SpPlus;

    [SerializeField]
    public GameObject[] MusicDisplayBox;

    [SerializeField]
    TopBoxSetting topBox;

    private void Awake()
    {
        displayMusic = this;
    }

    void Start()
    {
        sorting = 5;
        isReverse = true;
    }

    void Update()
    {

    }

    public void DisplayMusicInfo()
    {
        int index;
        index = MusicSelectAct.musicSelectAct.firstIndex;
        int indexNum;
        indexNum = MusicSelectAct.musicSelectAct.NowOnIndex;
        int dif;
        dif = MusicSelectAct.musicSelectAct.NowOnDifficulty;
        SongInfo songInfo;
        SongFrame songFrame;

        for (int i = 0; i < 8; i++)
        {
            try
            {
                songInfo = MusicBox.musicBox.MusicInfoObjectList[index + i].GetComponent<SongInfo>();
                songFrame = MusicDisplayBox[i].GetComponent<SongFrame>();

                try
                {
                    MusicDisplayBox[i].SetActive(true);
                    // 음악 자켓 설정
                    songFrame.MusicJacket.sprite = songInfo.MusicJacket;
                    // 음악 이름 설정
                    songFrame.SongNameTmp.text = songInfo.SongName;
                    // 각 4단계 난이도 설정
                    for (int j = 0; j < 4; j++)
                    {
                        if (songInfo.difficulty[j] != 0)
                        {
                            songFrame.Difficulty[j].gameObject.SetActive(true);
                            songFrame.DifficultyTmp[j].gameObject.SetActive(true);

                            if (songInfo.difficulty[j] < 10)
                            {
                                songFrame.DifficultyTmp[j].text = "0" + songInfo.difficulty[j].ToString();
                            }
                            else { songFrame.DifficultyTmp[j].text = songInfo.difficulty[j].ToString(); }
                        }
                        else
                        {
                            songFrame.Difficulty[j].gameObject.SetActive(false);
                            songFrame.DifficultyTmp[j].gameObject.SetActive(false);
                        }
                    }
                    // 맥스콤보 설정
                    songFrame.ComboTmp.text
                        = songInfo.MaxCombo[dif].ToString() + "/" + songInfo.NoteCount[dif].ToString();
                    // 퓨어 설정
                    songFrame.PureTmp.text
                        = songInfo.MaxPure[dif].ToString() + "/" + songInfo.NoteCount[dif].ToString();
                    // 음악 가림막 설정
                    if (songInfo.is_SongOwned[dif] == true)
                    {
                        songFrame.MusicBlock.gameObject.SetActive(false);
                    }
                    else { songFrame.MusicBlock.gameObject.SetActive(true); }
                    // 점수 설정
                    if (songInfo.HighScore[dif] >= 1000000)
                    {
                        string str1;
                        string str2;
                        string str3;
                        // 앞 3자리
                        str1 = ((songInfo.HighScore[dif] - songInfo.HighScore[dif] % 1000000) / 1000000).ToString();
                        // 중간 3자리
                        if ((songInfo.HighScore[dif] % 1000000 - songInfo.HighScore[dif] % 1000) / 1000 < 10)
                        {
                            str2 = "00" + ((songInfo.HighScore[dif] % 1000000 - songInfo.HighScore[dif] % 1000) / 1000).ToString();
                        }
                        else if ((songInfo.HighScore[dif] % 1000000 - songInfo.HighScore[dif] % 1000) / 1000 < 100)
                        {
                            str2 = "0" + ((songInfo.HighScore[dif] % 1000000 - songInfo.HighScore[dif] % 1000) / 1000).ToString();
                        }
                        else
                        {
                            str2 = ((songInfo.HighScore[dif] % 1000000 - songInfo.HighScore[dif] % 1000) / 1000).ToString();
                        }
                        // 뒤 3자리
                        if (songInfo.HighScore[dif] % 1000 < 10)
                        {
                            str3 = "00" + (songInfo.HighScore[dif] % 1000).ToString();
                        }
                        else if (songInfo.HighScore[dif] % 1000 < 100)
                        {
                            str3 = "0" + (songInfo.HighScore[dif] % 1000).ToString();
                        }
                        else
                        {
                            str3 = (songInfo.HighScore[dif] % 1000).ToString();
                        }
                        songFrame.ScoreTmp.text = str1 + "," +  str2 + "," + str3;
                    }
                    else if (songInfo.HighScore[dif] >= 1000)
                    {
                        string str1;
                        string str2;
                        str1 = ((songInfo.HighScore[dif] - songInfo.HighScore[dif] % 1000) / 1000).ToString();
                        // 뒤 3자리
                        if (songInfo.HighScore[dif] % 1000 < 100)
                        {
                            str2 = "0" + (songInfo.HighScore[dif] % 1000).ToString();
                        }
                        else if (songInfo.HighScore[dif] % 1000 < 10)
                        {
                            str2 = "00" + (songInfo.HighScore[dif] % 1000).ToString();
                        }
                        else
                        {
                            str2 = (songInfo.HighScore[dif] % 1000).ToString();
                        }
                        songFrame.ScoreTmp.text = str1 + "," + str2;
                    }
                    else if (songInfo.HighScore[dif] == 0)
                    {
                        songFrame.ScoreTmp.text = "Non Played";
                    }
                    else { songFrame.ScoreTmp.text = (songInfo.HighScore[dif] % 1000).ToString(); }
                    // 랭크 설정
                    if (songInfo.HighScore[dif] == 0)
                    {
                        // Non Played
                        songFrame.Rank.sprite = null;
                        songFrame.RankPlus.sprite = null;
                    }
                    else if (songInfo.NoteCount[dif] == songInfo.MaxPure[dif])
                    {
                        // PR+
                        songFrame.Rank.sprite = SpRank[0];
                        songFrame.RankPlus.sprite = SpPlus[0];
                    }
                    else if (songInfo.HighScore[dif] == 100000000)
                    {
                        // PR
                        songFrame.Rank.sprite = SpRank[0];
                        songFrame.RankPlus.sprite = null;
                    }
                    else if (songInfo.NoteCount[dif] == songInfo.MaxCombo[dif])
                    {
                        // FR
                        songFrame.Rank.sprite = SpRank[1];
                        songFrame.RankPlus.sprite = null;
                    }
                    else if (songInfo.HighScore[dif] >= 97500000)
                    {
                        // AA+
                        songFrame.Rank.sprite = SpRank[2];
                        songFrame.RankPlus.sprite = SpPlus[1];
                    }
                    else if (songInfo.HighScore[dif] >= 95000000)
                    {
                        // AA
                        songFrame.Rank.sprite = SpRank[2];
                        songFrame.RankPlus.sprite = null;
                    }
                    else if (songInfo.HighScore[dif] >= 92500000)
                    {
                        // A+
                        songFrame.Rank.sprite = SpRank[3];
                        songFrame.RankPlus.sprite = SpPlus[1];
                    }
                    else if (songInfo.HighScore[dif] >= 90000000)
                    {
                        // A
                        songFrame.Rank.sprite = SpRank[3];
                        songFrame.RankPlus.sprite = null;
                    }
                    else if (songInfo.HighScore[dif] >= 87500000)
                    {
                        // B+
                        songFrame.Rank.sprite = SpRank[4];
                        songFrame.RankPlus.sprite = SpPlus[1];
                    }
                    else if (songInfo.HighScore[dif] >= 85000000)
                    {
                        // B
                        songFrame.Rank.sprite = SpRank[4];
                        songFrame.RankPlus.sprite = null;
                    }
                    else if (songInfo.HighScore[dif] >= 82500000)
                    {
                        // C+
                        songFrame.Rank.sprite = SpRank[5];
                        songFrame.RankPlus.sprite = SpPlus[1];
                    }
                    else if (songInfo.HighScore[dif] >= 80000000)
                    {
                        // C
                        songFrame.Rank.sprite = SpRank[5];
                        songFrame.RankPlus.sprite = null;
                    }
                    else if (songInfo.HighScore[dif] >= 77500000)
                    {
                        // D+
                        songFrame.Rank.sprite = SpRank[6];
                        songFrame.RankPlus.sprite = SpPlus[1];
                    }
                    else if (songInfo.HighScore[dif] >= 75000000)
                    {
                        // D
                        songFrame.Rank.sprite = SpRank[6];
                        songFrame.RankPlus.sprite = null;
                    }
                    else
                    {
                        songFrame.Rank.sprite = SpRank[7];
                        songFrame.RankPlus.sprite = null;
                    }
                }
                catch { Debug.Log("취소됨"); }
            }
            catch { MusicDisplayBox[i].SetActive(false); }
        }
        // Top Box Setting
        songInfo = MusicBox.musicBox.MusicInfoObjectList[index + indexNum].GetComponent<SongInfo>();
        songFrame = MusicDisplayBox[indexNum].GetComponent<SongFrame>();

        topBox.topJacket.sprite = songFrame.MusicJacket.sprite;
        topBox.topRank.sprite = songFrame.Rank.sprite;
        topBox.topPlus.sprite = songFrame.RankPlus.sprite;
        topBox.topScore.text = songFrame.ScoreTmp.text;
        for (int i = 0; i < 4; i++)
        {
            if (songInfo.difficulty[i] != 0)
            {
                topBox.topDifBox[i].gameObject.SetActive(true);
                topBox.topDifText[i].gameObject.SetActive(true);
                if (songInfo.difficulty[i] < 10)
                {
                    topBox.topDifText[i].text = "0" + songInfo.difficulty[i].ToString();
                }
                else { topBox.topDifText[i].text = songInfo.difficulty[i].ToString(); }
            }
            else
            {
                topBox.topDifBox[i].gameObject.SetActive(false);
                topBox.topDifText[i].gameObject.SetActive(false);
            }
        }
        topBox.topSongName.text = songInfo.SongName;
        topBox.topWhoMade.text = songInfo.WhoMade;
        if (songInfo.bpm % 1 == 0)
        {
            topBox.topBpm.text = "BPM\n" + songInfo.bpm.ToString() + ".00";
        }
        else if (songInfo.bpm * 10 % 1 == 0)
        {
            topBox.topBpm.text = "BPM\n" + songInfo.bpm.ToString() + "0";
        }
        else
        {
            topBox.topBpm.text = "BPM\n" + songInfo.bpm.ToString();
        }

        if (songInfo.ClearRate[dif] % 1 == 0)
        {
             topBox.topClearRate.text = "Clear Rate " + songInfo.ClearRate[dif].ToString() + ".00%";
        }
        else if (songInfo.ClearRate[dif] * 10 % 1 == 0)
        {
            topBox.topClearRate.text = "Clear Rate " + songInfo.ClearRate[dif].ToString() + ".0%";
        }
        else
        {
            topBox.topClearRate.text = "Clear Rate " + songInfo.ClearRate[dif].ToString() + "%";
        }


    }

    public void SortingList(int type, int index, bool reverse)
    {
        Debug.Log("실행됨");
        switch (type)
        {
            case 1: //easy 난이도 순에 따른 정렬
                MusicBox.musicBox.MusicInfoObjectList.Sort(delegate (GameObject A, GameObject B)
                {
                    int Aa = 1; int Bb = -1;
                    
                    if (reverse == true) { Aa = -1; Bb = 1; }

                    int dif1 = A.GetComponent<SongInfo>().difficulty[0];
                    int dif2 = B.GetComponent<SongInfo>().difficulty[0];

                    if (dif1 == 0) dif1 = 99; if (dif2 == 0) dif2 = 99;

                    if (dif1 > dif2) return Aa; if (dif1 < dif2) return Bb;
                    if (dif1 == dif2)
                    {
                        if (A.GetComponent<SongInfo>().bpm > B.GetComponent<SongInfo>().bpm) return Aa;
                        else if (A.GetComponent<SongInfo>().bpm < B.GetComponent<SongInfo>().bpm) return Bb;
                        else if (A.GetComponent<SongInfo>().bpm == B.GetComponent<SongInfo>().bpm)
                        {
                            if (A.GetComponent<SongInfo>().endMs > B.GetComponent<SongInfo>().endMs) return Aa;
                            else if (A.GetComponent<SongInfo>().endMs < B.GetComponent<SongInfo>().endMs) return Bb;
                        }
                    }
                    return 0;
                });
                break;

            case 2: //normal 난이도 순에 따른 정렬
                MusicBox.musicBox.MusicInfoObjectList.Sort(delegate (GameObject A, GameObject B)
                {
                    int Aa = 1; int Bb = -1;

                    if (reverse == true) { Aa = -1; Bb = 1; }

                    int dif1 = A.GetComponent<SongInfo>().difficulty[1];
                    int dif2 = B.GetComponent<SongInfo>().difficulty[1];

                    if (dif1 == 0) dif1 = 99; if (dif2 == 0) dif2 = 99;

                    if (dif1 > dif2) return Aa; if (dif1 < dif2) return Bb;
                    if (dif1 == dif2)
                    {
                        if (A.GetComponent<SongInfo>().bpm > B.GetComponent<SongInfo>().bpm) return Aa;
                        else if (A.GetComponent<SongInfo>().bpm < B.GetComponent<SongInfo>().bpm) return Bb;
                        else if (A.GetComponent<SongInfo>().bpm == B.GetComponent<SongInfo>().bpm)
                        {
                            if (A.GetComponent<SongInfo>().endMs > B.GetComponent<SongInfo>().endMs) return Aa;
                            else if (A.GetComponent<SongInfo>().endMs < B.GetComponent<SongInfo>().endMs) return Bb;
                        }
                    }
                    return 0;
                });
                break;

            case 3: //hard 난이도 순에 따른 정렬
                MusicBox.musicBox.MusicInfoObjectList.Sort(delegate (GameObject A, GameObject B)
                {
                    int Aa = 1; int Bb = -1;

                    if (reverse == true) { Aa = -1; Bb = 1; }

                    int dif1 = A.GetComponent<SongInfo>().difficulty[2];
                    int dif2 = B.GetComponent<SongInfo>().difficulty[2];

                    if (dif1 == 0) dif1 = 99; if (dif2 == 0) dif2 = 99;

                    if (dif1 > dif2) return Aa; if (dif1 < dif2) return Bb;
                    if (dif1 == dif2)
                    {
                        if (A.GetComponent<SongInfo>().bpm > B.GetComponent<SongInfo>().bpm) return Aa;
                        else if (A.GetComponent<SongInfo>().bpm < B.GetComponent<SongInfo>().bpm) return Bb;
                        else if (A.GetComponent<SongInfo>().bpm == B.GetComponent<SongInfo>().bpm)
                        {
                            if (A.GetComponent<SongInfo>().endMs > B.GetComponent<SongInfo>().endMs) return Aa;
                            else if (A.GetComponent<SongInfo>().endMs < B.GetComponent<SongInfo>().endMs) return Bb;
                        }
                    }
                    return 0;
                });
                break;

            case 4: //extra 난이도 순에 따른 정렬
                MusicBox.musicBox.MusicInfoObjectList.Sort(delegate (GameObject A, GameObject B)
                {
                    int Aa = 1; int Bb = -1;

                    if (reverse == true) { Aa = -1; Bb = 1; }

                    int dif1 = A.GetComponent<SongInfo>().difficulty[3];
                    int dif2 = B.GetComponent<SongInfo>().difficulty[3];

                    if (dif1 == 0) dif1 = 99; if (dif2 == 0) dif2 = 99;

                    if (dif1 > dif2) return Aa; if (dif1 < dif2) return Bb;
                    if (dif1 == dif2)
                    {
                        if (A.GetComponent<SongInfo>().bpm > B.GetComponent<SongInfo>().bpm) return Aa;
                        else if (A.GetComponent<SongInfo>().bpm < B.GetComponent<SongInfo>().bpm) return Bb;
                        else if (A.GetComponent<SongInfo>().bpm == B.GetComponent<SongInfo>().bpm)
                        {
                            if (A.GetComponent<SongInfo>().endMs > B.GetComponent<SongInfo>().endMs) return Aa;
                            else if (A.GetComponent<SongInfo>().endMs < B.GetComponent<SongInfo>().endMs) return Bb;
                        }
                    }
                    return 0;
                });
                break;

            case 5: //Bpm 순으로 정렬
                MusicBox.musicBox.MusicInfoObjectList.Sort(delegate (GameObject A, GameObject B)
                {
                    int Aa = 1; int Bb = -1;

                    if (reverse == true) { Aa = -1; Bb = 1; }

                    float dif1 = A.GetComponent<SongInfo>().bpm;
                    float dif2 = B.GetComponent<SongInfo>().bpm;

                    if (dif1 == 0) dif1 = 99; if (dif2 == 0) dif2 = 99;

                    if (dif1 > dif2) return Aa; if (dif1 < dif2) return Bb;
                    if (dif1 == dif2)
                    {
                        if (A.GetComponent<SongInfo>().endMs > B.GetComponent<SongInfo>().endMs) return Aa;
                        else if (A.GetComponent<SongInfo>().endMs < B.GetComponent<SongInfo>().endMs) return Bb;
                    }
                    return 0;
                });
                break;
        }
    }
}
