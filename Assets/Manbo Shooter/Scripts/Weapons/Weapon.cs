using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract base class for all weapon types. Handles common functionality such as targeting, damage calculation,
/// stat configuration, and sound effects. Implements IPlayerStatDependency for dynamic stat updates.
/// </summary>
public abstract class Weapon : MonoBehaviour, IPlayerStatDependency
{
    [field: SerializeField] public WeaponDataSO WeaponData { get; private set; } // Weapon data (SO) for base values

    [Header("Settings")]
    [SerializeField] protected float range;               // Range of the weapon's targeting
    [SerializeField] protected LayerMask enemyMask;       // Layer to detect enemies

    [Header("Attack")]
    [SerializeField] protected int damage;                // Base damage
    [SerializeField] protected float attackDelay;         // Delay between attacks
    [SerializeField] protected Animator animator;         // Animator for weapon attack animations
    protected float attackTimer;                          // Timer for attack cooldown

    [Header("Critical Hit")]
    protected int criticalChance;                         // % chance to trigger a critical hit
    protected float criticalPercent;                      // Critical hit damage multiplier

    [Header("Aiming")]
    [SerializeField] protected float aimLerp;             // Speed of aiming toward the target

    [Header("Level")]
    public int Level { get; private set; }                // Current weapon level

    [Header("Audio")]
    protected AudioSource audioSource;                    // Audio source for playing attack sound effects

    /// <summary>
    /// Unity Awake method. Initializes audio and animator override from WeaponData.
    /// </summary>
    protected void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.clip = WeaponData.AttackSound;

        if (animator != null && WeaponData.AnimatorOverride != null)
            animator.runtimeAnimatorController = WeaponData.AnimatorOverride;
    }

    /// <summary>
    /// Plays the weapon's attack sound if SFX is enabled in AudioManager.
    /// </summary>
    protected void PlayAttackSound()
    {
        if (!AudioManager.instance.IsSFXOn)
            return;

        audioSource.pitch = Random.Range(0.9f, 1.05f); // Adds variation to avoid repetition
        audioSource.Play();
    }

    /// <summary>
    /// Finds the closest enemy within range.
    /// </summary>
    /// <returns>Closest Enemy or null if none found.</returns>
    protected Enemy GetClosestEnemy()
    {
        Enemy closestEnemy = null;
        Vector2 targetUpVector = Vector3.up;
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, range, enemyMask);

        if (enemies.Length <= 0)
        {
            transform.up = targetUpVector; // Reset aim direction
            return null;
        }

        float closestDistance = range;

        foreach (Collider2D col in enemies)
        {
            Enemy enemyChecked = col.GetComponent<Enemy>();
            float distance = Vector2.Distance(transform.position, enemyChecked.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemyChecked;
            }
        }

        return closestEnemy;
    }

    /// <summary>
    /// Calculates weapon damage with critical hit chance.
    /// </summary>
    /// <param name="isCriticalHit">Output: was it a critical hit?</param>
    /// <returns>Final damage amount</returns>
    protected int GetDamage(out bool isCriticalHit)
    {
        isCriticalHit = false;

        if (Random.Range(0, 101) <= criticalChance)
        {
            isCriticalHit = true;
            return Mathf.RoundToInt(damage * criticalPercent);
        }

        return damage;
    }

    /// <summary>
    /// Visualizes the weapon range in the editor.
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    /// <summary>
    /// Abstract method to update weapon stats from player stats.
    /// </summary>
    public abstract void UpdateStats(PlayerStatManager playerStatManager);

    /// <summary>
    /// Configures stats from WeaponData using WeaponStatsCalculator.
    /// </summary>
    protected void ConfigureStats()
    {
        Dictionary<Stat, float> calculatedStats = WeaponStatsCalculator.GetStats(WeaponData, Level);

        damage = Mathf.RoundToInt(calculatedStats[Stat.Attack]);
        attackDelay = 1f / calculatedStats[Stat.AttackSpeed];
        criticalChance = Mathf.RoundToInt(calculatedStats[Stat.CriticalChance]);
        criticalPercent = calculatedStats[Stat.CriticalPercent];
        range = calculatedStats[Stat.Range];
    }

    /// <summary>
    /// Upgrades the weapon to a specific level.
    /// </summary>
    public void UpgradeTo(int targetLevel)
    {
        Level = targetLevel;
        ConfigureStats();
    }

    /// <summary>
    /// Calculates the recycle price of the weapon.
    /// </summary>
    public int GetRecyclePrice()
    {
        return WeaponStatsCalculator.GetRecyclePrice(WeaponData, Level);
    }

    /// <summary>
    /// Upgrades weapon by one level.
    /// </summary>
    public void Upgrade() => UpgradeTo(Level + 1);
}
