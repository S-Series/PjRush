using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ComboManager : MonoBehaviour
{
    public static ComboManager comboManager;

    public int combo;
    public int maxCombo;

    private int[] comboPart = new int[4];

    private string comboText;
    private int comboTextLength;

    private int textColor;

    [SerializeField]
    TextMeshPro[] comboTMP;

    private void Awake()
    {
        comboManager = this;
    }

    void Start()
    {
        combo = 0;
        textColor = 1;
    }

    void Update()
    {
        comboText = combo.ToString();
        comboTextLength = comboText.Length;

        for (int i = 0; i < comboTextLength; i++)
        {
            comboPart[3 - i] = int.Parse(comboText.Substring(comboTextLength - 1 - i , 1));
        }

        for (int i = 0; i < 4; i++)
        {
            comboTMP[i].text = comboPart[i].ToString();
        }

        if (maxCombo <= combo) maxCombo = combo;

        setColor();
    }

    private void setColor()
    {
        switch (textColor)
        {
            case 1:
                if (combo >= 1)
                {
                    comboTMP[3].color = new Color32(200, 200, 200, 255);
                    textColor++;
                }
                break;
            case 2:
                if (combo >= 10)
                {
                    comboTMP[2].color = new Color32(200, 200, 200, 255);
                    textColor++;
                }
                break;
            case 3:
                if (combo >= 100)
                {
                    comboTMP[1].color = new Color32(200, 200, 200, 255);
                    textColor++;
                }
                break;
            case 4:
                if (combo >= 1000)
                {
                    comboTMP[0].color = new Color32(200, 200, 200, 255);    
                    textColor++;
                }
                break;
            default:
                textColor = 1;
                break;
        }
    }

    public void resetCombo()
    {
        combo = 0;

        for(int i = 0; i < 4; i++)
        {
            comboPart[i] = 0;
        }

        textColor = 1;
        comboTMP[0].color = new Color32(255, 255, 255, 100);
        comboTMP[1].color = new Color32(255, 255, 255, 100);
        comboTMP[2].color = new Color32(255, 255, 255, 100);
        comboTMP[3].color = new Color32(255, 255, 255, 100);
    }
}
