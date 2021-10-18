using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicBox : MonoBehaviour
{
    public static MusicBox musicBox;

    //게임 오브젝트의 이름을 SongName과 같게 할것
    [SerializeField]
    public List<GameObject> MusicInfoObjectList;

    GameObject MusicIndex;

    private void Awake()
    {
        if (musicBox == null)
        {
            musicBox = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        SetListReset();
    }

    void SetListReset()
    {
        MusicInfoObjectList = new List<GameObject>();
        MusicIndex = this.gameObject;

        if (MusicIndex == null)
        {
            Debug.LogError("시스템 오류가 발생했습니다.");
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
        else
        {
            for (int i = 0; i < MusicIndex.transform.childCount; i++)
            {
                MusicInfoObjectList.Add(MusicIndex.transform.GetChild(i).gameObject);
            }           
        }
        try
        {
            DisplayMusic.displayMusic.SortingList
                (DisplayMusic.displayMusic.sorting, MusicSelectAct.musicSelectAct.firstIndex, DisplayMusic.displayMusic.isReverse);
        }
        catch { }
    }
}
