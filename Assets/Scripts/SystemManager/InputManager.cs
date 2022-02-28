using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public KeyCode[] MainInputCode = new KeyCode[5];
    public KeyCode[] SubInputCode = new KeyCode[5];

    public Dictionary<string, KeyCode> keybinds { get; set; }

    private string bindName;

    private void Awake()
    {
        keybinds = new Dictionary<string, KeyCode>();
    }

    public void BindKey(string key, KeyCode keyBind)
    {
        Dictionary<string, KeyCode> currentDictionary = keybinds;

        if (!currentDictionary.ContainsValue(keyBind))
        {
            currentDictionary.Add(key, keyBind);
        }
        else if (currentDictionary.ContainsValue(keyBind))
        {
            string inputKey = currentDictionary.FirstOrDefault(x => x.Value == keyBind).Key;

            currentDictionary[inputKey] = KeyCode.None;
        }

        currentDictionary[key] = keyBind;
        bindName = string.Empty;
    }
}
