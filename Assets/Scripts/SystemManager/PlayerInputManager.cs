using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    private static readonly KeyCode[] UnAvailableKey = 
    {
        KeyCode.Escape,KeyCode.Tab,
        KeyCode.F1, KeyCode.F2, KeyCode.F3,
        KeyCode.F4, KeyCode.F5, KeyCode.F6,
        KeyCode.F7, KeyCode.F8, KeyCode.F9,
        KeyCode.F10, KeyCode.F11, KeyCode.F12,
        KeyCode.Delete,KeyCode.End,KeyCode.PageDown,
        KeyCode.Insert,KeyCode.Home,KeyCode.PageUp,
        KeyCode.Numlock,KeyCode.CapsLock,KeyCode.LeftWindows,
        KeyCode.RightWindows,KeyCode.Exclaim,KeyCode.Return,
        KeyCode.Print,KeyCode.ScrollLock,KeyCode.Pause,
        KeyCode.Mouse0,KeyCode.Mouse1,KeyCode.Mouse2,KeyCode.Mouse3,
        KeyCode.Mouse4,KeyCode.Mouse5,KeyCode.Mouse6
    };
    public static KeyCode[] s_Line1 = new KeyCode[2];
    public static KeyCode[] s_Line2 = new KeyCode[2];
    public static KeyCode[] s_Line3 = new KeyCode[2];
    public static KeyCode[] s_Line4 = new KeyCode[2];
    public static KeyCode[] s_Line5 = new KeyCode[2];
    public static KeyCode[] s_Line6 = new KeyCode[2];
    public enum InputStatus
    {
        Default_A, Default_B, UserSetting
    }
    public static InputStatus inputStatus = InputStatus.Default_A;
    private static SaveKeySetting saveKey;

    private void Awake() { LoadKeyOption(); }
    public static void KeyBinding(int _line, bool _isMain, KeyCode _keyCode)
    {
        if (inputStatus != InputStatus.UserSetting) { return; }

        int _index;
        if (_isMain) { _index = 0; }
        else { _index = 1; }

        if (_index == 1 && _line > 4) { return; }

        if (_line == 1) { UserKeySetting.Line1[_index] = _keyCode; }
        else if (_line == 2) { UserKeySetting.Line2[_index] = _keyCode; }
        else if (_line == 3) { UserKeySetting.Line3[_index] = _keyCode; }
        else if (_line == 4) { UserKeySetting.Line4[_index] = _keyCode; }
        else if (_line == 5) { UserKeySetting.Line5[0] = _keyCode; }
        else if (_line == 6) { UserKeySetting.Line6[0] = _keyCode; }

        print(_keyCode);
        print(((int)_keyCode));
    }
    public static void SetKeyOption()
    {
        switch (inputStatus)
        {
            case InputStatus.Default_A:
                s_Line1 = Default_A.Line1;
                s_Line2 = Default_A.Line2;
                s_Line3 = Default_A.Line3;
                s_Line4 = Default_A.Line4;
                s_Line5 = Default_A.Line5;
                s_Line6 = Default_A.Line6;
                break;

            case InputStatus.Default_B:
                s_Line1 = Default_B.Line1;
                s_Line2 = Default_B.Line2;
                s_Line3 = Default_B.Line3;
                s_Line4 = Default_B.Line4;
                s_Line5 = Default_B.Line5;
                s_Line6 = Default_B.Line6;
                break;

            case InputStatus.UserSetting:
                s_Line1 = UserKeySetting.Line1;
                s_Line2 = UserKeySetting.Line2;
                s_Line3 = UserKeySetting.Line3;
                s_Line4 = UserKeySetting.Line4;
                s_Line5 = UserKeySetting.Line5;
                s_Line6 = UserKeySetting.Line6;
                break;
        }
    }
    public static bool CheckKeyAvailable(KeyCode _keycode)
    {
        if (_keycode == KeyCode.None) { return true; }
        if (Array.Exists(UnAvailableKey, item => item.Equals(_keycode))) { return false; }
        else 
        {
            if (Array.Exists(s_Line1, item => item == _keycode)) { return false; }
            else if (Array.Exists(s_Line2, item => item == _keycode)) { return false; }
            else if (Array.Exists(s_Line3, item => item == _keycode)) { return false; }
            else if (Array.Exists(s_Line4, item => item == _keycode)) { return false; }
            else if (Array.Exists(s_Line5, item => item == _keycode)) { return false; }
            else if (Array.Exists(s_Line6, item => item == _keycode)) { return false; }
            else { return true; } 
        }
    }
    public static void SaveKeyOption()
    {
        saveKey = new SaveKeySetting();

        saveKey.StatusIndex = (int)inputStatus;

        saveKey.Line1[0] = ((int)UserKeySetting.Line1[0]);
        saveKey.Line1[1] = ((int)UserKeySetting.Line1[1]);
        saveKey.Line2[0] = ((int)UserKeySetting.Line2[0]);
        saveKey.Line2[1] = ((int)UserKeySetting.Line2[1]);
        saveKey.Line3[0] = ((int)UserKeySetting.Line3[0]);
        saveKey.Line3[1] = ((int)UserKeySetting.Line3[1]);
        saveKey.Line4[0] = ((int)UserKeySetting.Line4[0]);
        saveKey.Line4[1] = ((int)UserKeySetting.Line4[1]);
        saveKey.Line5[0] = ((int)UserKeySetting.Line5[0]);
        saveKey.Line5[1] = ((int)UserKeySetting.Line5[1]);
        saveKey.Line6[0] = ((int)UserKeySetting.Line6[0]);
        saveKey.Line6[1] = ((int)UserKeySetting.Line6[1]);

        if (!Directory.Exists(Application.dataPath + "/_PlayData/"))
            { Directory.CreateDirectory(Application.dataPath + "/_PlayData/"); }

        string _path = Application.dataPath + "/_PlayData/0000.json";
        File.WriteAllText(_path, Utils.EncryptAES(JsonUtility.ToJson(saveKey)));
    }
    private static void LoadKeyOption()
    {
        SaveKeySetting _saveKey = new SaveKeySetting();
        string _path = Application.dataPath + "/_PlayData/0000.json";

        if (File.Exists(_path))
        {
            _saveKey = JsonUtility.FromJson<SaveKeySetting>
                (Utils.DecryptAES(File.ReadAllText(_path)));
            if (_saveKey.StatusIndex == 0) { inputStatus = InputStatus.Default_A; }
            if (_saveKey.StatusIndex == 1) { inputStatus = InputStatus.Default_B; }
            if (_saveKey.StatusIndex == 2) { inputStatus = InputStatus.UserSetting; }

            foreach (KeyCode kc in Enum.GetValues(typeof(KeyCode)))
                { if ((int)kc == _saveKey.Line1[0]) { UserKeySetting.Line1[0] = kc; } }
            foreach (KeyCode kc in Enum.GetValues(typeof(KeyCode)))
                { if ((int)kc == _saveKey.Line1[1]) { UserKeySetting.Line1[1] = kc; } }
            foreach (KeyCode kc in Enum.GetValues(typeof(KeyCode)))
                { if ((int)kc == _saveKey.Line2[0]) { UserKeySetting.Line2[0] = kc; } }
            foreach (KeyCode kc in Enum.GetValues(typeof(KeyCode)))
                { if ((int)kc == _saveKey.Line2[1]) { UserKeySetting.Line2[1] = kc; } }
            foreach (KeyCode kc in Enum.GetValues(typeof(KeyCode)))
                { if ((int)kc == _saveKey.Line3[0]) { UserKeySetting.Line3[0] = kc; } }
            foreach (KeyCode kc in Enum.GetValues(typeof(KeyCode)))
                { if ((int)kc == _saveKey.Line3[1]) { UserKeySetting.Line3[1] = kc; } }
            foreach (KeyCode kc in Enum.GetValues(typeof(KeyCode)))
                { if ((int)kc == _saveKey.Line4[0]) { UserKeySetting.Line4[0] = kc; } }
            foreach (KeyCode kc in Enum.GetValues(typeof(KeyCode)))
                { if ((int)kc == _saveKey.Line4[1]) { UserKeySetting.Line4[1] = kc; } }
            foreach (KeyCode kc in Enum.GetValues(typeof(KeyCode)))
                { if ((int)kc == _saveKey.Line5[0]) { UserKeySetting.Line5[0] = kc; } }
            foreach (KeyCode kc in Enum.GetValues(typeof(KeyCode)))
                { if ((int)kc == _saveKey.Line5[1]) { UserKeySetting.Line5[1] = kc; } }
            foreach (KeyCode kc in Enum.GetValues(typeof(KeyCode)))
                { if ((int)kc == _saveKey.Line6[0]) { UserKeySetting.Line6[0] = kc; } }
            foreach (KeyCode kc in Enum.GetValues(typeof(KeyCode)))
                { if ((int)kc == _saveKey.Line6[1]) { UserKeySetting.Line6[1] = kc; } }
        }
    
        SetKeyOption();
    }
}

public static class Default_A
{
    public static KeyCode[] Line1 = { KeyCode.D, KeyCode.None };
    public static KeyCode[] Line2 = { KeyCode.F, KeyCode.H };
    public static KeyCode[] Line3 = { KeyCode.G, KeyCode.J };
    public static KeyCode[] Line4 = { KeyCode.None, KeyCode.K };
    public static KeyCode[] Line5 = { KeyCode.S, KeyCode.None };
    public static KeyCode[] Line6 = { KeyCode.L, KeyCode.None };
}

public static class Default_B
{
    public static KeyCode[] Line1 = { KeyCode.S, KeyCode.J };
    public static KeyCode[] Line2 = { KeyCode.D, KeyCode.K };
    public static KeyCode[] Line3 = { KeyCode.F, KeyCode.L };
    public static KeyCode[] Line4 = { KeyCode.G, KeyCode.Semicolon };
    public static KeyCode[] Line5 = { KeyCode.A, KeyCode.None };
    public static KeyCode[] Line6 = { KeyCode.Quote, KeyCode.None };
}

public static class UserKeySetting
{
    public static KeyCode[] Line1 = new KeyCode[2];
    public static KeyCode[] Line2 = new KeyCode[2];
    public static KeyCode[] Line3 = new KeyCode[2];
    public static KeyCode[] Line4 = new KeyCode[2];
    public static KeyCode[] Line5 = new KeyCode[2];
    public static KeyCode[] Line6 = new KeyCode[2];
}
public class SaveKeySetting
{
    public int StatusIndex;
    public int[] Line1 = new int[2];
    public int[] Line2 = new int[2];
    public int[] Line3 = new int[2];
    public int[] Line4 = new int[2];
    public int[] Line5 = new int[2];
    public int[] Line6 = new int[2];
}
