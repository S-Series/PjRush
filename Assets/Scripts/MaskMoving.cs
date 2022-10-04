using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskMoving : MonoBehaviour
{
    private static MaskMoving mask;
    public static float s_MaskMoveFloat;
    public static bool[] s_MaskBlind 
        = new bool[6] { false, false, false, false, false, false };
    public static bool[] s_AlreadyUnblind 
        = new bool[6] { false, false, false, false, false, false };
    private static IEnumerator[] s_MaskCoroutine = new IEnumerator[6];
    [SerializeField] private GameObject[] MaskObject;

    private void Awake() 
    { 
        mask = this; 
        for (int i = 0; i < 6; i++) { s_MaskCoroutine[i] = IMoveMask(MaskObject[i]); }
    }
    public static void ChangeBlind(int _line, bool _isBlind)
    {
        if (_isBlind) 
        {
            mask.StopCoroutine(s_MaskCoroutine[_line - 1]);
            mask.MaskObject[_line - 1].transform.localPosition 
                = new Vector3(getPosX(_line), 0, 0); 
        }
        else if (!s_AlreadyUnblind[_line - 1])
        { 
            mask.StartCoroutine(s_MaskCoroutine[_line - 1]);
        }
    }
    private static float getPosX(int _line)
    {
        switch (_line)
        {
            case 1: return -3.0f;
            case 2: return -1.0f;
            case 3: return +1.0f;
            case 4: return +3.0f;
            case 5: return -2.0f;
            case 6: return +2.0f;
            
            default: 
                throw new System.Exception("None Available");
        }
    }
    private IEnumerator IMoveMask(GameObject _object)
    {
        Vector3 _pos;
        int _lastMs = 0;
        while(true)
        {
            _pos = _object.transform.localPosition;
            _pos.y -= (GamePlaySystem.s_gameMs - _lastMs) * GamePlaySystem.gameBpm / 15000;
            _lastMs = GamePlaySystem.s_gameMs;
            _object.transform.localPosition = _pos;
            yield return null;
        }
    }
}
