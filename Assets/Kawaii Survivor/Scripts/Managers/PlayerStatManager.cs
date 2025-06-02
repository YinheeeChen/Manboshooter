using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using System.Linq;

public class PlayerStatManager : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private CharacterDataSO playerData;

    [Header("Settings")]
    // private List<StatData> statDatas = new List<StatData>();
    private Dictionary<Stat, float> addends = new Dictionary<Stat, float>();
    private Dictionary<Stat, float> playerStats = new Dictionary<Stat, float>();

    private void Awake()
    {
        playerStats = playerData.BaseStats;

        foreach(KeyValuePair<Stat, float> kvp in playerStats)
        {
            addends.Add(kvp.Key, 0f);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // addends.Add(Stat.MaxHealth, 10);

        UpdatePlayerStats();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public float GetStatVlaue(Stat stat)
    {
        float value = playerStats[stat] + addends[stat];
        return value;
    }

    public void AddPlayerStat(Stat stat, float value)
    {
        if (addends.ContainsKey(stat))
            addends[stat] += value;

        else
            Debug.LogError($"Stat {stat} not found in addends dictionary.");

        UpdatePlayerStats();
    }
    
    public void UpdatePlayerStats()
    {
        IEnumerable<IPlayerStatDependency> playerStatDependencies =
            FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
            .OfType<IPlayerStatDependency>();

        foreach (IPlayerStatDependency dependency in playerStatDependencies)
        {
            dependency.UpdateStats(this);
        }
    }
}
