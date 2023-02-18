using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFillUpdater : MonoBehaviour
{
    public Image uiImage;
    public float duration = .1f;

    private void OnValidate()
    {
        if(uiImage == null)
        {
            uiImage = gameObject.GetComponent<Image>();
        }
    }

    void Awake()
    {
        uiImage.type = Image.Type.Filled;
    }

    public void UpdateValue(float val)
    {
        uiImage.fillAmount = val;
    }

    public void UpdateValue(float max, float cur)
    {
        uiImage.fillAmount = 1 - (cur/max);
    }
}
