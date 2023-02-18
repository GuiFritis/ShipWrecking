using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu]
public class SOFloat : ScriptableObject
{
    public Action<float> OnValueChanged;

    public float Value
    {
        get{return _value;}
        set
        {
            _value = value;
            OnValueChanged?.Invoke(_value);
        }
    }

    [SerializeField]
    private float _value;
}
