using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManagerUI : MonoBehaviour
{
    [Header("Player Stats Elements")]
    [SerializeField] private RectTransform playerStatsPanel;
    [SerializeField] private RectTransform closeButton;
    private Vector2 playerStatsOpenedPos;
    private Vector2 playerStatsClosedPos;


    [Header("Player Stats Elements")]
    [SerializeField] private RectTransform inventorysPanel;
    [SerializeField] private RectTransform inventorysPanelcloseButton;
    private Vector2 inventorysPanelOpenedPos;
    private Vector2 inventorysPanelClosedPos;

    [Header("Item Info Elements")]
    [SerializeField] private RectTransform itemInfoPanel;
    private Vector2 itemInfoOpenedPos;
    private Vector2 itemInfoClosedPos;



    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return null;

        ConfigurePlayerStatsPanel();
        ConfigureInventorysPanel();
        ConfigureItemInfoPanel();
    }


    // Update is called once per frame
    void Update()
    {

    }

    private void ConfigurePlayerStatsPanel()
    {
        float width = Screen.width / (4 * playerStatsPanel.lossyScale.x);
        playerStatsPanel.offsetMax = playerStatsPanel.offsetMax.With(x: width);

        playerStatsOpenedPos = playerStatsPanel.anchoredPosition;
        playerStatsClosedPos = playerStatsOpenedPos + Vector2.left * width;

        playerStatsPanel.anchoredPosition = playerStatsClosedPos;

        HidePlayerStats();
    }

    private void ConfigureInventorysPanel()
    {
        float width = Screen.width / (4 * inventorysPanel.lossyScale.x);
        inventorysPanel.offsetMin = inventorysPanel.offsetMin.With(x: -width);

        inventorysPanelOpenedPos = inventorysPanel.anchoredPosition;
        inventorysPanelClosedPos = inventorysPanelOpenedPos - Vector2.left * width;

        inventorysPanel.anchoredPosition = inventorysPanelClosedPos;

        HideInventorys(false);
    }

    private void ConfigureItemInfoPanel()
    {
        float height = Screen.height / (2 * itemInfoPanel.lossyScale.x);
        itemInfoPanel.offsetMax = itemInfoPanel.offsetMax.With(y: height);

        itemInfoOpenedPos = itemInfoPanel.anchoredPosition;
        itemInfoClosedPos = itemInfoOpenedPos + Vector2.down * height;

        itemInfoPanel.anchoredPosition = itemInfoClosedPos;

        itemInfoPanel.gameObject.SetActive(false);
    }

    [NaughtyAttributes.Button("Show Player Stats")]
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

    [NaughtyAttributes.Button("Hide Player Stats")]
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

    [NaughtyAttributes.Button("Show Inventorys")]
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

    [NaughtyAttributes.Button("Hide Inventorys")]
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
        if(hideItemInfo)
            HideItemInfo();
    }

    [NaughtyAttributes.Button("Show Item Info")]
    public void ShowItemInfo()
    {
        itemInfoPanel.gameObject.SetActive(true);

        itemInfoPanel.LeanCancel();
        itemInfoPanel.LeanMove((Vector3)itemInfoOpenedPos, 0.3f)
            .setEase(LeanTweenType.easeOutBack);
    }
    
    [NaughtyAttributes.Button("Hide Item Info")]
    public void HideItemInfo()
    {
        itemInfoPanel.LeanCancel();
        itemInfoPanel.LeanMove((Vector3)itemInfoClosedPos, 0.3f)
            .setEase(LeanTweenType.easeOutBack)
            .setOnComplete(() => itemInfoPanel.gameObject.SetActive(false));
    }
}
