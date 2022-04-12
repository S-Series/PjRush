using UnityEngine;
using System.Collections;


public class ShowFps : MonoBehaviour
{
    float deltaTime = 0.0f;

    GUIStyle style;
    Rect rect;
    float msec;
    float fps;
    string text;

    void Awake()
    {
        Application.targetFrameRate = 240;

        int w = Screen.width, h = Screen.height;

        rect = new Rect(0, 0, w, h);

        style = new GUIStyle();
        style.alignment = TextAnchor.LowerRight;
        style.fontSize = h * 2 / 100;
        style.normal.textColor = Color.white;
    }

    void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
    }

    void OnGUI(){

        msec = deltaTime * 1000.0f;
        fps = 1.0f / deltaTime;

        text = fps.ToString("F1") + "fps (" + msec.ToString("F1") + "ms" + ") ";
        GUI.Label(rect, text, style);
    }
}
