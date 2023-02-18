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
    
    void OnValidate()
    {
        UITextValue = GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        soFloat.OnValueChanged += UpdateText;
        UpdateText(soFloat.Value);
    }

    protected virtual void UpdateText(float i)
    {
        UITextValue.text = prefixText + soFloat.Value.ToString("n1") + sufixText;
    }

}
