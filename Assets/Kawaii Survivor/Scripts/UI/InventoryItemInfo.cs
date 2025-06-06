using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryItemInfo : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI recyclePriceText;

    [Header("Colors")]
    [SerializeField] private Image container;

    [Header("Stats")]
    [SerializeField] private Transform statsParent;

    [Header("Buttons")]
    [field: SerializeField] public Button RecycleButton{ get; set;}
    [SerializeField] private Button mergeButton;


    public void Configure(Weapon weapon)
    {
        Configure(
            weapon.WeaponData.WeaponIcon,
            weapon.WeaponData.WeaponName + " (lvl " + weapon.Level + ")",
            ColorHolder.GetColor(weapon.Level),
            WeaponStatsCalculator.GetRecyclePrice(weapon.WeaponData, weapon.Level),
            WeaponStatsCalculator.GetStats(weapon.WeaponData, weapon.Level)
        );

        mergeButton.gameObject.SetActive(true);
    }

    public void Configure(ObjectDataSO objectData)
    {
        Configure(
            objectData.Icon,
            objectData.Name,
            ColorHolder.GetColor(objectData.Rarity),
            objectData.RecyclePrice,
            objectData.BaseStats
        );

        mergeButton.gameObject.SetActive(false);
    }

    private void Configure(Sprite iconSprite, string itemName, Color color, int recyclePrice, Dictionary<Stat, float> stats)
    {
        icon.sprite = iconSprite;
        itemNameText.text = itemName;
        itemNameText.color = color;
        recyclePriceText.text = recyclePrice.ToString();
        container.color = color;

        StatContainerManager.GenerateStatContainers(stats, statsParent);
    }
}
