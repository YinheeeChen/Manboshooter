using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ResourcesManager
{
    const string statIconsDataPath = "Data/Stat Icons";
    const string objectDataPath = "Data/Objects/";

    private static StatIcon[] statIcons;
    public static Sprite GetStatIcon(Stat stat)
    {
        if (statIcons == null)
        {
            StatIconsDataSO statIconsData = Resources.Load<StatIconsDataSO>(statIconsDataPath);
            statIcons = statIconsData.StatIcons;
        }

        foreach (StatIcon statIcon in statIcons)
            if (statIcon.stat == stat)
                return statIcon.icon;

        return null;
    }

    public static ObjectDataSO[] objectDatas;
    public static ObjectDataSO[] Objects
    {
        get
        {
            if(objectDatas == null)
                objectDatas = Resources.LoadAll<ObjectDataSO>(objectDataPath);
            return objectDatas;
        }
        private set{ }
    }
}
