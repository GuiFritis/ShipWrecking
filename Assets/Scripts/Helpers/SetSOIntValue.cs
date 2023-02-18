using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSOIntValue : MonoBehaviour
{
    public SOInt soInt;

    public void SetValue(int i)
    {
        soInt.Value = i;
    }

    public void AddValue(int i)
    {
        soInt.Value += i;
    }

    public void SetValue(float i)
    {
        soInt.Value = (int)i;
    }

    public void AddValue(float i)
    {
        soInt.Value += (int)i;
    }
}
