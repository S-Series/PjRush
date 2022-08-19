using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    private static List<Animator> animators = new List<Animator>();
    private static string[] trigger = {"Start", "End"};
    [SerializeField] GameObject[] animatorObject;
    private void Awake()
    {
        for (int i = 0; i < animatorObject.Length; i++)
            { animators.Add(animatorObject[i].GetComponent<Animator>()); }
    }
    public static void PlayAnimation(int index, bool isStart)
    {
        if (isStart) animators[index].SetTrigger(trigger[0]);
        else animators[index].SetTrigger(trigger[1]);
    }
}
