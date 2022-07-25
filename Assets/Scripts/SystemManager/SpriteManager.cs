﻿using System.Collections;
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
    public static Sprite getRankSprite(int index)
    {
        try
        {
            return spriteManager.RankSprite[index];
        }
        catch { return null; }
    }
    public static Sprite getDifficultySprite(int index)
    {
        try
        {
            return spriteManager.DifficultySprite[index];
        }
        catch { return null; }
    }
    public static Sprite getDreamSprite(int index)
    {
        if (index < 0 || index > spriteManager.DreamDifficultySprite.Length - 1) return null;
        else return spriteManager.DreamDifficultySprite[index];
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
