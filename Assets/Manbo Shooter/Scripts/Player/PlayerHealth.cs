using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the player's health, armor, life steal, dodge, and health recovery.
/// Implements IPlayerStatDependency for stat synchronization.
/// </summary>
public class PlayerHealth : MonoBehaviour, IPlayerStatDependency
{
    [Header("Settings")]
    [SerializeField] private int baseMaxHealth; // The base maximum health of the player
    private float maxHealth;                    // The current maximum health of the player
    private float health;                       // The current health of the player
    private float armor;                        // The player's armor value
    private float lifeSteal;                    // The player's life steal percentage (0-1)
    private float dodge;                        // The player's dodge chance (0-100)
    private float healthRecoverySpeed;          // The player's health recovery speed
    private float healthRecoveryTimer;          // Timer for health recovery
    private float healthRecoveryDuration;       // Duration between each health recovery tick

    [Header("Elements")]
    [SerializeField] private Slider healthSlider;           // UI slider for displaying health
    [SerializeField] private TextMeshProUGUI healthText;    // UI text for displaying health values

    [Header("Actions")]
    public static Action<Vector2> onAttackDodged;           // Event triggered when the player dodges an attack

    private void Awake()
    {
        Enemy.onDamageTaken += EnemyTookDamageCallback;
    }

    private void OnDestroy()
    {
        Enemy.onDamageTaken -= EnemyTookDamageCallback;
    }

    private void EnemyTookDamageCallback(int damage, Vector2 enemyPos, bool isCritical)
    {
        if (health >= maxHealth) return;

        float lifeStealValue = damage * lifeSteal;
        float healthToAdd = Mathf.Min(lifeStealValue, maxHealth - health);

        health += healthToAdd;
        UpdateHealthUI();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(health < maxHealth) RecoverHealth();
    }
    
    private void RecoverHealth()
    {
        healthRecoveryTimer += Time.deltaTime;

        if (healthRecoveryTimer >= healthRecoveryDuration)
        {
            healthRecoveryTimer = 0;

            float healthToAdd = Mathf.Min(.1f, maxHealth - health);
            health += healthToAdd;

            UpdateHealthUI();
        }
    }   

    /// <summary>
    /// Applies damage to the player, considering dodge and armor.
    /// </summary>
    /// <param name="damage">The amount of damage to apply.</param>
    public void TakeDamage(int damage)
    {
        if (ShouldDodge())
        {
            onAttackDodged?.Invoke(transform.position);
            return;
        }

        float realDamage = damage * Mathf.Clamp(1 - (armor / 1000), 0, 10000);
        realDamage = Mathf.Min(realDamage, health);
        health -= realDamage;

        UpdateHealthUI();

        if (health <= 0)
            PassAway();

    }

    /// <summary>
    /// Determines if the player dodges an attack.
    /// </summary>
    /// <returns>True if dodged, false otherwise.</returns>
    private bool ShouldDodge()
    {
        return UnityEngine.Random.Range(0f, 100f) < dodge;
    }

    /// <summary>
    /// Handles player death.
    /// </summary>
    private void PassAway()
    {
        GameManager.instance.SetGmaeState(GameState.GAMEOVER);
    }

    /// <summary>
    /// Updates the health UI elements.
    /// </summary>
    private void UpdateHealthUI()
    {
        healthSlider.value = health / maxHealth;
        healthText.text = (int)health + " / " + maxHealth;
    }

    /// <summary>
    /// Updates player stats from the PlayerStatManager.
    /// </summary>
    /// <param name="playerStatManager">The PlayerStatManager instance.</param>
    public void UpdateStats(PlayerStatManager playerStatManager)
    {
        float addedHealth = playerStatManager.GetStatVlaue(Stat.MaxHealth);
        maxHealth = baseMaxHealth + (int)addedHealth;
        maxHealth = Mathf.Max(maxHealth, 1);

        health = maxHealth;
        UpdateHealthUI();

        armor = playerStatManager.GetStatVlaue(Stat.Armor);
        lifeSteal = playerStatManager.GetStatVlaue(Stat.LifeSteal) / 100;
        dodge = playerStatManager.GetStatVlaue(Stat.Dodge);

        healthRecoverySpeed = Mathf.Max(.0001f, playerStatManager.GetStatVlaue(Stat.HealthRecoverySpeed));
        healthRecoveryDuration = 1f / healthRecoverySpeed;
    }
}