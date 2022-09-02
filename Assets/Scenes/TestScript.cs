using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    KeyCode[] Locked = {KeyCode.Escape, KeyCode.Return};
    KeyCode[] input01 = {KeyCode.Z, KeyCode.M};
    KeyCode[] input02 = {KeyCode.X, KeyCode.C};
    KeyCode[] input03 = {KeyCode.V, KeyCode.B};
    KeyCode targetKeycode = KeyCode.Return;
    private bool nowOnBinding = false;

    private void Update()
    {
        if (nowOnBinding) { return; }
        if (Input.GetKeyDown(targetKeycode))
        {
            print("Input : " + targetKeycode.ToString());
        }
    }

    public void BindingButton()
    {
        if (nowOnBinding) { return; }
        StartCoroutine(IStartBinding());
    }

    private IEnumerator IStartBinding()
    {
        while (true)
        {
            if (Input.anyKeyDown)
            {
                foreach (KeyCode kc in Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKeyDown(kc))
                    {
                        bool _exist = false;
                        if (Array.Exists(Locked, item => item == kc)) { _exist = true; }
                        if (Array.Exists(input01, item => item == kc)) { _exist = true; }
                        if (Array.Exists(input02, item => item == kc)) { _exist = true; }
                        if (Array.Exists(input03, item => item == kc)) { _exist = true; }
                        if (_exist) { print("Key is Already Exist"); }
                        else { targetKeycode = kc; }
                        yield break;
                    }
                }
            }
            yield return null;
        }
    }
}
