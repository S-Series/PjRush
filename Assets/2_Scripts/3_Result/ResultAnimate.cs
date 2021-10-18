using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ResultAnimate : MonoBehaviour
{
    [SerializeField] Animator TopAnimatorTrigger;
    [SerializeField] Animator BottomAnimatorTrigger;

    bool InputAble;

    private void Start()
    {
        InputAble = false;
        //MainSystem.mainSystem.SceneAnimate.SetTrigger("ChangeOut");
        StartCoroutine(AnimateStart());
    }

    private void Update()
    {
        if (InputAble == true)
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Escape))
            {
                StartCoroutine(loadScene());
            }
        }
    }

    IEnumerator loadScene()
    {
        // 화면전환 애니메이션 시작

        yield return new WaitForSeconds(2.0f);

        SceneManager.LoadScene("SongSelect_Normal");
    }

    IEnumerator AnimateStart()
    {
        yield return new WaitForSeconds(2.0f);

        BottomAnimatorTrigger.SetTrigger("play");

        yield return new WaitForSeconds(1.0f);

        TopAnimatorTrigger.SetTrigger("play");

        yield return new WaitForSeconds(2.0f);

        InputAble = true;
    }
}
