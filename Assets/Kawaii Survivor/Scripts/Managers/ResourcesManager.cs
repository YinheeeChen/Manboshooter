using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ResourcesManager
{
    const string statIconsDataPath = "Data/Stat Icons";

    private static StatIcon[] statIcons;
    public static Sprite GetStatIcon(Stat stat)
    {
        if(statIcons == null)
        {
            StatIconsDataSO statIconsData = Resources.Load<StatIconsDataSO>(statIconsDataPath);
            statIcons = statIconsData.StatIcons;
        }

        foreach (StatIcon statIcon in statIcons)
            if (statIcon.stat == stat)
                return statIcon.icon;
            
        return null;
    }
}
