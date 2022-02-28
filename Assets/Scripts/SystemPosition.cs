using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemPosition : MonoBehaviour
{
    [SerializeField]
    GameObject SystemPositionObject;

    private void Awake()
    {

    }

    void Update()
    {
        if (SystemPositionObject == null)
        {
            SystemPositionObject = GameObject.FindWithTag("systemPos");
            transform.position
                = SystemPositionObject.transform.position;
            transform.eulerAngles
                = SystemPositionObject.transform.eulerAngles;
        }
    }
}
