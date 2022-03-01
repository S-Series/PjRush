using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class KeySetting
{
    public static Dictionary<KeyActions, KeyCode> keys = new Dictionary<KeyActions, KeyCode>();
}

public class InputManager : MonoBehaviour
{
    int key = -1;

    KeyCode[] defaultKeys = new KeyCode[]
    {
        KeyCode.Z,
        KeyCode.X,
        KeyCode.C,
        KeyCode.V,
        KeyCode.LeftShift,
        KeyCode.M,
        KeyCode.Comma,
        KeyCode.Period,
        KeyCode.Slash,
        KeyCode.RightShift,
        KeyCode.None
    };

    private void Start()
    {
        for (int i = 0; i < (int)KeyActions.KeyCount; i++)
        {
            KeySetting.keys.Add((KeyActions)i, defaultKeys[i]);
        }
    }

    private void OnGUI()
    {
        Event keyEvent = Event.current;
        if (keyEvent.isKey)
        {
            KeySetting.keys[(KeyActions)key] = keyEvent.keyCode;
            key = -1;
        }
    }

    public void ChangeKey(int num)
    {
        key = num;
    }
}

public enum KeyActions
{
    Line1,
    Line2,
    Line3,
    Line4,
    LineBottom,
    SubLine1,
    SubLine2,
    SubLine3,
    SubLine4,
    SubLineBottom,
    KeyCount
}
