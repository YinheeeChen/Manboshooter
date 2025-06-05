using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WeaponStatsCalculator
{
    public static Dictionary<Stat, float> GetStats(WeaponDataSO weaponData, int level)
    {
        float multiplier = 1 + (float)level / 3;
        Dictionary<Stat, float> calculatedStats = new Dictionary<Stat, float>();

        foreach (KeyValuePair<Stat, float> stat in weaponData.BaseStats)
        {
            if (weaponData.Prefab.GetType() != typeof(RangeWeapon) && stat.Key == Stat.Range)
                calculatedStats.Add(stat.Key, stat.Value);
            else
                calculatedStats.Add(stat.Key, stat.Value * multiplier);

        }

        return calculatedStats;
    }
    
    public static int GetPurchasePrice(WeaponDataSO weaponData, int level)
    {
        float multiplier = 1 + (float)level / 3;
        return Mathf.RoundToInt(weaponData.PurchasePrice * multiplier);
    }
}
