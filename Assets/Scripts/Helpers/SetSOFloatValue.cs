using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSOFloatValue : MonoBehaviour
{
    public SOFloat soFloat;

    public void SetValue(float i)
    {
        soFloat.Value = i;
    }

    public void AddValue(float i)
    {
        soFloat.Value += i;
    }
}
