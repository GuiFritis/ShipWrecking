using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class SOFloatUpdateToTime : SOFloatUpdate
{
    protected override void UpdateText(float i)
    {
        TimeSpan time = TimeSpan.FromSeconds(i);
        UITextValue.text = prefixText + time.ToString("mm':'ss") + sufixText;
    }

}
