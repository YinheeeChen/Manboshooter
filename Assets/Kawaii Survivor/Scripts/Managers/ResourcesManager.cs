using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ResourcesManager
{
    const string statIconsDataPath = "Data/Stat Icons";
    const string objectDataPath = "Data/Objects/";
    const string weaponDataPath = "Data/Weapons/";

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
            if (objectDatas == null)
                objectDatas = Resources.LoadAll<ObjectDataSO>(objectDataPath);
            return objectDatas;
        }
        private set { }
    }

    public static ObjectDataSO GetRandomObject()
    {
        return Objects[Random.Range(0, Objects.Length)];
    }

    public static WeaponDataSO[] weaponDatas;
    public static WeaponDataSO[] Weapons
    {
        get
        {
            if (weaponDatas == null)
                weaponDatas = Resources.LoadAll<WeaponDataSO>(weaponDataPath);
            return weaponDatas;
        }
        private set { }
    }

    public static WeaponDataSO GetRandomWeapon()
    {
        return Weapons[Random.Range(0, Weapons.Length)];
    }

}
