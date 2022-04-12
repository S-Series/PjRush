using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainSelect : MonoBehaviour
{
    bool isMain;
    int itemNum;
    /// <summary>
    /// index num   || Mode Name    || Trigger
    /// itemNum = 1 || Option       || "Option"
    /// itemNum = 2 || Quit         || "Quit"
    /// itemNum = 3 || Normal Mode  || "Normal"
    /// itemNum = 4 || Arcade Mode  || "Arcade"
    /// itemNum = 5 || Shop         || "Shop"
    /// </summary>

    [SerializeField]
    GameObject MovingPart;

    [SerializeField]
    Animator GameStartAnimate;

    private void Awake()
    {
        isMain = true;
        itemNum = 3;
    }

    private void Start()
    {
        isMain = true;
        itemNum = 3;
    }

    private void Update()
    {
        if (isMain)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                // 게임종료 선택창 활성화 처리
            }
            if (Input.GetKeyDown(KeyCode.Return) 
                || Input.GetKeyDown(KeyCode.Space))
            {
                // 메인화면 -> 선택화면
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                // 선택화면 -> 메인화면
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                itemNum--;
                checkItemIndex();
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                itemNum++;
                checkItemIndex();
            }
            if (Input.GetKeyDown(KeyCode.Return)
                || Input.GetKeyDown(KeyCode.Space))
            {
                switch (itemNum)
                {
                    case 1:

                        break;

                    case 2:

                        break;

                    case 3:
                        StartCoroutine(IcheckOnlice("Normal", "NormalSelect"));
                        break;

                    case 4:

                        break;

                    case 5:

                        break;
                }
            }
        }


    }

    private void checkItemIndex()
    {
        if (itemNum <= 0) itemNum = 5;
        else if (itemNum >= 6) itemNum = 1;
    }

    private IEnumerator IcheckOnlice(string trigger, string Scene)
    {
        bool b = false;
        yield return b = CheckOnline();

        if (b)
        {
            isMain = false; 
            MainSystem.isUserOnline = b;
            GameStartAnimate.SetTrigger(trigger);
            yield return new WaitForSeconds(5.0f);
            SceneManager.LoadScene(Scene);
        }
        else
        {
            // 재시도 할건지 묻는 메시지 출력
        }
    }

    private bool CheckOnline()
    {
        if (Application.internetReachability 
            == NetworkReachability.NotReachable) 
            return false;
        else 
            return true;
    }
}
