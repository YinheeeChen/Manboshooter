using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class StatContainer : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Image statImage;
    [SerializeField] private TextMeshProUGUI statText;
    [SerializeField] private TextMeshProUGUI statValueText;

    public void Configure(Sprite icon, string statName, float statValue, bool useColor = false)
    {
        statImage.sprite = icon;
        statText.text = statName;

        if (useColor)
            ColorizeStatValueText(statValue);
        else
        {
            statValueText.color = Color.white;
            statValueText.text = statValue.ToString("F2");
        }
        
    }

    private void ColorizeStatValueText(float statValue)
    {
        float sign = Mathf.Sign(statValue);

        if (statValue == 0)
            sign = 0;

        float absStatValue = Mathf.Abs(statValue);

        Color statValueTextColor = Color.white;

        if (sign > 0)
            statValueTextColor = Color.green;
        else if (sign < 0)
            statValueTextColor = Color.red;

        statValueText.color = statValueTextColor;
        statValueText.text = absStatValue.ToString("F2"); 
    }

    public float GetFontSize()
    {
        return statText.fontSize;
    }

    public void SetFontSize(float fontSize)
    {
        statText.fontSizeMax = fontSize;
        statValueText.fontSizeMax = fontSize;
    }
}
