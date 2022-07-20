using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    public static Animator Booting;
    public static readonly string[] TriggerBooting = {"", ""};

    public static Animator AnimatorSceneChange;
    public static readonly string[] TriggerSceneChange = {"",""};

    public static Animator AnimatorLoadMusic;
    public static readonly string[] TriggerLoadMusic = {"",""};

    [SerializeField] GameObject[] animatorObject;
    private void Awake()
    {
        Booting = animatorObject[0].GetComponent<Animator>();
        AnimatorSceneChange = animatorObject[1].GetComponent<Animator>();
        AnimatorLoadMusic = animatorObject[2].GetComponent<Animator>();
    }
}
