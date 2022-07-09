using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatorManager : MonoBehaviour
{
#if UNITY_EDITOR
    private void Start()
    {
        try
        {
            GameObject.Find("GameField").SetActive(false);
        }
        catch{}
    }
#else
    private void Start()
    {
        Destroy(this.gameObject);
    }
#endif
}
