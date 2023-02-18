using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class SOIntSliderUpdate : MonoBehaviour
{
    public SOInt soInt;
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
        slider.value = soInt.Value;
    }
}
