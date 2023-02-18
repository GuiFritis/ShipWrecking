using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class SOFloatSliderUpdate : MonoBehaviour
{
    public SOFloat soFloat;
    public Slider slider;

    void OnValidate()
    {
        slider = GetComponent<Slider>();
    }

    void Start()
    {
        UpdateValue();
    }

    private void UpdateValue()
    {
        slider.value = soFloat.Value;
    }
}
