using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RankingBox : MonoBehaviour
{
    [SerializeField]
    TextMeshPro[] RankingInfo;
    [SerializeField]
    SpriteRenderer CharacterIcon;

    // 레벨, 닉네임, 퓨어비율, 점수, 캐릭터인덱스
    public void SetRankingInfo(string[] info)
    {
        RankingInfo[0].text = "LV." + String.Format("{0:D4}", Int32.Parse(info[0]));
        RankingInfo[1].text = info[1];
        RankingInfo[2].text = String.Format("{0:F2}",float.Parse(info[2])) + "%";
        RankingInfo[3].text = String.Format("{0:D9}",Int32.Parse(info[3]));
        CharacterIcon.sprite = MainSystem.spriteManager.getCharacterIconSprite(Int32.Parse(info[3]));
    }
}
