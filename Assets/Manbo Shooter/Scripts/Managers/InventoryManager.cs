using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Manages the player's inventory UI, including active and pause states.
/// Responds to weapon purchases, merges, and game state changes.
/// Implements IGameStateListener for reacting to game state transitions.
/// </summary>
public class InventoryManager : MonoBehaviour, IGameStateListener
{
    [Header("Player Components")]
    [SerializeField] private PlayerWeapons playerWeapons;             // Reference to player's weapons system
    [SerializeField] private PlayerObjects playerObjects;             // Reference to player's passive objects

    [Header("Elements")]
    [SerializeField] private Transform inventoryItemsParent;          // Parent for active inventory items UI
    [SerializeField] private Transform pauseInventoryItemsParent;     // Parent for pause screen inventory UI
    [SerializeField] private InventoryItemContainer inventoryItemContainer; // Inventory item UI prefab
    [SerializeField] private ShopManagerUI shopManagerUI;             // Reference to the shop UI
    [SerializeField] private InventoryItemInfo inventoryItemInfo;     // UI element for displaying item info/details

    /// <summary>
    /// Subscribes to relevant events for shop purchases, merges, and game pause.
    /// </summary>
    private void Awake()
    {
        ShopManager.onItemPurchased += ItemPurchasedCallback;
        WeaponMerger.onMerge += WeaponMergedallback;

        GameManager.onGamePaused += Configure;
    }

    /// <summary>
    /// Unsubscribes from all events to avoid memory leaks.
    /// </summary>
    private void OnDestroy()
    {
        ShopManager.onItemPurchased -= ItemPurchasedCallback;
        WeaponMerger.onMerge -= WeaponMergedallback;

        GameManager.onGamePaused -= Configure;
    }

    void Start()
    {
        // No startup logic needed
    }

    void Update()
    {
        // No frame update logic needed
    }

    /// <summary>
    /// Responds to global game state changes. Reconfigures inventory during SHOP phase.
    /// </summary>
    public void GmaeStateChangeCallback(GameState gameState)
    {
        if (gameState == GameState.SHOP)
            Configure();
    }

    /// <summary>
    /// Clears and repopulates the inventory UI with current weapons and objects.
    /// Called on pause, shop entry, weapon merge, and item purchase.
    /// </summary>
    private void Configure()
    {
        inventoryItemsParent.Clear();
        pauseInventoryItemsParent.Clear();

        // Populate with player's current weapons
        Weapon[] weapons = playerWeapons.GetWeapons();

        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i] == null)
                continue;

            // Active inventory UI
            InventoryItemContainer itemContainer = Instantiate(inventoryItemContainer, inventoryItemsParent);
            itemContainer.Configure(weapons[i], i, () => ShowItemInfo(itemContainer));

            // Pause inventory UI
            InventoryItemContainer pauseItemContainer = Instantiate(inventoryItemContainer, pauseInventoryItemsParent);
            pauseItemContainer.Configure(weapons[i], i, null);
        }

        // Populate with player's passive objects
        ObjectDataSO[] objectDatas = playerObjects.Objects.ToArray();

        for (int i = 0; i < objectDatas.Length; i++)
        {
            InventoryItemContainer itemContainer = Instantiate(inventoryItemContainer, inventoryItemsParent);
            itemContainer.Configure(objectDatas[i], () => ShowItemInfo(itemContainer));

            InventoryItemContainer pauseItemContainer = Instantiate(inventoryItemContainer, pauseInventoryItemsParent);
            pauseItemContainer.Configure(objectDatas[i], null);
        }
    }

    /// <summary>
    /// Displays item information in the UI, based on the type (weapon or object).
    /// </summary>
    private void ShowItemInfo(InventoryItemContainer container)
    {
        if (container.Weapon != null)
            ShowWeaponInfo(container.Weapon, container.Index);
        else if (container.ObjectData != null)
            ShowObjectInfo(container.ObjectData);
    }

    /// <summary>
    /// Shows detailed info for a weapon and sets up recycle button.
    /// </summary>
    private void ShowWeaponInfo(Weapon weapon, int index)
    {
        inventoryItemInfo.Configure(weapon);

        inventoryItemInfo.RecycleButton.onClick.RemoveAllListeners();
        inventoryItemInfo.RecycleButton.onClick.AddListener(() => RecycleWeapon(index));
        shopManagerUI.ShowItemInfo();
    }

    /// <summary>
    /// Shows detailed info for a passive object and sets up recycle button.
    /// </summary>
    private void ShowObjectInfo(ObjectDataSO objectData)
    {
        inventoryItemInfo.Configure(objectData);

        inventoryItemInfo.RecycleButton.onClick.RemoveAllListeners();
        inventoryItemInfo.RecycleButton.onClick.AddListener(() => RecycleObject(objectData));
        shopManagerUI.ShowItemInfo();
    }

    /// <summary>
    /// Recycles a passive object, updates inventory UI, and hides info panel.
    /// </summary>
    private void RecycleObject(ObjectDataSO objectData)
    {
        playerObjects.RecycleObject(objectData);
        Configure();
        shopManagerUI.HideItemInfo();
    }

    /// <summary>
    /// Recycles a weapon by index, updates inventory UI, and hides info panel.
    /// </summary>
    private void RecycleWeapon(int index)
    {
        playerWeapons.RecycleWeapon(index);
        Configure();
        shopManagerUI.HideItemInfo();
    }

    /// <summary>
    /// Callback triggered when an item is purchased. Refreshes inventory UI.
    /// </summary>
    private void ItemPurchasedCallback() => Configure();

    /// <summary>
    /// Callback triggered when a weapon is merged. Updates UI with new weapon.
    /// </summary>
    private void WeaponMergedallback(Weapon weapon)
    {
        Configure();
        inventoryItemInfo.Configure(weapon);
    }
}
