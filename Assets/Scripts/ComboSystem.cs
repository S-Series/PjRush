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
    public static bool s_isMaximum;
    private int animateIndex;
    private int ColorIndex;
    private int comboTrigger;
    private static Sprite[] nowSprite;
    [SerializeField] SpriteRenderer[] ComboRenderer;
    [SerializeField] SpriteRenderer[] ComboAnimateRenderer;
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
        s_isMaximum = true;

        // nowSprite = comboSystem.SemiPerfectSprite;
        nowSprite = comboSystem.PerfectSprite;
        comboSystem.ResetComboInfo();
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
                nowSprite = comboSystem.NormalSprite;
            }
        }
        comboSystem.DisplayComboInfo();
    }

    public static void ComboCutoff()
    {
        if (s_isMaximum)
        {
            s_isMaximum = false;
            s_isPerfect = false;
            s_isSemiPerfect = false;
            nowSprite = comboSystem.NormalSprite;
        }
        s_playCombo = 0;
        comboSystem.ResetComboInfo();
    }

    private void DisplayComboInfo()
    {
        string _charCombo = String.Format("{0:D4}", s_playCombo);
        ComboRenderer[0].sprite = nowSprite[int.Parse(_charCombo[0].ToString())];
        ComboRenderer[1].sprite = nowSprite[int.Parse(_charCombo[1].ToString())];
        ComboRenderer[2].sprite = nowSprite[int.Parse(_charCombo[2].ToString())];
        ComboRenderer[3].sprite = nowSprite[int.Parse(_charCombo[3].ToString())];

        if (ColorIndex > 3)
        {
            //* 콤보 수 9999 초과
        }
        else if (s_playCombo > comboTrigger)
        {
            ComboRenderer[3 - ColorIndex].color = new Color32(255, 255, 255, 255);
            ColorIndex++;
            comboTrigger = Mathf.FloorToInt(Mathf.Pow(10, ColorIndex));
        }
        return;     //! Debugging Return
        if (s_playCombo >= 50 * animateIndex)
        {
            ComboAnimateRenderer[0].sprite = nowSprite[Mathf.FloorToInt(animateIndex / 20.0f)];
            ComboAnimateRenderer[1].sprite = nowSprite[Mathf.FloorToInt((animateIndex % 20) / 2.0f)];
            if (animateIndex % 2 == 0) { ComboAnimateRenderer[2].sprite = nowSprite[0]; }
            else { ComboAnimateRenderer[2].sprite = nowSprite[5]; }
            // ComboAnimateRenderer[3].sprite = nowSprite[0]; //* 고정이라서 바꾸지 않음
            ComboAnimator.SetTrigger("Play");
            animateIndex++;
        }
    }

    private void ResetComboInfo()
    {
        comboTrigger = 0;
        animateIndex = 1;
        ColorIndex = 0;
        comboTrigger = 0;
        ComboRenderer[0].sprite = nowSprite[0];
        ComboRenderer[0].color = new Color32(150, 150, 150, 255);

        ComboRenderer[1].sprite = nowSprite[0];
        ComboRenderer[1].color = new Color32(150, 150, 150, 255);

        ComboRenderer[2].sprite = nowSprite[0];
        ComboRenderer[2].color = new Color32(150, 150, 150, 255);

        ComboRenderer[3].sprite = nowSprite[0];
        ComboRenderer[3].color = new Color32(150, 150, 150, 255);
    }
}
