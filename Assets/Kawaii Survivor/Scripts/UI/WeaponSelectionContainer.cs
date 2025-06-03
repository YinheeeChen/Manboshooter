using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponSelectionContainer : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Image weaponImage;
    [SerializeField] private TextMeshProUGUI weaponNameText;

    [Header("Stats")]
    [SerializeField] private Transform statContainersParent;
    [field: SerializeField] public Button Button { get; private set; }

    [Header("Color")]
    [SerializeField] private Image[] levelImages;

    public void Configure(Sprite weaponSprite, string weaponName, int level, WeaponDataSO weaponData)
    {
        if (weaponImage != null)
            weaponImage.sprite = weaponSprite;

        if (weaponNameText != null)
            weaponNameText.text = weaponName + $" (lvl {level + 1})";

        Color imageColor = ColorHolder.GetColor(level);
        weaponNameText.color = imageColor;

        foreach (Image image in levelImages)
            image.color = imageColor;
        
        Dictionary<Stat, float> calculatedStats = WeaponStatsCalculator.GetStats(weaponData, level);
        ConfigureStatContainers(calculatedStats);
    }

    private void ConfigureStatContainers(Dictionary<Stat, float> calculatedStats)
    {
        StatContainerManager.GenerateStatContainers(calculatedStats, statContainersParent);
    }

    public void Select()
    {
        LeanTween.cancel(gameObject);
        LeanTween.scale(gameObject, Vector3.one * 1.2f, 0.3f).setEase(LeanTweenType.easeInOutSine);
    }

    public void Deselect()
    {
        LeanTween.cancel(gameObject);
        LeanTween.scale(gameObject, Vector3.one, 0.3f).setEase(LeanTweenType.easeInOutSine);
    }
}
