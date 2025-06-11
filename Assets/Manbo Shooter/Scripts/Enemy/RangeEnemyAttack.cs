using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// Handles ranged attack behavior for enemies, including bullet shooting and object pooling.
/// </summary>
public class RangeEnemyAttack : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Transform shootingPoint;        // The point from which bullets are fired
    [SerializeField] private EnemyBullet bulletPrefab;       // The bullet prefab to be instantiated
    private Player player;                                   // Reference to the player for aiming

    [Header("Settings")]
    [SerializeField] private int damage;                     // Damage dealt by each bullet
    [SerializeField] private float attackRate;               // Number of attacks per second
    private float attackDelay;                               // Calculated delay between attacks
    private float attackTimer;                               // Timer to track shooting cooldown

    [Header("Bullet Pooling")]
    private ObjectPool<EnemyBullet> bulletPool;              // Object pool to manage bullet instances efficiently

    /// <summary>
    /// Initializes attack timing and sets up the bullet object pool.
    /// </summary>
    void Start()
    {
        attackDelay = 1f / attackRate;
        attackTimer = attackDelay;

        bulletPool = new ObjectPool<EnemyBullet>(
            CreateBulletInstance,
            ActionOnGet,
            ActionOnRelease,
            ActionOnDestroy
        );
    }

    /// <summary>
    /// Update loop – not used currently.
    /// </summary>
    void Update()
    {
        // This class does not rely on Update by default.
    }

    /// <summary>
    /// Creates a new bullet instance and configures it for pooling.
    /// </summary>
    /// <returns>The created EnemyBullet instance.</returns>
    private EnemyBullet CreateBulletInstance()
    {
        EnemyBullet bullet = Instantiate(bulletPrefab, shootingPoint.position, Quaternion.identity);
        bullet.Configure(this); // Set up bullet reference to this attack source
        return bullet;
    }

    /// <summary>
    /// Releases a bullet back into the pool.
    /// </summary>
    /// <param name="bullet">The bullet to release.</param>
    public void ReleaseBullet(EnemyBullet bullet)
    {
        bulletPool.Release(bullet);
    }

    /// <summary>
    /// Called when a bullet is retrieved from the pool.
    /// Resets and activates the bullet.
    /// </summary>
    private void ActionOnGet(EnemyBullet bullet)
    {
        bullet.Reload(); // Reset bullet internal state
        bullet.transform.position = shootingPoint.position;
        bullet.gameObject.SetActive(true);
    }

    /// <summary>
    /// Called when a bullet is released back to the pool.
    /// Deactivates the bullet.
    /// </summary>
    private void ActionOnRelease(EnemyBullet bullet)
    {
        bullet.gameObject.SetActive(false);
    }

    /// <summary>
    /// Called when a bullet is permanently removed from the pool.
    /// </summary>
    private void ActionOnDestroy(EnemyBullet bullet)
    {
        Destroy(bullet.gameObject);
    }

    /// <summary>
    /// Assigns the player reference used for aiming.
    /// </summary>
    /// <param name="player">The player to track.</param>
    public void StorePlayer(Player player)
    {
        this.player = player;
    }

    /// <summary>
    /// Called externally to trigger auto-aiming and shooting behavior.
    /// </summary>
    public void AutoAim()
    {
        ManageShooting();
    }

    /// <summary>
    /// Handles attack cooldown and triggers shooting when ready.
    /// </summary>
    private void ManageShooting()
    {
        attackTimer += Time.deltaTime;

        if (attackTimer >= attackDelay)
        {
            Shoot();
            attackTimer = 0f;
        }
    }

    Vector2 gizmosDirection; // (Unused) Possibly for debug drawing or future visualizations

    /// <summary>
    /// Aims at the player and fires a bullet in their direction.
    /// </summary>
    private void Shoot()
    {
        Vector2 direction = (player.GetCenter() - (Vector2)shootingPoint.position).normalized;
        InstantShoot(direction);
    }

    /// <summary>
    /// Instantly fires a bullet in a given direction with defined damage.
    /// </summary>
    /// <param name="direction">Direction to fire the bullet.</param>
    public void InstantShoot(Vector2 direction)
    {
        EnemyBullet bullet = bulletPool.Get();
        bullet.Shoot(damage, direction);
    }
}
