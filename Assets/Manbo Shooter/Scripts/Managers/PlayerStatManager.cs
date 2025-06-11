using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using System.Linq;
using System;

/// <summary>
/// Manages the player's stats during gameplay, including base stats, additive bonuses,
/// and temporary boosts from objects.
/// Notifies all systems that implement IPlayerStatDependency when stats change.
/// </summary>
public class PlayerStatManager : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private CharacterDataSO playerData;  // Reference to the current character's base stat data

    [Header("Settings")]
    private Dictionary<Stat, float> addends = new Dictionary<Stat, float>();         // Runtime stat boosts (e.g. upgrades)
    private Dictionary<Stat, float> playerStats = new Dictionary<Stat, float>();     // Base stats from CharacterDataSO
    private Dictionary<Stat, float> objectAddends = new Dictionary<Stat, float>();   // Bonuses from equipped/passive objects

    /// <summary>
    /// Initializes stat dictionaries and subscribes to character selection event.
    /// </summary>
    private void Awake()
    {
        CharacterSelectionManager.OnCharacterSelected += CharacterSelectedCallback;
        playerStats = playerData.BaseStats;

        foreach (KeyValuePair<Stat, float> kvp in playerStats)
        {
            addends.Add(kvp.Key, 0f);
            objectAddends.Add(kvp.Key, 0f);
        }
    }

    /// <summary>
    /// Unsubscribes from character selection event to prevent memory leaks.
    /// </summary>
    private void OnDestroy()
    {
        CharacterSelectionManager.OnCharacterSelected -= CharacterSelectedCallback;
    }

    /// <summary>
    /// Updates all systems that rely on player stats.
    /// </summary>
    void Start() => UpdatePlayerStats();

    /// <summary>
    /// Adds stat values provided by a new object (e.g. passive item).
    /// </summary>
    /// <param name="objectStats">Dictionary of stats to add.</param>
    public void AddObject(Dictionary<Stat, float> objectStats)
    {
        foreach (KeyValuePair<Stat, float> kvp in objectStats)
            objectAddends[kvp.Key] += kvp.Value;

        UpdatePlayerStats();
    }

    /// <summary>
    /// Gets the total value of a stat, including base, addends, and object bonuses.
    /// </summary>
    /// <param name="stat">The stat to query.</param>
    /// <returns>The final calculated value.</returns>
    public float GetStatVlaue(Stat stat) => playerStats[stat] + addends[stat] + objectAddends[stat];

    /// <summary>
    /// Increases a player stat at runtime (e.g. through level up or upgrade).
    /// </summary>
    /// <param name="stat">The stat to modify.</param>
    /// <param name="value">The value to add.</param>
    public void AddPlayerStat(Stat stat, float value)
    {
        if (addends.ContainsKey(stat))
            addends[stat] += value;
        else
            Debug.LogError($"Stat {stat} not found in addends dictionary.");

        UpdatePlayerStats();
    }

    /// <summary>
    /// Notifies all components implementing IPlayerStatDependency to refresh based on current stats.
    /// </summary>
    public void UpdatePlayerStats()
    {
        IEnumerable<IPlayerStatDependency> playerStatDependencies =
            FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None)
            .OfType<IPlayerStatDependency>();

        foreach (IPlayerStatDependency dependency in playerStatDependencies)
        {
            dependency.UpdateStats(this);
        }
    }

    /// <summary>
    /// Removes stat bonuses associated with a removed object (e.g. recycled or unequipped item).
    /// </summary>
    /// <param name="objectStats">Dictionary of stat values to subtract.</param>
    public void RemoveObjectStats(Dictionary<Stat, float> objectStats)
    {
        foreach (KeyValuePair<Stat, float> kvp in objectStats)
        {
            if (objectAddends.ContainsKey(kvp.Key))
                objectAddends[kvp.Key] -= kvp.Value;
            else
                Debug.LogError($"Stat {kvp.Key} not found in object addends dictionary.");
        }

        UpdatePlayerStats();
    }

    /// <summary>
    /// Callback for when a new character is selected.
    /// Reinitializes all stat dictionaries based on the selected character.
    /// </summary>
    /// <param name="characterData">The newly selected character's data.</param>
    private void CharacterSelectedCallback(CharacterDataSO characterData)
    {
        playerData = characterData;
        playerStats = characterData.BaseStats;

        addends.Clear();
        objectAddends.Clear();

        foreach (KeyValuePair<Stat, float> kvp in playerStats)
        {
            addends.Add(kvp.Key, 0f);
            objectAddends.Add(kvp.Key, 0f);
        }

        UpdatePlayerStats();
    }
}
