using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemPosition : MonoBehaviour
{
    [SerializeField]
    GameObject Camera;

    Vector3 recentA;
    Quaternion recentB;

    private void Start()
    {
        recentA = new Vector3(0, 0, 0);
        recentB = new Quaternion(0, 0, 0, 0);
    }

    void Update()
    {
        if (Camera == null)
        {
            Camera = GameObject.Find("Main Camera");
        }
        try
        {
            Vector3 posA = Camera.transform.GetChild(0).position;
            Quaternion posB = Camera.transform.GetChild(0).rotation;

            if (recentA != posA || recentB != posB)
            {
                recentA = posA;
                recentB = posB;

                this.gameObject.transform.localPosition = posA;
                this.gameObject.transform.localRotation = posB;
            }
        }
        catch { Camera = GameObject.Find("Main Camera"); }
    }
}
