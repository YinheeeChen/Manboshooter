using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour, IPlayerStatDependency
{

    [Header("Settings")]
    [SerializeField] private int baseMaxHealth;
    private float maxHealth;
    private float health;
    private float armor;
    private float lifeSteal;
    private float dodge;
    private float healthRecoverySpeed;
    private float healthRevoveryValue;
    private float healthRecoveryTimer;
    private float healthRecoveryDuration;

    [Header("Elements")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TextMeshProUGUI healthText;

    [Header("Actions")]
    public static Action<Vector2> onAttackDodged;

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
        {
            PassAway();
        }
        else
        {
            // Handle player taking damage
            Debug.Log($"Player took {realDamage} damage. Remaining health: {health}");
        }
    }

    private bool ShouldDodge()
    {
        return UnityEngine.Random.Range(0f, 100f) < dodge;
    }

    private void PassAway()
    {
        GameManager.instance.SetGmaeState(GameState.GAMEOVER);
    }

    private void UpdateHealthUI()
    {
        healthSlider.value = health / maxHealth;
        healthText.text = (int)health + " / " + maxHealth;
    }


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
