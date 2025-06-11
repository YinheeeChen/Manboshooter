using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using Random = UnityEngine.Random;

/// <summary>
/// Manages the in-game shop during the SHOP game state.
/// Handles weapon and object offerings, rerolls, purchases, and visual updates.
/// Implements IGameStateListener to react to game state transitions.
/// </summary>
public class ShopManager : MonoBehaviour, IGameStateListener
{
    [Header("Elements")]
    [SerializeField] private Transform containerParent;                         // Parent container for shop item UI
    [SerializeField] private ShopItemContainer shopItemContainerPrefab;        // Prefab for shop item UI

    [Header("Player Components")]
    [SerializeField] private PlayerWeapons playerWeapons;                      // Reference to player weapon system
    [SerializeField] private PlayerObjects playerObjects;                      // Reference to player object inventory

    [Header("Reroll")]
    [SerializeField] private Button rerollButton;                              // Button to reroll shop items
    [SerializeField] private int rerollPrice;                                  // Cost to reroll shop
    [SerializeField] private TextMeshProUGUI rerollPriceText;                  // Text displaying reroll cost

    [Header("Actions")]
    public static Action onItemPurchased;                                      // Event invoked when an item is purchased

    /// <summary>
    /// Subscribes to shop and currency events.
    /// </summary>
    private void Awake()
    {
        ShopItemContainer.onPurchased += ShopItemPurchasedCallback;
        CurrencyManager.onUpdated += CurrencyUpdatedCallback;
    }

    /// <summary>
    /// Unsubscribes from all events.
    /// </summary>
    private void OnDestroy()
    {
        ShopItemContainer.onPurchased -= ShopItemPurchasedCallback;
        CurrencyManager.onUpdated -= CurrencyUpdatedCallback;
    }

    /// <summary>
    /// Called on game state change. Initializes shop if entering SHOP state.
    /// </summary>
    public void GmaeStateChangeCallback(GameState gameState)
    {
        if (gameState == GameState.SHOP)
        {
            Configure();
            UpdateRerollVisuals();
        }
    }

    /// <summary>
    /// Configures the shop UI by destroying unlocked containers and instantiating new offers.
    /// </summary>
    private void Configure()
    {
        List<GameObject> toDestroy = new List<GameObject>();

        // Mark all unlocked containers for destruction
        for (int i = 0; i < containerParent.childCount; i++)
        {
            ShopItemContainer container = containerParent.GetChild(i).GetComponent<ShopItemContainer>();
            if (!container.IsLocked)
                toDestroy.Add(container.gameObject);
        }

        // Destroy all unlocked containers
        while (toDestroy.Count > 0)
        {
            Transform t = toDestroy[0].transform;
            t.SetParent(null);
            Destroy(t.gameObject);
            toDestroy.RemoveAt(0);
        }

        // Decide how many new containers to add
        int containersToAdd = 6 - containerParent.childCount;
        int weaponContainerCount = Random.Range(Mathf.Min(2, containersToAdd), containersToAdd);
        int objectContainerCount = containersToAdd - weaponContainerCount;

        // Add weapon offers
        for (int i = 0; i < weaponContainerCount; i++)
        {
            ShopItemContainer weaponContainerInstance = Instantiate(shopItemContainerPrefab, containerParent);
            WeaponDataSO randomWeapon = ResourcesManager.GetRandomWeapon();
            weaponContainerInstance.Configure(randomWeapon, Random.Range(0, 2));
        }

        // Add object offers
        for (int i = 0; i < objectContainerCount; i++)
        {
            ShopItemContainer objectContainerInstance = Instantiate(shopItemContainerPrefab, containerParent);
            ObjectDataSO randomObject = ResourcesManager.GetRandomObject();
            objectContainerInstance.Configure(randomObject);
        }
    }

    /// <summary>
    /// Handles reroll button press. Refreshes shop and deducts currency.
    /// </summary>
    public void Reroll()
    {
        Configure();
        CurrencyManager.instance.UseCurrency(rerollPrice);
    }

    /// <summary>
    /// Updates reroll button's interactable state and cost display.
    /// </summary>
    public void UpdateRerollVisuals()
    {
        rerollPriceText.text = rerollPrice.ToString();
        rerollButton.interactable = CurrencyManager.instance.HasEnoughCurrency(rerollPrice);
    }

    /// <summary>
    /// Called when currency changes. Updates reroll button visuals.
    /// </summary>
    private void CurrencyUpdatedCallback()
    {
        UpdateRerollVisuals();
    }

    /// <summary>
    /// Called when a shop item is purchased. Dispatches purchase logic based on item type.
    /// </summary>
    private void ShopItemPurchasedCallback(ShopItemContainer container, int weaponLevel)
    {
        if (container.WeaponData != null)
            TryPurchaseWeapon(container, weaponLevel);
        else
            PurchaseObject(container);
    }

    /// <summary>
    /// Attempts to add a weapon to the player's inventory and deducts cost if successful.
    /// </summary>
    private void TryPurchaseWeapon(ShopItemContainer container, int weaponLevel)
    {
        if (playerWeapons.TryAddWeapon(container.WeaponData, weaponLevel))
        {
            int price = WeaponStatsCalculator.GetPurchasePrice(container.WeaponData, weaponLevel);
            CurrencyManager.instance.UseCurrency(price);
            Destroy(container.gameObject);
        }

        onItemPurchased?.Invoke();
    }

    /// <summary>
    /// Purchases a passive object and adds it to the player's collection.
    /// </summary>
    private void PurchaseObject(ShopItemContainer container)
    {
        playerObjects.AddObject(container.ObjectData);
        CurrencyManager.instance.UseCurrency(container.ObjectData.Price);
        Destroy(container.gameObject);
        onItemPurchased?.Invoke();
    }
}
