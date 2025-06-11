using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemContainer : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Image itemIcon;
    [SerializeField] private Image container;
    [SerializeField] private Button button;

    public int Index { get; private set; }

    public Weapon Weapon { get; private set; }
    public ObjectDataSO ObjectData { get; private set; }

    public void Configure(Color color, Sprite icon)
    {
        container.color = color;
        itemIcon.sprite = icon;
    }

    public void Configure(Weapon weapon, int index, Action clickedCallBack)
    {
        Weapon = weapon;
        Index = index;

        Color color = ColorHolder.GetColor(weapon.Level);
        Sprite icon = weapon.WeaponData.WeaponIcon;
        Configure(color, icon);

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => clickedCallBack?.Invoke());
    }

    public void Configure(ObjectDataSO objectData, Action clickedCallBack)
    {
        ObjectData = objectData;

        Color color = ColorHolder.GetColor(objectData.Rarity);
        Sprite icon = objectData.Icon;

        Configure(color, icon);
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => clickedCallBack?.Invoke());
    }

}
