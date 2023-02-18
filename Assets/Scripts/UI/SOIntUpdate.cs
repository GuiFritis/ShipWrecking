using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SOIntUpdate : MonoBehaviour
{
    public SOInt soInt;
    public string prefixText;
    public TextMeshProUGUI UITextValue;

    void Start()
    {
        soInt.OnValueChanged += UpdateText;
        UITextValue.text = prefixText + soInt.Value.ToString();
    }

    void UpdateText(int i)
    {
        UITextValue.text = prefixText + soInt.Value.ToString();
    }

}