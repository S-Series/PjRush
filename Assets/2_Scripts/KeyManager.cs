using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum KeyAction { LINE1, LINE2, LINE3, LINE4, LEFT_SIDE, RIGHT_SIDE, KEY_COUNT}

public static class KeySetting 
    { public static Dictionary<KeyAction, KeyCode> keys = new Dictionary<KeyAction, KeyCode>(); }

public class KeyManager : MonoBehaviour
{
    KeyCode[] DefaultKeys = new KeyCode[] 
        { KeyCode.S, KeyCode.D, KeyCode.L, KeyCode.Colon, KeyCode.A, KeyCode.Quote };

    private void Start()
    { 
        for (int i = 0; i < (int)KeyAction.KEY_COUNT; i++)
        {
            KeySetting.keys.Add((KeyAction)i, DefaultKeys[i]);
        }
    }
}
