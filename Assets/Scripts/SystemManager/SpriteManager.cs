using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : MonoBehaviour
{
    public static SpriteManager spriteManager;
    private void Awake() { spriteManager = this; }
    [SerializeField] Sprite[] CharacterSprite;
    [SerializeField] Sprite[] CharacterIconSprite;

    [SerializeField] Sprite[] RankSprite;

    [SerializeField] Sprite[] DifficultySprite;
    [SerializeField] Sprite[] DreamDifficultySprite;
    [SerializeField] string[] DifficultyText;

    [SerializeField] Sprite[] userTitleSprite;
    [SerializeField] Sprite[] userRatingSprite;

    public static Sprite getCharacterSprite(int index)
    {
        try
        {
            return spriteManager.CharacterSprite[index];
        }
        catch { return null; }
    }
    public static Sprite getCharacterIconSprite(int index)
    {
        try
        {
            return spriteManager.CharacterIconSprite[index];
        }
        catch { return null; }
    }
    public static Sprite getRankSprite(bool isDetail = false, bool isPerfect = false, bool isMax = false, int score = 0)
    {
        if (score == 0) { return null; }
        if (isDetail) { return spriteManager.RankSprite[0]; }
        else if (isPerfect) { return spriteManager.RankSprite[1]; }
        else if (isMax) { return spriteManager.RankSprite[2]; }
        else if (score >= 99000000) { return spriteManager.RankSprite[3]; }
        else if (score >= 98000000) { return spriteManager.RankSprite[4]; }
        else if (score >= 97000000) { return spriteManager.RankSprite[5]; }
        else if (score >= 96000000) { return spriteManager.RankSprite[6]; }
        else if (score >= 95000000) { return spriteManager.RankSprite[7]; }
        else if (score >= 92500000) { return spriteManager.RankSprite[8]; }
        else if (score >= 90000000) { return spriteManager.RankSprite[9]; }
        else if (score >= 87500000) { return spriteManager.RankSprite[10]; }
        else if (score >= 85000000) { return spriteManager.RankSprite[11]; }
        else if (score >= 82500000) { return spriteManager.RankSprite[12]; }
        else if (score >= 80000000) { return spriteManager.RankSprite[13]; }
        else { return spriteManager.RankSprite[14]; }
    }
    public static Sprite getDifficultySprite(int index)
    {
        try
        {
            return spriteManager.DifficultySprite[index];
        }
        catch { return null; }
    }
    public static Sprite getDreamSprite(Music.Status status)
    {
        switch (status)
        {
            case Music.Status.Hexagon:
                return spriteManager.DreamDifficultySprite[1];
                
            case Music.Status.Butterfly:
                return spriteManager.DreamDifficultySprite[2];

            default:
            case Music.Status.Null:
                return spriteManager.DreamDifficultySprite[0];
        }
    }
    public static string getDifficultyText(int index)
    {
        try
        {
            return spriteManager.DifficultyText[index];
        }
        catch { return ""; }
    }
    public static Sprite getUserTitleSprite(int index)
    {
        try
        {
            return spriteManager.userTitleSprite[index];
        }
        catch { return null; }
    }
    public static Sprite getUserRankIndex(int index)
    {
        try
        {
            return spriteManager.userRatingSprite[index];
        }
        catch { return null; }
    }
}
