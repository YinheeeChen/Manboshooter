using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "Character Data", menuName = "Scriptable Objects/New Character Data", order = 0)]
public class CharacterDataSO : ScriptableObject
{
    [field: SerializeField] public string CharacterName { get; private set; }
    [field: SerializeField] public Sprite CharacterIcon { get; private set; }
    [field: SerializeField] public int PurchasePrice { get; private set; }

    [HorizontalLine]
    [SerializeField] private float attack;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float criticalChance;
    [SerializeField] private float criticalPercent;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float maxHealth;
    [SerializeField] private float range;
    [SerializeField] private float healthRecoverySpeed;
    [SerializeField] private float armor;
    [SerializeField] private float luck;
    [SerializeField] private float dodge;
    [SerializeField] private float lifeSteal;

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
                { Stat.MoveSpeed, moveSpeed },
                { Stat.MaxHealth, maxHealth },
                { Stat.Range, range },
                { Stat.HealthRecoverySpeed, healthRecoverySpeed },
                { Stat.Armor, armor },
                { Stat.Luck, luck },
                { Stat.Dodge, dodge },
                { Stat.LifeSteal, lifeSteal },
            };
        }
        private set
        {
            
        }
    }

}
