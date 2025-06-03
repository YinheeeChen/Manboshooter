using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "ObjectData", menuName = "Scriptable Objects/Object Data", order = 0)]
public class ObjectDataSO : ScriptableObject
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public Sprite Icon { get; private set; }
    [field: SerializeField] public int Price { get; private set; }

    [field: Range(0, 3)]
    [field: SerializeField] public int Rarity { get; private set; }

    [SerializeField] private StatData[] statDatas;

    public Dictionary<Stat, float> BaseStats
    {
        get
        {
            Dictionary<Stat, float> stats = new Dictionary<Stat, float>();

            foreach (StatData data in statDatas)
                stats.Add(data.stat, data.value);
            return stats;
        }
    }
}

[System.Serializable]
public struct StatData
{
    public Stat stat;
    public float value;
}
