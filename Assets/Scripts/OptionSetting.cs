using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionSetting : MonoBehaviour
{
    public static bool s_isSetting = true;
    private int MainIndex = 0;
    private int SubIndex = 0;
    private int MaxSubIndex;
    private IEnumerator mainCoroutine;
    private IEnumerator settingCoroutine;
    [SerializeField] private GameObject SettingFrame;
    [SerializeField] private GameObject[] SettingObject;

    private void Start()
    {
        //* Graphic
        GraphicDropdowns[0].value = UserManager.UserSetting.ScreenResolutionIndex;
        if (UserManager.UserSetting.isFullScreen) { GraphicDropdowns[1].value = 1; }
        else { GraphicDropdowns[1].value = 0; }
        GraphicDropdowns[2].value = UserManager.UserSetting.FrameRateIndex;
        GraphicDropdowns[3].value = UserManager.UserSetting.AntiAliasingIndex;
        //* Input
        InputDropdown.value = (int)(PlayerInputManager.inputStatus);
        KeyShowSetting();
    }
    private void Update()
    {
        if (!s_isSetting) { return; }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            MainSystem.LoadMainScene();
        }
    }

    #region GamePlay Setting
    public void SetToGamePlay()
    {
        SettingFrame.transform.localPosition = new Vector3(-7.9f, 4.0f, 0);
        foreach(GameObject gameObject in SettingObject)
        { gameObject.SetActive(false); }
        SettingObject[0].SetActive(true);
    }
    #endregion

    #region Graphic Setting
    public void SetToGraphic()
    {
        SettingFrame.transform.localPosition = new Vector3(-7.9f, 2.5f, 0);
        foreach(GameObject gameObject in SettingObject)
        { gameObject.SetActive(false); }
        SettingObject[1].SetActive(true);
    }
    [SerializeField] TMP_Dropdown[] GraphicDropdowns;
    public void ChangeScreenResolution()
    {
        bool _isFullScreen;
        int _index = GraphicDropdowns[0].value;
        if (GraphicDropdowns[1].value == 0) { _isFullScreen = false; }
        else { _isFullScreen = true; }
        switch(_index)
        {
            case 00:
                Screen.SetResolution(640, 360, _isFullScreen);
                break;

            case 01:
                Screen.SetResolution(800, 450, _isFullScreen);
                break;

            case 02:
                Screen.SetResolution(960, 540, _isFullScreen);
                break;

            case 03:
                Screen.SetResolution(1120, 630, _isFullScreen);
                break;

            case 04:
                Screen.SetResolution(1280, 720, _isFullScreen);
                break;

            case 05:
                Screen.SetResolution(1440, 810, _isFullScreen);
                break;

            case 06:
                Screen.SetResolution(1600, 900, _isFullScreen);
                break;

            case 07:
                Screen.SetResolution(1760, 990, _isFullScreen);
                break;

            case 08:
                Screen.SetResolution(1920, 1080, _isFullScreen);
                break;

            case 09:
                Screen.SetResolution(2560, 1440, _isFullScreen);
                break;
            
            case 10:
                Screen.SetResolution(3840, 2160, _isFullScreen);
                break;
        }
        ShowFps.ApplyScreenSize();
        UserManager.UserSetting.ScreenResolutionIndex = _index;
        UserManager.UserSetting.isFullScreen = _isFullScreen;
        UserManager.SavePlayerData();
    }
    public void ChangeFrameRate()
    {
        int _index = GraphicDropdowns[2].value;
        switch(_index)
        {
            case 0:
                Application.targetFrameRate = 60;
                QualitySettings.vSyncCount = 0;
                break;

            case 1:
                Application.targetFrameRate = 75;
                QualitySettings.vSyncCount = 0;
                break;
            
            case 2:
                Application.targetFrameRate = 120;
                QualitySettings.vSyncCount = 0;
                break;

            case 3:
                Application.targetFrameRate = 144;
                QualitySettings.vSyncCount = 0;
                break;

            case 4:
                Application.targetFrameRate = 240;
                QualitySettings.vSyncCount = 0;
                break;

            case 5:
                Application.targetFrameRate = 240;
                QualitySettings.vSyncCount = 1;
                break;

            case 6:
                Application.targetFrameRate = 1000;
                QualitySettings.vSyncCount = 2;
                break;
        }
        UserManager.UserSetting.FrameRateIndex = _index;
        UserManager.SavePlayerData();
    }
    public void ChangeScreenAntialliasing()
    {
        int _index = GraphicDropdowns[3].value;
        switch(_index)
        {
            case 0:
                QualitySettings.antiAliasing = 0;
                break;

            case 1:
                QualitySettings.antiAliasing = 2;
                break;

            case 2:
                QualitySettings.antiAliasing = 4;
                break;

            case 3:
                QualitySettings.antiAliasing = 8;
                break;
        }
        UserManager.UserSetting.AntiAliasingIndex = _index;
        UserManager.SavePlayerData();
    }
    #endregion

    #region Input Setting
    public void SetToInput()
    {
        SettingFrame.transform.localPosition = new Vector3(-7.9f, 1.0f, 0);
        foreach(GameObject gameObject in SettingObject)
        { gameObject.SetActive(false); }
        SettingObject[2].SetActive(true);
    }
    private Vector3[] framePosition =
    {
        new Vector3(-4.5f, +1.0f, 0),
        new Vector3(-1.5f, +1.0f, 0),
        new Vector3(+1.5f, +1.0f, 0),
        new Vector3(+4.5f, +1.0f, 0),
        new Vector3(-4.5f, -2.0f, 0),
        new Vector3(-1.5f, -2.0f, 0),
        new Vector3(+1.5f, -2.0f, 0),
        new Vector3(+4.5f, -2.0f, 0),
        new Vector3(-3.0f, -4.5f, 0),
        new Vector3(+3.0f, -4.5f, 0),
    };
    [SerializeField] private TMP_Dropdown InputDropdown;
    [SerializeField] private Button[] InputButtonObject;
    [SerializeField] private TextMeshPro[] InputButtonTmp;
    [SerializeField] private SpriteRenderer BindingKeyFrame;
    public void KeyShowSetting()
    {
        int _index;
        _index = InputDropdown.value;

        if (_index == 0) { PlayerInputManager.inputStatus = PlayerInputManager.InputStatus.Default_A; }
        if (_index == 1) { PlayerInputManager.inputStatus = PlayerInputManager.InputStatus.Default_B; }
        if (_index == 2) 
        {
            foreach(Button btn in InputButtonObject) { btn.interactable = true; }
            PlayerInputManager.inputStatus = PlayerInputManager.InputStatus.UserSetting;
        }
        else { foreach(Button btn in InputButtonObject) { btn.interactable = false; } }

        PlayerInputManager.SetKeyOption();
        InputButtonTmp[01].text = PlayerInputManager.s_Line1[1].ToString();
        InputButtonTmp[05].text = PlayerInputManager.s_Line1[0].ToString();

        InputButtonTmp[02].text = PlayerInputManager.s_Line2[1].ToString();
        InputButtonTmp[06].text = PlayerInputManager.s_Line2[0].ToString();

        InputButtonTmp[03].text = PlayerInputManager.s_Line3[0].ToString();
        InputButtonTmp[07].text = PlayerInputManager.s_Line3[1].ToString();

        InputButtonTmp[04].text = PlayerInputManager.s_Line4[0].ToString();
        InputButtonTmp[08].text = PlayerInputManager.s_Line4[1].ToString();

        InputButtonTmp[09].text = PlayerInputManager.s_Line5[0].ToString();
        InputButtonTmp[10].text = PlayerInputManager.s_Line6[0].ToString();

        PlayerInputManager.SaveKeyOption();
    }
    public void BindAllKey()
    {
        StartCoroutine(IBindAll());
    }
    private IEnumerator IBindAll()
    {
        foreach(Button btn in InputButtonObject) { btn.interactable = false; }

        BindingKeyFrame.transform.localPosition = framePosition[0];
        yield return IBindingKey(1, false);
        InputButtonTmp[01].text = PlayerInputManager.s_Line1[1].ToString();
        BindingKeyFrame.transform.localPosition = framePosition[1];
        yield return IBindingKey(2, false);
        InputButtonTmp[02].text = PlayerInputManager.s_Line2[1].ToString();
        BindingKeyFrame.transform.localPosition = framePosition[2];
        yield return IBindingKey(3, true);
        InputButtonTmp[03].text = PlayerInputManager.s_Line3[0].ToString();
        BindingKeyFrame.transform.localPosition = framePosition[3];
        yield return IBindingKey(4, true);
        InputButtonTmp[04].text = PlayerInputManager.s_Line4[0].ToString();

        BindingKeyFrame.transform.localPosition = framePosition[4];
        yield return IBindingKey(1, true);
        InputButtonTmp[05].text = PlayerInputManager.s_Line1[0].ToString();
        BindingKeyFrame.transform.localPosition = framePosition[5];
        yield return IBindingKey(2, true);
        InputButtonTmp[06].text = PlayerInputManager.s_Line2[0].ToString();
        BindingKeyFrame.transform.localPosition = framePosition[6];
        yield return IBindingKey(3, false);
        InputButtonTmp[07].text = PlayerInputManager.s_Line3[1].ToString();
        BindingKeyFrame.transform.localPosition = framePosition[7];
        yield return IBindingKey(4, false);
        InputButtonTmp[08].text = PlayerInputManager.s_Line4[1].ToString();

        BindingKeyFrame.transform.localPosition = framePosition[8];
        yield return IBindingKey(5, true);
        InputButtonTmp[09].text = PlayerInputManager.s_Line5[0].ToString();
        BindingKeyFrame.transform.localPosition = framePosition[9];
        yield return IBindingKey(6, true);
        InputButtonTmp[10].text = PlayerInputManager.s_Line6[0].ToString();

        foreach(Button btn in InputButtonObject) { btn.interactable = true; }
        PlayerInputManager.SaveKeyOption();
    }
    private IEnumerator IBindingKey(int line, bool isMain)
    {
        BindingKeyFrame.enabled = true;
        while (true)
        {
            if (Input.anyKeyDown)
            {
                foreach (KeyCode kc in Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKeyDown(kc))
                    {
                        if (PlayerInputManager.CheckKeyAvailable(kc))
                        {
                            PlayerInputManager.KeyBinding(line, isMain, kc);
                            BindingKeyFrame.enabled = false;
                            yield break;
                        }
                        else
                        {
                            PlayerInputManager.KeyBinding(line, isMain, KeyCode.None);
                            BindingKeyFrame.enabled = false;
                            yield break;
                        }
                    }
                }
            }
            yield return null;
        }
    }
    #endregion

    #region Sound Setting
    public void SetToSound()
    {
        SettingFrame.transform.localPosition = new Vector3(-7.9f, -0.5f, 0);
        foreach(GameObject gameObject in SettingObject)
        { gameObject.SetActive(false); }
        SettingObject[3].SetActive(true);
    }
    #endregion

    #region Other Setting
    public void SetToOthers()
    {
        SettingFrame.transform.localPosition = new Vector3(-7.9f, -2.0f, 0);
        foreach(GameObject gameObject in SettingObject)
        { gameObject.SetActive(false); }
        SettingObject[4].SetActive(true);
    }
    #endregion
}
