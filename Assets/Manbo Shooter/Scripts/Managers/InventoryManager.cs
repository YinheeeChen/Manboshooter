using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryManager : MonoBehaviour, IGameStateListener
{
    [Header("Player Components")]
    [SerializeField] private PlayerWeapons playerWeapons;
    [SerializeField] private PlayerObjects playerObjects;

    [Header("Elements")]
    [SerializeField] private Transform inventoryItemsParent;
    [SerializeField] private Transform pauseInventoryItemsParent;
    [SerializeField] private InventoryItemContainer inventoryItemContainer;
    [SerializeField] private ShopManagerUI shopManagerUI;
    [SerializeField] private InventoryItemInfo inventoryItemInfo;

    private void Awake()
    {
        ShopManager.onItemPurchased += ItemPurchasedCallback;
        WeaponMerger.onMerge += WeaponMergedallback;

        GameManager.onGamePaused += Configure;
    }

    private void OnDestroy()
    {
        ShopManager.onItemPurchased -= ItemPurchasedCallback;
        WeaponMerger.onMerge -= WeaponMergedallback;

        GameManager.onGamePaused -= Configure;
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GmaeStateChangeCallback(GameState gameState)
    {
        if (gameState == GameState.SHOP)
            Configure();
    }

    private void Configure()
    {
        inventoryItemsParent.Clear();
        pauseInventoryItemsParent.Clear();

        Weapon[] weapons = playerWeapons.GetWeapons();

        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i] == null)
                continue;

            InventoryItemContainer itemContainer = Instantiate(inventoryItemContainer, inventoryItemsParent);
            itemContainer.Configure(weapons[i], i, () => ShowItemInfo(itemContainer));

            InventoryItemContainer pauseItemContainer = Instantiate(inventoryItemContainer, pauseInventoryItemsParent);
            pauseItemContainer.Configure(weapons[i], i, null);
        }

        ObjectDataSO[] objectDatas = playerObjects.Objects.ToArray();

        for (int i = 0; i < objectDatas.Length; i++)
        {
            InventoryItemContainer itemContainer = Instantiate(inventoryItemContainer, inventoryItemsParent);
            itemContainer.Configure(objectDatas[i], () => ShowItemInfo(itemContainer));

            InventoryItemContainer pauseItemContainer = Instantiate(inventoryItemContainer, pauseInventoryItemsParent);
            pauseItemContainer.Configure(objectDatas[i], null);
        }
    }

    private void ShowItemInfo(InventoryItemContainer container)
    {
        if (container.Weapon != null)
            ShowWeaponInfo(container.Weapon, container.Index);
        else if (container.ObjectData != null)
            ShowObjectInfo(container.ObjectData);
    }

    private void ShowWeaponInfo(Weapon weapon, int index)
    {
        inventoryItemInfo.Configure(weapon);

        inventoryItemInfo.RecycleButton.onClick.RemoveAllListeners();
        inventoryItemInfo.RecycleButton.onClick.AddListener(() => RecycleWeapon(index));
        shopManagerUI.ShowItemInfo();
    }

    private void ShowObjectInfo(ObjectDataSO objectData)
    {
        inventoryItemInfo.Configure(objectData);

        inventoryItemInfo.RecycleButton.onClick.RemoveAllListeners();
        inventoryItemInfo.RecycleButton.onClick.AddListener(() => RecycleObject(objectData));
        shopManagerUI.ShowItemInfo();
    }

    private void RecycleObject(ObjectDataSO objectData)
    {
        playerObjects.RecycleObject(objectData);
        Configure();
        shopManagerUI.HideItemInfo();
    }

    private void RecycleWeapon(int index)
    {
        playerWeapons.RecycleWeapon(index);
        Configure();
        shopManagerUI.HideItemInfo();
    }

    private void ItemPurchasedCallback() => Configure();
    
    private void WeaponMergedallback(Weapon weapon)
    {
        Configure();
        inventoryItemInfo.Configure(weapon);
    }

}
