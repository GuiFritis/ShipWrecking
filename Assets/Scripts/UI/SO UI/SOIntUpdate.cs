using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SOIntUpdate : MonoBehaviour
{
    public SOInt soInt;
    public string prefixText;
    public string sufixText;
    public TextMeshProUGUI UITextValue;

    void OnValidate()
    {
        UITextValue = GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        soInt.OnValueChanged += UpdateText;
        UITextValue.text = prefixText + soInt.Value.ToString() + sufixText;
    }

    void UpdateText(int i)
    {
        UITextValue.text = prefixText + soInt.Value.ToString() + sufixText;
    }

}
