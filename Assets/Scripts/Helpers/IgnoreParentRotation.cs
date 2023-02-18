using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreParentRotation : MonoBehaviour
{
    private Quaternion _rotation;

    void Awake()
    {
        _rotation = transform.rotation;
    }

    void LateUpdate()
    {
        transform.rotation = _rotation;
    }
}
