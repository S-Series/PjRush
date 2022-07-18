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
    //** System Managers    ------------------------ //
    public static GameManager gameManager;
    public static SystemManager systemManager;
    public static MusicManager musicManager;
    public static InputManager inputManager;
    public static SpriteManager spriteManager;
    public static SystemSoundManager soundManager;
    public static CharacterManager characterManager;

    //** SerializeField     ------------------------ //

    private void Awake()
    {
        gameManager = GetComponentInChildren<GameManager>();
        systemManager = GetComponentInChildren<SystemManager>();
        musicManager = GetComponentInChildren<MusicManager>();
        inputManager = GetComponentInChildren<InputManager>();
        spriteManager = GetComponentInChildren<SpriteManager>();
        soundManager = GetComponentInChildren<SystemSoundManager>();
        characterManager = GetComponentInChildren<CharacterManager>();
    }

}
