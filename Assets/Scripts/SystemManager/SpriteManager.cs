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
    [SerializeField] string[] DifficultyText;

    [SerializeField] Sprite[] userTitleSprite;
    [SerializeField] Sprite[] userRatingSprite;

    public Sprite getCharacterSprite(int index)
    {
        try
        {
            return CharacterSprite[index];
        }
        catch { return null; }
    }

    public Sprite getCharacterIconSprite(int index)
    {
        try
        {
            return CharacterIconSprite[index];
        }
        catch { return null; }
    }

    public Sprite getRankSprite(int index)
    {
        try
        {
            return RankSprite[index];
        }
        catch { return null; }
    }

    public Sprite getDifficultySprite(int index)
    {
        try
        {
            return DifficultySprite[index];
        }
        catch { return null; }
    }

    public string getDifficultyText(int index)
    {
        try
        {
            return DifficultyText[index];
        }
        catch { return ""; }
    }

    public Sprite getUserTitleSprite(int index)
    {
        try
        {
            return userTitleSprite[index];
        }
        catch { return null; }
    }

    public Sprite getUserRankIndex(int index)
    {
        try
        {
            return userRatingSprite[index];
        }
        catch { return null; }
    }
}
