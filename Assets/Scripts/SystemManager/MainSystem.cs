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
    private void Start()
    {
        main = this;
        DontDestroyOnLoad(this);
        StartCoroutine(BootingProgram());
    }

    private IEnumerator BootingProgram()
    {
        //AnimatorManager.Booting.SetTrigger(AnimatorManager.TriggerBooting[0]);
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
        yield return ILoadMainScene();
    }
    //** Scene Loader
    public static void LoadMainScene()
    {
        main.StartCoroutine(main.ILoadMainScene());
    }
    private IEnumerator ILoadMainScene()
    {
        yield return new WaitForSeconds(2.0f);
        AnimatorManager.AnimatorSceneChange.SetTrigger(AnimatorManager.TriggerSceneChange[0]);
        SceneManager.LoadScene(MainScene);
        yield return new WaitForSeconds(1.0f);
        AnimatorManager.AnimatorSceneChange.SetTrigger(AnimatorManager.TriggerSceneChange[1]);
    }
    public static void LoadSelectScene()
    {
        main.StartCoroutine(main.ILoadSelectScene());
    }
    private IEnumerator ILoadSelectScene()
    {
        yield return new WaitForSeconds(2.0f);
        AnimatorManager.AnimatorSceneChange.SetTrigger(AnimatorManager.TriggerSceneChange[0]);
        SceneManager.LoadScene(MusicSelectScene);
        yield return new WaitForSeconds(1.0f);
        MusicSelectAct.LoadSelectMusic();
        yield return new WaitForSeconds(1.0f);
        AnimatorManager.AnimatorSceneChange.SetTrigger(AnimatorManager.TriggerSceneChange[1]);
        yield return new WaitForSeconds(1.0f);
        MusicSelectAct.isSelectable = true;
    }
}
