using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ComboSystem : MonoBehaviour
{
    private static ComboSystem comboSystem;
    public static int s_playCombo;
    public static int s_playMaxCombo;
    public static bool s_isSemiPerfect;
    public static bool s_isPerfect;
    public static bool isMaximum;
    private int typeIndex;
    private int ColorIndex;
    private int comboTrigger;
    private static Sprite[] nowSprite;
    [SerializeField] SpriteRenderer[] ComboRenderer;
    [SerializeField] Sprite[] SemiPerfectSprite;
    [SerializeField] Sprite[] PerfectSprite;
    [SerializeField] Sprite[] MaximumSprite;
    [SerializeField] Sprite[] NormalSprite;
    [SerializeField] Animator ComboAnimator;

    private void Awake() { comboSystem = this; }
    public static void ResetComboSystem()
    {
        s_playCombo = 0;
        s_isSemiPerfect = true;
        s_isPerfect = true;
        isMaximum = true;

        comboSystem.typeIndex = 0;
        comboSystem.ColorIndex = 0;
        comboSystem.comboTrigger = 0;

        nowSprite = comboSystem.SemiPerfectSprite;
    }

    public static void AddCombo(bool _isSemi = false, bool _isPerfect = false)
    {
        s_playCombo++;
        if (s_playCombo > s_playMaxCombo) { s_playMaxCombo = s_playCombo; }

        if ( s_isSemiPerfect )
        {
            if (!_isPerfect && !_isSemi)
            {
                s_isSemiPerfect = false;
                s_isPerfect = false;
                nowSprite = comboSystem.MaximumSprite;
            }
            else if (!_isSemi)
            {
                s_isPerfect = false;
                nowSprite = comboSystem.PerfectSprite;
            }
        }
        else if ( s_isPerfect )
        {
            if (!_isPerfect && !_isSemi)
            {
                s_isPerfect = false;
                nowSprite = comboSystem.MaximumSprite;
            }
        }
    }

    public static void ComboCutoff()
    {
        if (isMaximum)
        {
            isMaximum = false;
            nowSprite = comboSystem.NormalSprite;
        }
        s_playCombo = 0;
        comboSystem.ResetComboInfo();
    }

    private void DisplayComboInfo()
    {
        if (ColorIndex > 3)
        {
            //* 콤보 수 9999 초과
        }
        else if (s_playCombo > comboTrigger)
        {
            ComboRenderer[3 - ColorIndex].color = new Color32(150, 150, 150, 255);
            ColorIndex++;
            comboTrigger = Mathf.FloorToInt(Mathf.Pow(10, ColorIndex));
        }
    }

    private void ResetComboInfo()
    {
        comboTrigger = 0;
        typeIndex = 0;
        ColorIndex = 0;
        comboTrigger = 0;
    }
}
