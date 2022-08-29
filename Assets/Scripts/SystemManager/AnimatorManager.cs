using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AnimatorManager : MonoBehaviour
{
    private static AnimatorManager AM;
    private static string[] trigger = {"Start", "End"};
    [SerializeField] private Animator[] Animator;
    [SerializeField] private Sprite[] DifficultySprite;
    [SerializeField] private SpriteRenderer[] GameInfoRenderer;
    [SerializeField] private TextMeshPro[] GameInfoTMP;
    private void Awake() { AM = this; }
    public static void PlayAnimation(int index, bool isStart)
    {
        if (isStart) AM.Animator[index].SetTrigger(trigger[0]);
        else AM.Animator[index].SetTrigger(trigger[1]);
    }
    public static void ChangeJacket(Sprite _sprite, int _DifficultyIndex, Music.Status status = Music.Status.Null)
    {
        AM.GameInfoRenderer[0].sprite = _sprite;
        if (_DifficultyIndex == 4) { AM.GameInfoRenderer[1].sprite = SpriteManager.getDreamSprite(status); }
        else { AM.GameInfoRenderer[1].sprite = AM.DifficultySprite[_DifficultyIndex]; }
    }
    public static void ChangeMusicInfo(string[] MusicInfo)
    {
        //* index = 0   || MusicName
        //* index = 1   || MusicArtist
        //* index = 2   || Bpm
        //* index = 3   || Difficulty
        //* index = 4   || Difficulty Name
        //* index = 5   || NoteEffecter
        //* index = 6   || Jacket Illustrator
        for (int i = 0; i < 7; i++) { AM.GameInfoTMP[i].text = MusicInfo[i]; }
    }
}
