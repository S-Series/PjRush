using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackMove : MonoBehaviour
{
    void Update()
    {
        this.gameObject.transform.localPosition += Time.deltaTime * new Vector3(21.175f, 12.0f, 0f) / 50;

        if (this.gameObject.transform.localPosition.x >= 21.175)
        {
            this.gameObject.transform.localPosition -= new Vector3(21.175f, 12.0f, 0f);
        }
    }
}
