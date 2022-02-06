using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainSystem : MonoBehaviour
{
    public static MainSystem mainSystem;

    private void Awake()
    {
        if (mainSystem != null) 
        { 
            Destroy(this); 
            return; 
        }
        mainSystem = this;
        DontDestroyOnLoad(this);
    }

    #region Manager System
    public GamePlayed gamePlayed;
    public MusicManager musicManager;
    public InputManager inputManager;
    #endregion

    [SerializeField]
    public Animator[] GameAnimator;

    [SerializeField]
    public SpriteRenderer[] SceneAnimateJacket;

    [SerializeField]
    public TextMeshPro[] SceneAnimateTMPro;

    [SerializeField]
    public Animator[] AudioFadeIn;

    [SerializeField]
    public Animator[] AudioFadeOut;

    [SerializeField]
    Sprite[] RankSprite;
    [SerializeField]
    Sprite[] RankSpritePlus;

    public int difficulty; // 0 ~ 15 계획
    public int difficultyNum; // 0 : Easy || 1 : Normal || 2 : Hard || 3 : Extra || 4 : Special

    public int gameEndMS;

    public static int UID = 100001;

    public bool isUserOnline;
    public bool isUserLogin;

    public string songName;

    public double bpm;
    public double highSpeed;
    public double gameSpeed;

    public int gameType;
    /// <summary>
    /// 0 || !Error (GameType Missed)
    /// 1 || NormalMode
    /// 2 || ArcadeMode - 
    /// 3 || ArcadeMode - 
    /// 4 || ArcadeMode - 
    /// </summary>

    public Sprite Rank(int index)
    {
        if (index == -1) return null;
        return RankSprite[index];
    }

    public Sprite RankPlus(int index)
    {
        if (index == -1) return null;
        return RankSpritePlus[index];
    }
}
