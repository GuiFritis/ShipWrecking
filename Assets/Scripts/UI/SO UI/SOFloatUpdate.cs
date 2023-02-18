using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SOFloatUpdate : MonoBehaviour
{
    public SOFloat soFloat;
    public string prefixText;
    public string sufixText;
    public TextMeshProUGUI UITextValue;

    void Start()
    {
        soFloat.OnValueChanged += UpdateText;
        UITextValue.text = prefixText + soFloat.Value.ToString() + sufixText;
    }

    void UpdateText(float i)
    {
        UITextValue.text = prefixText + soFloat.Value.ToString("n1") + sufixText;
    }

}
