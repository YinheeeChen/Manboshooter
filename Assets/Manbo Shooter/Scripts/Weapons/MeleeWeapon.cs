using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A melee-based weapon that automatically targets nearby enemies and performs area attacks using a collider.
/// </summary>
public class MeleeWeapon : Weapon
{
    // Enumeration for weapon states
    enum State
    {
        Idle,       // Not attacking
        Attacking   // Currently executing an attack
    }

    private State state; // Current state of the weapon

    [Header("Elements")]
    [SerializeField] private Transform hitDetectionTransform;          // Position where hit detection happens
    [SerializeField] private BoxCollider2D hitDetectionBoxCollider;    // Collider used for detecting enemies

    [Header("Settings")]
    private List<Enemy> damagedEnemies = new List<Enemy>();            // Enemies already hit in current attack

    // Called once at the start
    void Start()
    {
        state = State.Idle;
    }

    // Called once per frame
    void Update()
    {
        switch (state)
        {
            case State.Idle:
                AutoAim();         // Try to detect and aim at nearby enemies
                break;
            case State.Attacking:
                Attacking();       // Perform attack
                break;
        }
    }

    /// <summary>
    /// Aims the weapon toward the nearest enemy and manages attack timing.
    /// </summary>
    private void AutoAim()
    {
        Enemy closestEnemy = GetClosestEnemy();
        Vector2 targetUpVector = Vector3.up;

        if (closestEnemy != null)
        {
            targetUpVector = (closestEnemy.transform.position - transform.position).normalized;
            transform.up = targetUpVector;

            MangeAttack(); // Check if it's time to attack
        }

        // Smoothly rotate towards target
        transform.up = Vector3.Lerp(transform.up, targetUpVector, Time.deltaTime * aimLerp);

        IncrementAttackTimer(); // Ensures timer increases even if enemy is out of range
    }

    /// <summary>
    /// Handles attack timing logic while aiming.
    /// </summary>
    private void MangeAttack()
    {
        attackTimer += Time.deltaTime;

        if (attackTimer >= attackDelay)
        {
            attackTimer = 0f;
            StartAttack(); // Trigger attack animation and damage logic
        }
    }

    /// <summary>
    /// Increment attack timer continuously while idle.
    /// </summary>
    private void IncrementAttackTimer()
    {
        attackTimer += Time.deltaTime;
    }

    /// <summary>
    /// Starts the attack animation and prepares to apply damage.
    /// </summary>
    [NaughtyAttributes.Button]
    private void StartAttack()
    {
        animator.Play("Attack");              // Trigger animation
        state = State.Attacking;
        damagedEnemies.Clear();               // Reset hit list
        animator.speed = 1f / attackDelay;    // Adjust animation speed based on attack rate
        PlayAttackSound();                    // Optional: play attack SFX
    }

    /// <summary>
    /// Executes during the attacking phase.
    /// </summary>
    private void Attacking()
    {
        Attack(); // Apply damage logic
    }

    /// <summary>
    /// Resets to idle state after attack finishes.
    /// </summary>
    private void StopAttack()
    {
        state = State.Idle;
        damagedEnemies.Clear();
    }

    /// <summary>
    /// Performs the hit detection using a box collider and applies damage to detected enemies.
    /// </summary>
    private void Attack()
    {
        Collider2D[] enemies = Physics2D.OverlapBoxAll(
            hitDetectionTransform.position,
            hitDetectionBoxCollider.bounds.size,
            hitDetectionTransform.localEulerAngles.z,
            enemyMask
        );

        for (int i = 0; i < enemies.Length; i++)
        {
            Enemy enemy = enemies[i].GetComponent<Enemy>();

            // Avoid damaging the same enemy multiple times in one attack
            if (!damagedEnemies.Contains(enemy))
            {
                int damage = GetDamage(out bool isCriticalHit);
                enemy.TakeDamage(damage, isCriticalHit);
                damagedEnemies.Add(enemy);
            }
        }
    }

    /// <summary>
    /// Updates melee weapon stats based on player upgrades.
    /// </summary>
    public override void UpdateStats(PlayerStatManager playerStatManager)
    {
        ConfigureStats(); // Apply base weapon stats

        // Apply additive bonuses from player stats
        damage = Mathf.RoundToInt(damage * (1 + playerStatManager.GetStatVlaue(Stat.Attack) / 100));
        attackDelay /= 1 + (playerStatManager.GetStatVlaue(Stat.AttackSpeed) / 100);

        criticalChance = Mathf.RoundToInt(criticalChance * (1 + playerStatManager.GetStatVlaue(Stat.CriticalChance) / 100));
        criticalPercent += playerStatManager.GetStatVlaue(Stat.CriticalPercent);
    }
}
