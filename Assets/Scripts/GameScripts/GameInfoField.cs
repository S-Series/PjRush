using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameInfoField : MonoBehaviour
{
    public static int Combo;
    public static int ComboColorIndex;

    Color32 Normal = new Color32(255, 255, 120, 255);
    Color32 Transparent = new Color32(255, 255, 120, 180);

    [SerializeField]
    public TextMeshPro[] ComboTmp;

    public void AddCombo()
    {
        Combo++;
        if ( Combo >= Mathf.Pow(10, ComboColorIndex + 1))
        {
            ComboTmp[3 - ComboColorIndex].color = Normal;
            ComboColorIndex++;
        }

        for (int i = 0; i < 4; i++)
        {
            ComboTmp[i].text 
                = ((Combo - Combo % Mathf.Pow(10, 3 - i)) / Mathf.Pow(10, 3 - i)).ToString();
        }
    }

    public void ResetCombo()
    {
        Combo = 0;
        for (int i = 0; i < 4; i++)
        {
            ComboTmp[i].color = Transparent;
        }
    }
}
