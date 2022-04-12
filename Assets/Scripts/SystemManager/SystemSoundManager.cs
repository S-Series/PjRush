using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemSoundManager : MonoBehaviour
{
    private static SystemSoundManager soundManager;
    public static AudioSource systemAudio;
    [SerializeField] private AudioClip[] systemAudioClipArr;
    #region AudioClipSummary
    // [0] || open
    // [1] || close
    // [2] || tick
    // [3] || click
    // [4] || pop
    #endregion
    private void Awake() {
        soundManager = this;
        systemAudio = GetComponent<AudioSource>();
        systemAudio.playOnAwake = false;
        systemAudio.loop = false;
    }
    public static void playSystemAudio(int index){
        systemAudio.Stop();
        systemAudio.clip = soundManager.systemAudioClipArr[index];
        systemAudio.Play();
    }
}
