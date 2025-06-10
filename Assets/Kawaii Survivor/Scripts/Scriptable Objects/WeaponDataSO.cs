using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon Data", menuName = "Scriptable Objects/New Weapon Data", order = 0)]
public class WeaponDataSO : ScriptableObject
{
    [field: SerializeField] public string WeaponName { get; private set; }
    [field: SerializeField] public Sprite WeaponIcon { get; private set; }
    [field: SerializeField] public int PurchasePrice { get; private set; }
    [field: SerializeField] public int RecyclePrice { get; private set; }

    [field: SerializeField] public AudioClip AttackSound { get; private set; }
    [field: SerializeField] public AnimatorOverrideController AnimatorOverride { get; private set; }

    [field: SerializeField] public Weapon Prefab { get; private set; }

    [HorizontalLine]
    [SerializeField] private float attack;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float criticalChance;
    [SerializeField] private float criticalPercent;
    [SerializeField] private float range;


    public Dictionary<Stat, float> BaseStats
    {
        get
        {
            return new Dictionary<Stat, float>
            {
                { Stat.Attack, attack },
                { Stat.AttackSpeed, attackSpeed },
                { Stat.CriticalChance, criticalChance },
                { Stat.CriticalPercent, criticalPercent },
                { Stat.Range, range },
            };
        }
        private set
        {

        }
    }

    public float GetStatValue(Stat stat)
    {
        foreach (KeyValuePair<Stat, float> pair in BaseStats)
        {
            if (pair.Key == stat)
            {
                return pair.Value;
            }
        }

        Debug.LogError($"Stat {stat} not found in weapon data: {WeaponName}");
        return 0;
    }
}
