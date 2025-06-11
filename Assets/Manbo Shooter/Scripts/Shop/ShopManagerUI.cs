using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the shop UI panels, including player stats, inventory, and item info panels.
/// Handles showing/hiding with animated transitions using LeanTween.
/// </summary>
public class ShopManagerUI : MonoBehaviour
{
    [Header("Player Stats Elements")]
    [SerializeField] private RectTransform playerStatsPanel;       // Panel displaying player stats
    [SerializeField] private RectTransform closeButton;            // Close button for player stats panel
    private Vector2 playerStatsOpenedPos;                          // Position when player stats panel is visible
    private Vector2 playerStatsClosedPos;                          // Position when player stats panel is hidden

    [Header("Inventory Panel Elements")]
    [SerializeField] private RectTransform inventorysPanel;             // Inventory panel
    [SerializeField] private RectTransform inventorysPanelcloseButton; // Close button for inventory panel
    private Vector2 inventorysPanelOpenedPos;                          // Position when inventory panel is visible
    private Vector2 inventorysPanelClosedPos;                          // Position when inventory panel is hidden

    [Header("Item Info Elements")]
    [SerializeField] private RectTransform itemInfoPanel;          // Panel showing selected item details
    private Vector2 itemInfoOpenedPos;                             // Position when item info panel is visible
    private Vector2 itemInfoClosedPos;                             // Position when item info panel is hidden

    /// <summary>
    /// Initializes UI panels after one frame delay to ensure proper layout calculations.
    /// </summary>
    IEnumerator Start()
    {
        yield return null;

        ConfigurePlayerStatsPanel();
        ConfigureInventorysPanel();
        ConfigureItemInfoPanel();
    }

    /// <summary>
    /// Configures the animation positions for the player stats panel.
    /// </summary>
    private void ConfigurePlayerStatsPanel()
    {
        float width = Screen.width / (4 * playerStatsPanel.lossyScale.x);
        playerStatsPanel.offsetMax = playerStatsPanel.offsetMax.With(x: width);

        playerStatsOpenedPos = playerStatsPanel.anchoredPosition;
        playerStatsClosedPos = playerStatsOpenedPos + Vector2.left * width;

        playerStatsPanel.anchoredPosition = playerStatsClosedPos;
        HidePlayerStats();
    }

    /// <summary>
    /// Configures the animation positions for the inventory panel.
    /// </summary>
    private void ConfigureInventorysPanel()
    {
        float width = Screen.width / (4 * inventorysPanel.lossyScale.x);
        inventorysPanel.offsetMin = inventorysPanel.offsetMin.With(x: -width);

        inventorysPanelOpenedPos = inventorysPanel.anchoredPosition;
        inventorysPanelClosedPos = inventorysPanelOpenedPos - Vector2.left * width;

        inventorysPanel.anchoredPosition = inventorysPanelClosedPos;
        HideInventorys(false);
    }

    /// <summary>
    /// Configures the animation positions for the item info panel.
    /// </summary>
    private void ConfigureItemInfoPanel()
    {
        float height = Screen.height / (2 * itemInfoPanel.lossyScale.x);
        itemInfoPanel.offsetMax = itemInfoPanel.offsetMax.With(y: height);

        itemInfoOpenedPos = itemInfoPanel.anchoredPosition;
        itemInfoClosedPos = itemInfoOpenedPos + Vector2.down * height;

        itemInfoPanel.anchoredPosition = itemInfoClosedPos;
        itemInfoPanel.gameObject.SetActive(false);
    }

    /// <summary>
    /// Shows the player stats panel with animation.
    /// </summary>
    public void ShowPlayerStats()
    {
        playerStatsPanel.gameObject.SetActive(true);
        closeButton.gameObject.SetActive(true);
        closeButton.GetComponent<Image>().raycastTarget = true;

        LeanTween.cancel(playerStatsPanel);
        LeanTween.move(playerStatsPanel, playerStatsOpenedPos, 0.5f).setEase(LeanTweenType.easeOutBack);

        LeanTween.cancel(closeButton);
        LeanTween.alpha(closeButton, 0.8f, 0.5f).setRecursive(false);
    }

    /// <summary>
    /// Hides the player stats panel with animation.
    /// </summary>
    public void HidePlayerStats()
    {
        closeButton.GetComponent<Image>().raycastTarget = false;

        LeanTween.cancel(playerStatsPanel);
        LeanTween.move(playerStatsPanel, playerStatsClosedPos, 0.5f)
            .setEase(LeanTweenType.easeOutBack)
            .setOnComplete(() => playerStatsPanel.gameObject.SetActive(false));

        LeanTween.cancel(closeButton);
        LeanTween.alpha(closeButton, 0, 0.5f)
            .setRecursive(false)
            .setOnComplete(() => closeButton.gameObject.SetActive(false));
    }

    /// <summary>
    /// Shows the inventory panel with animation.
    /// </summary>
    public void ShowInventorys()
    {
        inventorysPanel.gameObject.SetActive(true);
        inventorysPanelcloseButton.gameObject.SetActive(true);
        inventorysPanelcloseButton.GetComponent<Image>().raycastTarget = true;

        LeanTween.cancel(inventorysPanel);
        LeanTween.move(inventorysPanel, inventorysPanelOpenedPos, 0.5f).setEase(LeanTweenType.easeOutBack);

        LeanTween.cancel(inventorysPanelcloseButton);
        LeanTween.alpha(inventorysPanelcloseButton, 0.8f, 0.5f).setRecursive(false);
    }

    /// <summary>
    /// Hides the inventory panel with animation.
    /// Also optionally hides the item info panel.
    /// </summary>
    public void HideInventorys(bool hideItemInfo = true)
    {
        inventorysPanelcloseButton.GetComponent<Image>().raycastTarget = false;

        LeanTween.cancel(inventorysPanel);
        LeanTween.move(inventorysPanel, inventorysPanelClosedPos, 0.5f)
            .setEase(LeanTweenType.easeOutBack)
            .setOnComplete(() => inventorysPanel.gameObject.SetActive(false));

        LeanTween.cancel(inventorysPanelcloseButton);
        LeanTween.alpha(inventorysPanelcloseButton, 0, 0.5f)
            .setRecursive(false)
            .setOnComplete(() => inventorysPanelcloseButton.gameObject.SetActive(false));

        if (hideItemInfo)
            HideItemInfo();
    }

    /// <summary>
    /// Shows the item info panel with animation.
    /// </summary>
    public void ShowItemInfo()
    {
        itemInfoPanel.gameObject.SetActive(true);

        itemInfoPanel.LeanCancel();
        itemInfoPanel.LeanMove((Vector3)itemInfoOpenedPos, 0.3f)
            .setEase(LeanTweenType.easeOutBack);
    }

    /// <summary>
    /// Hides the item info panel with animation.
    /// </summary>
    public void HideItemInfo()
    {
        itemInfoPanel.LeanCancel();
        itemInfoPanel.LeanMove((Vector3)itemInfoClosedPos, 0.3f)
            .setEase(LeanTweenType.easeOutBack)
            .setOnComplete(() => itemInfoPanel.gameObject.SetActive(false));
    }
}
