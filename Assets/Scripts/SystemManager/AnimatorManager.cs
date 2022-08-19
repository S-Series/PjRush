using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    private static string[] trigger = {"Start", "End"};
    [SerializeField] private GameObject[] animatorObject;
    private static List<Animator> animators = new List<Animator>();
    [SerializeField] private GameObject _spriteRenderer; 
    private static SpriteRenderer JacketRenderer; 
    private void Awake()
    {
        for (int i = 0; i < animatorObject.Length; i++)
            { animators.Add(animatorObject[i].GetComponent<Animator>()); }
        JacketRenderer = _spriteRenderer.GetComponent<SpriteRenderer>();
    }
    public static void PlayAnimation(int index, bool isStart)
    {
        if (isStart) animators[index].SetTrigger(trigger[0]);
        else animators[index].SetTrigger(trigger[1]);
    }
    public static void ChangeJacket(Sprite _sprite)
    {
        JacketRenderer.sprite = _sprite;
    }
}
