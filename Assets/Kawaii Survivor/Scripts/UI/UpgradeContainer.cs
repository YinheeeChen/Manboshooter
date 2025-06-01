using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeContainer : MonoBehaviour
{

    [Header("Elements")]
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI upgradeNmaeText;
    [SerializeField] private TextMeshProUGUI upgradeVlaueeText;

    [field: SerializeField] public Button Button { get; private set; }

    public void Configure(Sprite icon, string upgradeName, string upgradeValue)
    {
        image.sprite = icon;
        upgradeNmaeText.text = upgradeName;
        upgradeVlaueeText.text = upgradeValue;
    }
}
