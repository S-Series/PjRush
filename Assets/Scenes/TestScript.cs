using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    private void Awake() 
    {
        StartCoroutine(RunCoroutine());
    }
    private void Start()
    {
        print("Start");
    }
    void Update()
    {

    }
    IEnumerator RunCoroutine()
    {
        yield return A();
        yield return B();
        yield return C();
        print("Coroutine End");
    }
    IEnumerator A(){
        print("A");
        yield return new WaitForSeconds(1.0f);
    }
    IEnumerator B(){
        print("B");
        yield return new WaitForSeconds(1.0f);
    }
    IEnumerator C(){
        print("C");
        yield return new WaitForSeconds(1.0f);
    }
}
