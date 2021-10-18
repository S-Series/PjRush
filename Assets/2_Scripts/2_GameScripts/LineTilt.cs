using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineTilt : MonoBehaviour
{
    public static LineTilt lineTilt;

    public bool isTiltReset;

    public bool isTiltRight;

    public bool isTiltLeft;

    public float TiltSpeed;

    [SerializeField]
    private float tiltAccurity;

    [SerializeField]
    GameObject lineObject;

    [SerializeField]
    GameObject bossObject;

    private Vector3 pos;

    private void Awake()
    {
        if (lineTilt != this) lineTilt = this;
    }

    void Start()
    {
        isTiltReset = true;
        isTiltLeft = false;
        isTiltRight = false;

        TiltSpeed = 1;

        tiltAccurity = 0;

        pos = lineObject.transform.localPosition;
    }

    void FixedUpdate()
    {
        if (isTiltReset == true)
        {
            tiltAccurity -= tiltAccurity / 750 * TiltSpeed;
        }

        if (isTiltLeft == true)
        {
            tiltAccurity += (-3 - tiltAccurity) / 3000 * TiltSpeed;
        }

        if (isTiltRight == true)
        {
            tiltAccurity += (3 - tiltAccurity) / 3000 * TiltSpeed;
        }

        if (Math.Abs(tiltAccurity) > 3)
        {
            if (tiltAccurity > 0)
            {
                lineObject.transform.localPosition = new Vector3(pos.x + 3 + (tiltAccurity - 3) * 0.1f , pos.y, pos.z);
            }
            else
            {
                lineObject.transform.localPosition = new Vector3(pos.x - 3 - (tiltAccurity - 3) * 0.1f, pos.y, pos.z);
            }
        }
        else
        {
            lineObject.transform.localPosition = new Vector3(pos.x + tiltAccurity, pos.y, pos.z);
        }

        lineObject.transform.eulerAngles = new Vector3(0, 0, tiltAccurity * 10);
    }

    public void setReset()
    {
        isTiltReset = true;
        isTiltLeft = false;
        isTiltRight = false;
    }

    public void setRight()
    {
        isTiltReset = false;
        isTiltLeft = false;
        isTiltRight = true;
    }

    public void setLeft()
    {
        isTiltReset = false;
        isTiltLeft = true;
        isTiltRight = false;
    }
}
