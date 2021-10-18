using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SongFrame : MonoBehaviour
{
    [SerializeField]
    public SpriteRenderer MusicJacket;

    [SerializeField]
    public SpriteRenderer MusicBlock;

    [SerializeField]
    public SpriteRenderer[] Difficulty;

    [SerializeField]
    public TextMeshPro[] DifficultyTmp;

    [SerializeField]
    public TextMeshPro ScoreTmp;

    [SerializeField]
    public TextMeshPro SongNameTmp;

    [SerializeField]
    public TextMeshPro ComboTmp;

    [SerializeField]
    public TextMeshPro PureTmp;

    [SerializeField]
    public SpriteRenderer Rank;

    [SerializeField]
    public SpriteRenderer RankPlus;
}
