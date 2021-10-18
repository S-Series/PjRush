using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultInput : MonoBehaviour
{
    public static ResultInput resultInput;

    public bool isResultInputActive;

    private void Awake()
    {
        resultInput = this;
    }

    void Start()
    {
        isResultInputActive = false;
    }

    void Update()
    {
        if (isResultInputActive == true)
        {

        }
    }

    IEnumerator SceneLoad(string title)
    {
        yield return new WaitForSeconds(1.0f);

        SceneManager.LoadScene(title);
    }
}
