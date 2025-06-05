using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItemContainer : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI priceText;

    [Header("Stats")]
    [SerializeField] private Transform statContainersParent;
    [field: SerializeField] public Button PurchaseButton { get; private set; }

    [Header("Color")]
    [SerializeField] private Image[] levelImages;
    [SerializeField] private Image outline;

    [Header("Lock Elements")]
    [SerializeField] private Image lockImage;
    [SerializeField] private Sprite lockedSprite, unlockedSprite;
    public bool IsLocked
    { get; private set; }

    public void Configure(WeaponDataSO weaponData, int level)
    {

        icon.sprite = weaponData.WeaponIcon;
        nameText.text = weaponData.WeaponName + $" (lvl {level + 1})";
        priceText.text = WeaponStatsCalculator.GetPurchasePrice(weaponData, level).ToString();

        Color imageColor = ColorHolder.GetColor(level);
        nameText.color = imageColor;

        outline.color = ColorHolder.GetOutlineColor(level);

        foreach (Image image in levelImages)
            image.color = imageColor;

        Dictionary<Stat, float> calculatedStats = WeaponStatsCalculator.GetStats(weaponData, level);
        ConfigureStatContainers(calculatedStats);
    }

    public void Configure(ObjectDataSO objectData)
    {
        icon.sprite = objectData.Icon;
        nameText.text = objectData.Name;
        priceText.text = objectData.Price.ToString();

        Color imageColor = ColorHolder.GetColor(objectData.Rarity);
        nameText.color = imageColor;

        outline.color = ColorHolder.GetOutlineColor(objectData.Rarity);

        foreach (Image image in levelImages)
            image.color = imageColor;

        ConfigureStatContainers(objectData.BaseStats);
    }

    private void ConfigureStatContainers(Dictionary<Stat, float> stats)
    {
        statContainersParent.Clear();
        StatContainerManager.GenerateStatContainers(stats, statContainersParent);
    }

    public void LockButtonCallback()
    {
        IsLocked = !IsLocked;
        UpdateLockVisuals();
    }

    private void UpdateLockVisuals()
    {
        lockImage.sprite = IsLocked ? lockedSprite : unlockedSprite;
    }
}
