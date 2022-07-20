using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using TMPro;

public class MainSystem : MonoBehaviour
{
    private void Start()
    {
        
    }

    private IEnumerator BootingProgram()
    {
        AnimatorManager.Booting.SetTrigger(AnimatorManager.TriggerBooting[0]);
        yield return SystemManager.ILoadUserData();
        yield return MusicManager.ILoadMusic();
        //yield return OnlineManager.IConnectToDB();
        AnimatorManager.Booting.SetTrigger(AnimatorManager.TriggerBooting[1]);
        gameProgramStart();
    }
    private void gameProgramStart()
    {

    }
}
