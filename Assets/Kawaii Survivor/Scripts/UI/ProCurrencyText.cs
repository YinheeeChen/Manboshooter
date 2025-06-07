using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class ProCurrencyText : MonoBehaviour
{
    [Header("Elements")]
    private TextMeshProUGUI text;

    public void UpdateText(string currencyString)
    {
        if (text == null)
            text = GetComponent<TextMeshProUGUI>();

        text.text = currencyString;
    }
}
