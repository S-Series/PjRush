using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keepPosition : MonoBehaviour
{
    void Update()
    {
        this.gameObject.transform.localPosition = new Vector3(0, 0, 10);
        this.gameObject.transform.localRotation = new Quaternion(0, 0, 0, 0);
    }
}
