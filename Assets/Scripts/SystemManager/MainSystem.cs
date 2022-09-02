using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using TMPro;

public class MainSystem : MonoBehaviour
{
    private static MainSystem main;
    private const string MainScene = "Main";
    private const string MusicSelectScene = "Select";
    private const string GameScene = "GameField";
    private const string ResultScene = "Result";
    private void Start()
    {
        main = this;
        DontDestroyOnLoad(this);
        StartCoroutine(BootingProgram());
    }

    private IEnumerator BootingProgram()
    {
        SystemManager.SetSystemText("Loading Player Data...");
        //yield return SystemManager.ILoadUserData();
        SystemManager.SetSystemText("Loading Music...");
        yield return MusicManager.ILoadMusic();
        SystemManager.SetSystemText("Checking Online...");
        //yield return OnlineManager.IConnectOnline();
        SystemManager.SetSystemText("Connecting to DataBase...");
        //yield return OnlineManager.IConnectToDB();
        //AnimatorManager.Booting.SetTrigger(AnimatorManager.TriggerBooting[1]);
        SystemManager.SetSystemText("");
        yield return new WaitForSeconds(1.5f);
        yield return ILoadMainScene();
    }
    //** Scene Loader
    public static void LoadMainScene() { main.StartCoroutine(main.ILoadMainScene()); }
    public static void LoadSelectScene() { main.StartCoroutine(main.ILoadSelectScene()); }
    public static void LoadGameScene() { main.StartCoroutine(main.ILoadGameScene()); }
    public static void LoadResultScene() { main.StartCoroutine(main.ILoadResultScene()); }

    private IEnumerator ILoadMainScene()
    {
        AnimatorManager.PlayAnimation(3, true);
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene(MainScene);
        yield return null;
        transform.position = GameObject.FindWithTag("systemPos").transform.position;
        transform.eulerAngles = GameObject.FindWithTag("systemPos").transform.eulerAngles;
        yield return new WaitForSeconds(1.0f);
        AnimatorManager.PlayAnimation(3, false);
    }
    private IEnumerator ILoadSelectScene()
    {
        AnimatorManager.PlayAnimation(3, true);
        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene(MusicSelectScene);
        yield return null;
        transform.position = GameObject.FindWithTag("systemPos").transform.position;
        transform.eulerAngles = GameObject.FindWithTag("systemPos").transform.eulerAngles;
        yield return new WaitForSeconds(1.0f);
        while(true)
        {
            try
            {
                MusicSelectAct.LoadSelectMusic();
                break;
            }
            catch { ; }
            yield return null;
        }
        yield return new WaitForSeconds(1.0f);
        AnimatorManager.PlayAnimation(3, false);
        yield return new WaitForSeconds(1.5f);
        MusicSelectAct.isSelectable = true;
    }
    private IEnumerator ILoadGameScene()
    {
        AnimatorManager.PlayAnimation(2, true);
        yield return new WaitForSeconds(5.0f);
        SceneManager.LoadScene(GameScene);
        yield return null;
        transform.position = GameObject.FindWithTag("systemPos").transform.position;
        transform.eulerAngles = GameObject.FindWithTag("systemPos").transform.eulerAngles;
        yield return new WaitForSeconds(0.25f);
        while(true)
        {
            try { NoteData.getNoteData(); break; }
            catch { }
            NoteData.getNoteData();
            yield return null;
        }
        yield return new WaitForSeconds(5.0f);
        AnimatorManager.PlayAnimation(2, false);
        yield return new WaitForSeconds(7.5f);
        StartCoroutine(GamePlaySystem.gamePlaySystem.IStartGame());
    }
    private IEnumerator ILoadResultScene()
    {
        AnimatorManager.PlayAnimation(3, true);
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene(ResultScene);
        yield return null;
        while(true)
        {
            try
            {
                GameResult.gameResult.DisplayResult();
                break;
            }
            catch { ; }
            yield return null;
        }
        yield return new WaitForSeconds(1.0f);
        AnimatorManager.PlayAnimation(3, false);
    }
}
