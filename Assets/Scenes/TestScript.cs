using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    [SerializeField]
    ParticleSystem test;

    private void Start()
    {
        test.Play();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {

        }

        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            test.Play();
        }
    }
}
