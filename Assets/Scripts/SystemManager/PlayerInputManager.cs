using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
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
}

public static class Default_A
{
    public static KeyCode[] Line1 = { KeyCode.Z, KeyCode.M };
    public static KeyCode[] Line2 = { KeyCode.X, KeyCode.Comma };
    public static KeyCode[] Line3 = { KeyCode.C, KeyCode.Period };
    public static KeyCode[] Line4 = { KeyCode.V, KeyCode.Slash };
    public static KeyCode[] Line5 = { KeyCode.LeftShift, KeyCode.None };
    public static KeyCode[] Line6 = { KeyCode.RightShift, KeyCode.None };
}

public static class Default_B
{
    public static KeyCode[] Line1 = { KeyCode.A, KeyCode.K };
    public static KeyCode[] Line2 = { KeyCode.S, KeyCode.L };
    public static KeyCode[] Line3 = { KeyCode.D, KeyCode.Semicolon };
    public static KeyCode[] Line4 = { KeyCode.F, KeyCode.Quote };
    public static KeyCode[] Line5 = { KeyCode.LeftShift, KeyCode.None };
    public static KeyCode[] Line6 = { KeyCode.RightShift, KeyCode.None };
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
