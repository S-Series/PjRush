using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorAudio : MonoBehaviour
{
    [SerializeField] private AudioSource[] audioSource;
    public void PlaySound(int _audioIndex)
    {
        try { audioSource[_audioIndex].Play();}
        catch { }
    }
}
