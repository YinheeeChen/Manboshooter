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
    [SerializeField] private Image outline;

    public void Configure(WeaponDataSO weaponData, int level)
    {
        if (weaponImage != null)
            weaponImage.sprite = weaponData.WeaponIcon;

        if (weaponNameText != null)
            weaponNameText.text = weaponData.WeaponName + $" (lvl {level + 1})";

        Color imageColor = ColorHolder.GetColor(level);
        weaponNameText.color = imageColor;

        outline.color = ColorHolder.GetOutlineColor(level);

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
