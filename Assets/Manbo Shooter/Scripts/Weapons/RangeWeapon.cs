using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// A ranged weapon that automatically targets enemies and fires bullets using object pooling for performance.
/// </summary>
public class RangeWeapon : Weapon
{
    [Header("Elements")]
    [SerializeField] private Bullet bullet;                  // Bullet prefab to instantiate
    [SerializeField] private Transform shootingPoint;        // Position where bullets are fired from

    [Header("Pooling")]
    private ObjectPool<Bullet> bulletPool;                   // Pool to reuse bullet instances

    [Header("Actions")]
    public static Action onBulletShot;                       // Called when a bullet is fired

    // Called on script start
    void Start()
    {
        // Initialize the bullet pool with creation and lifecycle callbacks
        bulletPool = new ObjectPool<Bullet>(
            CreateBulletInstance,
            ActionOnGet,
            ActionOnRelease,
            ActionOnDestroy
        );
    }

    // Called once per frame
    void Update()
    {
        AutoAim(); // Try to find and target the closest enemy
    }

    /// <summary>
    /// Instantiates a new bullet and configures it.
    /// </summary>
    private Bullet CreateBulletInstance()
    {
        Bullet bullet = Instantiate(this.bullet, shootingPoint.position, Quaternion.identity);
        bullet.Configure(this);
        return bullet;
    }

    /// <summary>
    /// Releases a bullet back to the pool.
    /// </summary>
    public void ReleaseBullet(Bullet bullet)
    {
        bulletPool.Release(bullet);
    }

    /// <summary>
    /// Called when a bullet is retrieved from the pool.
    /// </summary>
    private void ActionOnGet(Bullet bullet)
    {
        bullet.Reload();                         // Reset bullet state
        bullet.transform.position = shootingPoint.position;
        bullet.gameObject.SetActive(true);       // Make bullet visible
    }

    /// <summary>
    /// Called when a bullet is returned to the pool.
    /// </summary>
    private void ActionOnRelease(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);      // Hide bullet
    }

    /// <summary>
    /// Called when a bullet is destroyed (optional).
    /// </summary>
    private void ActionOnDestroy(Bullet bullet)
    {
        Destroy(bullet.gameObject);              // Cleanup if pool exceeds limits
    }

    /// <summary>
    /// Automatically aims the weapon at the closest enemy.
    /// </summary>
    private void AutoAim()
    {
        Enemy closestEnemy = GetClosestEnemy();
        Vector2 targetUpVector = Vector3.up;

        if (closestEnemy != null)
        {
            targetUpVector = (closestEnemy.GetCenter() - (Vector2)transform.position).normalized;
            transform.up = targetUpVector;        // Instantly aim at the enemy
            ManageShooting();                     // Handle shooting timing
            return;
        }

        // Smoothly rotate back to default aim if no enemy
        transform.up = Vector3.Lerp(transform.up, targetUpVector, Time.deltaTime * aimLerp);
    }

    /// <summary>
    /// Manages firing delay and shoots when ready.
    /// </summary>
    private void ManageShooting()
    {
        attackTimer += Time.deltaTime;

        if (attackTimer >= attackDelay)
        {
            Shoot();              // Fire a bullet
            attackTimer = 0f;
        }
    }

    /// <summary>
    /// Shoots a bullet in the current direction and triggers effects.
    /// </summary>
    private void Shoot()
    {
        int damage = GetDamage(out bool isCriticalHit);

        Bullet bulletInstance = bulletPool.Get(); // Get bullet from pool
        bulletInstance.Shoot(damage, transform.up, isCriticalHit); // Fire

        onBulletShot?.Invoke();  // Notify listeners
        PlayAttackSound();       // Optional SFX
    }

    /// <summary>
    /// Updates the weapon's stats based on player upgrades.
    /// </summary>
    public override void UpdateStats(PlayerStatManager playerStatManager)
    {
        ConfigureStats(); // Apply base stats

        // Apply scaling from player stats
        damage = Mathf.RoundToInt(damage * (1 + playerStatManager.GetStatVlaue(Stat.Attack) / 100));
        attackDelay /= 1 + (playerStatManager.GetStatVlaue(Stat.AttackSpeed) / 100);

        criticalChance = Mathf.RoundToInt(criticalChance * (1 + playerStatManager.GetStatVlaue(Stat.CriticalChance) / 100));
        criticalPercent += playerStatManager.GetStatVlaue(Stat.CriticalPercent);

        // Adjust range (might need balancing, hence the division by 10)
        range += playerStatManager.GetStatVlaue(Stat.Range) / 10;
    }
}
