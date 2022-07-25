using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using TMPro;

public class MainSystem : MonoBehaviour
{
    private const string MainScene = "";
    private const string MusicSelectScene = "";
    private void Start()
    {
        DontDestroyOnLoad(this);
        StartCoroutine(BootingProgram());
    }

    private IEnumerator BootingProgram()
    {
        AnimatorManager.Booting.SetTrigger(AnimatorManager.TriggerBooting[0]);
        yield return SystemManager.ILoadUserData();
        yield return MusicManager.ILoadMusic();
        //yield return OnlineManager.IConnectToDB();
        //yield return OnlineManager.IConnectOnline();
        AnimatorManager.Booting.SetTrigger(AnimatorManager.TriggerBooting[1]);
        ILoadMainScene();
    }
    //** Scene Loader
    private IEnumerator ILoadMainScene()
    {
        yield return new WaitForSeconds(2.0f);
        AnimatorManager.AnimatorSceneChange.SetTrigger(AnimatorManager.TriggerSceneChange[0]);
        SceneManager.LoadScene(MainScene);
        while(true)
        {
            try { MusicSelectAct.LoadSelectMusic(); break; }
            catch { }
            yield return null;
        }
        yield return new WaitForSeconds(1.0f);
        AnimatorManager.AnimatorSceneChange.SetTrigger(AnimatorManager.TriggerSceneChange[1]);
    }
    private IEnumerator ILoadSelectScene()
    {
        yield return new WaitForSeconds(2.0f);
        AnimatorManager.AnimatorSceneChange.SetTrigger(AnimatorManager.TriggerSceneChange[0]);
        SceneManager.LoadScene(MusicSelectScene);
        while(true)
        {
            try { MusicSelectAct.LoadSelectMusic(); break; }
            catch { }
            yield return null;
        }
        yield return new WaitForSeconds(1.0f);
        AnimatorManager.AnimatorSceneChange.SetTrigger(AnimatorManager.TriggerSceneChange[1]);
    }
}
