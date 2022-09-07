﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainSelect : MonoBehaviour
{
    //* const --------------------------------------//
    private const int MaxItemCount = 7;
    private const string SendAlertTrigger = "AlertStart";
    private const string EndAlertTrigger = "AlertEnd";
    private const KeyCode Up = KeyCode.UpArrow;
    private const KeyCode Down = KeyCode.DownArrow;
    private const KeyCode Left = KeyCode.LeftArrow;
    private const KeyCode Right = KeyCode.RightArrow;
    private const KeyCode Enter = KeyCode.Return;
    private const KeyCode Space = KeyCode.Space;
    private const KeyCode ESC = KeyCode.Escape;
    //* public --------------------------------------//
    //* private --------------------------------------//
    private bool isSelectable = true;
    private bool isEventAvailable = false;
    private int ItemIndex = 0;
    private IEnumerator shiftCoroutine;
    //* SerializeField --------------------------------------//
    [SerializeField] private Animator CursorAnimator;
    [SerializeField] private Animator AlertAnimator;
    [SerializeField] private TextMeshPro TmpAlertMessage;
    private void Update()
    {
        if (!isSelectable) return;
        if (Input.GetKeyDown(Enter) || Input.GetKeyDown(Space)) { RunItem(ItemIndex); }
        if (Input.GetKeyDown(Up)) { MoveItem(isUp: true); }
        if (Input.GetKeyDown(Down)) { MoveItem(isDown: true); }
        if (Input.GetKeyDown(Left)) { MoveItem(isLeft: true); }
        if (Input.GetKeyDown(Right)) { MoveItem(isRight: true); }
        if (Input.GetKeyDown(ESC)) { /*게임 종료 여부 확인 메시지*/ }
    }
    public void MouseOver(int index)
    {
        ItemIndex = index;
    }
    private void MoveItem(bool isUp = false, bool isDown = false, bool isLeft = false, bool isRight = false)
    {
        if (isUp)
        {
            if (ItemIndex == 0) { ItemIndex = 6; }
            else ItemIndex--;
        }
        if (isDown) 
        { 
            if (ItemIndex == 6) { ItemIndex = 0; } 
            else ItemIndex++;
        }
        if (isLeft) 
        {
            return;
        }
        if (isRight) 
        { 
            return;
        }
        CursorAnimator.SetTrigger("Play");
        CursorAnimator.gameObject.transform.localPosition 
            = new Vector3(0.0f, -1.0f * ItemIndex, 0.0f);
    }
    private void RunItem(int index)
    {
        if (index >= MaxItemCount) return;
        switch(index)
        {
            // World Mode
            case 0:
                StartCoroutine(ISendAlert("This Content is UnAvailable."));
                break;
            // Normal Mode
            case 1:
                MainSystem.LoadSelectScene();
                isSelectable = false;
                break;
            // Event
            case 2:
                if (!isEventAvailable) { StartCoroutine(ISendAlert("There is no Event now.")); }
                break;
            // Memorial
            case 3:
                StartCoroutine(ISendAlert("This Content is UnAvailable."));
                break;
            // Shop
            case 4:
                StartCoroutine(ISendAlert("This Content is UnAvailable."));
                break;
            // Setting
            case 5:
                StartCoroutine(ISendAlert("This Content is UnAvailable."));
                break;
            // More
            case 6:
                StartCoroutine(ISendAlert("This Content is UnAvailable."));
                break;
        }
    }
    private IEnumerator ISendAlert(string message)
    {
        isSelectable = false;
        TmpAlertMessage.text = message;
        AlertAnimator.SetTrigger(SendAlertTrigger);
        yield return new WaitForSeconds(0.2f);
        while(!isSelectable)
        {
            if (Input.anyKeyDown) { isSelectable = true; }
            yield return null;
        }
        AlertAnimator.SetTrigger(EndAlertTrigger);
    }
}
