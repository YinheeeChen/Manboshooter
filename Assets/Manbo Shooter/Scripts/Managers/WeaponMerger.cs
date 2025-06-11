using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMerger : MonoBehaviour
{
    public static WeaponMerger instacne;

    [Header("Elements")]
    [SerializeField] private PlayerWeapons PlayerWeapons;

    [Header("Settings")]
    private List<Weapon> mergeableWeapons = new List<Weapon>();

    [Header("Actions")]
    public static Action<Weapon> onMerge;

    private void Awake()
    {
        if (instacne == null)
            instacne = this;
        else
            Destroy(gameObject);
    }

    public bool CanMerge(Weapon weapon)
    {
        if (weapon.Level >= 3)
            return false;

        mergeableWeapons.Clear();
        mergeableWeapons.Add(weapon);

        Weapon[] weapons = PlayerWeapons.GetWeapons();

        foreach (Weapon w in weapons)
        {
            if (w == null)
                continue;
            if (w == weapon)
                continue;
            if (w.WeaponData.WeaponName != weapon.WeaponData.WeaponName)
                continue;
            if (w.Level != weapon.Level)
                continue;

            mergeableWeapons.Add(w);

            return true; // Found a matching weapon to merge with
        }

        return false; // No matching weapon found
    }

    public void Merge()
    {
        if (mergeableWeapons.Count < 2)
        {
            Debug.LogWarning("Not enough weapons to merge.");
            return;
        }

        DestroyImmediate(mergeableWeapons[1].gameObject);

        mergeableWeapons[0].Upgrade();

        Weapon w = mergeableWeapons[0];
        mergeableWeapons.Clear();

        onMerge?.Invoke(w);
    }

}
