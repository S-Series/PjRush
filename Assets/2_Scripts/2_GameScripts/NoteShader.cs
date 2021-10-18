using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteShader : MonoBehaviour
{
    public string orderName;
    public int order;

    [SerializeField]
    private MeshRenderer rend;

    private void Awake()
    {
        orderName = "NoteField";
        order = 1;
        rend = GetComponent<MeshRenderer>();
        rend.sortingLayerName = orderName;
        rend.sortingOrder = order;
    }

    private void Update()
    {
        if (rend.sortingLayerName != orderName)
        {
            rend.sortingLayerName = orderName;
        }
        if (rend.sortingOrder != order)
        {
            rend.sortingOrder = order;
        }
    }

    private void OnValidate()
    {
        rend = GetComponent<MeshRenderer>();
        rend.sortingLayerName = orderName;
        rend.sortingOrder = order;
    }
}
