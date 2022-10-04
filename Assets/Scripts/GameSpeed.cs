using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameSpeed : MonoBehaviour
{
    [SerializeField] TextMeshPro speedTmp;
    private int speed;
    private void Awake()
    {
        speed = GameManager.s_Multiply;
        speedTmp.text = string.Format("{0:F2}", speed / 100.0f);
    }
    private void Update()
    {
        if(Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
            {
                ChangeSpeed(100);
            }
            else { ChangeSpeed(10); }
        }
        if(Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
            {
                ChangeSpeed(100);
            }
            else { ChangeSpeed(10); }
        }
    }
    public void ChangeSpeed(int _Speed)
    {
        speed += _Speed;
        if (_Speed <= 500) { _Speed = 500; }
        else if (_Speed >= 2000) { _Speed = 2000; }
        UserManager.UserOptionData.s_DefaultGameSpeed = speed;
        GameManager.s_Multiply = speed;
        UserManager.SavePlayerData();
        speedTmp.text = string.Format("{0:F2}", speed / 100.0f);
    }
}
