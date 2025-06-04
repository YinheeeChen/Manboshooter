using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class StatContainerManager : MonoBehaviour
{
    public static StatContainerManager instance;

    [Header("Elements")]
    [SerializeField] private StatContainer statContainer;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);
    }

    private void GenerateContainers(Dictionary<Stat, float> statDictionary, Transform parent)
    {
        List<StatContainer> statContainers = new List<StatContainer>();

        foreach (KeyValuePair<Stat, float> stat in statDictionary)
        {
            StatContainer container = Instantiate(statContainer, parent);
            statContainers.Add(container);

            Sprite icon = ResourcesManager.GetStatIcon(stat.Key);
            string statName = Enums.FormatStatName(stat.Key);
            float statValue = stat.Value;

            container.Configure(icon, statName, statValue);
        }

        LeanTween.delayedCall(Time.deltaTime * 2, () =>
        {
            ResizeTexts(statContainers);
        });
    }

    private void ResizeTexts(List<StatContainer> statContainers)
    {
        float minFontSize = 5000;

        for (int i = 0; i < statContainers.Count; i++)
        {
            StatContainer statContainer = statContainers[i];
            float fontSize = statContainer.GetFontSize();
            if (fontSize < minFontSize)
                minFontSize = fontSize;
        }

        for (int i = 0; i < statContainers.Count; i++)
        {
            statContainers[i].SetFontSize(minFontSize);
        }
    }
    
    public static void GenerateStatContainers(Dictionary<Stat, float> statDictionary, Transform parent)
    {
        instance.GenerateContainers(statDictionary, parent);
    }
}
