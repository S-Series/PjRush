using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    Thread myThread;
    private void Start()
    {
        myThread = new Thread(slowJob);
        myThread.Start();
    }
    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void Update()
    {
        if (myThread.IsAlive) { print("Alive"); }
    }
    private void slowJob()
    {
        for (int i = 0; i < 10; i++)
        {
            Debug.Log("I'm running!");
            Thread.Sleep(1000);
        }
    }
}
