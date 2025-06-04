using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChestObjectContainer : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI nameText;

    [Header("Stats")]
    [SerializeField] private Transform statContainersParent;

    [field: SerializeField] public Button TakeButton { get; private set; }
    [field: SerializeField] public Button RecycleButton { get; private set; }

    [Header("Color")]
    [SerializeField] private Image[] levelImages;
    [SerializeField] private Image outline;

    public void Configure(ObjectDataSO objectData)
    {
        if (icon != null)
            icon.sprite = objectData.Icon;

        if (nameText != null)
            nameText.text = objectData.Name;

        Color imageColor = ColorHolder.GetColor(objectData.Rarity);
        nameText.color = imageColor;

        outline.color = ColorHolder.GetOutlineColor(objectData.Rarity);

        foreach (Image image in levelImages)
            image.color = imageColor;

        ConfigureStatContainers(objectData.BaseStats);
    }

    private void ConfigureStatContainers(Dictionary<Stat, float> stats)
    {
        StatContainerManager.GenerateStatContainers(stats, statContainersParent);
    }
}
