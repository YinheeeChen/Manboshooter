using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour, IPlayerStatDependency
{
    [field: SerializeField] public WeaponDataSO WeaponData { get; private set; }
    [Header("Settings")]
    [SerializeField] protected float range;
    [SerializeField] protected LayerMask enemyMask;

    [Header("Attack")]
    [SerializeField] protected int damage;
    [SerializeField] protected float attackDelay;
    [SerializeField] protected Animator animator;
    protected float attackTimer;

    [Header("Critical Hit")]
    protected int criticalChance;
    protected float criticalPercent;

    [Header("Aiming")]
    [SerializeField] protected float aimLerp;

    [Header("Level")]
    [field: SerializeField] public int Level { get; private set; }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    protected Enemy GetClosestEnemy()
    {
        Enemy closestEnemy = null;
        Vector2 targetUpVector = Vector3.up;
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, range, enemyMask);

        if (enemies.Length <= 0)
        {
            transform.up = targetUpVector;
            return null;
        }

        // Find the closest enemy
        float closestDistance = range;

        for (int i = 0; i < enemies.Length; i++)
        {
            Enemy enemyChecked = enemies[i].GetComponent<Enemy>();

            float distance = Vector2.Distance(transform.position, enemyChecked.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemyChecked;
            }
        }

        return closestEnemy;

    }

    protected int GetDamage(out bool isCriticalHit)
    {
        isCriticalHit = false;

        if (Random.Range(0, 101) <= criticalChance) // 10% chance for a critical hit
        {
            isCriticalHit = true;
            return Mathf.RoundToInt(damage * criticalPercent);
        }

        return damage; // Normal damage

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    public abstract void UpdateStats(PlayerStatManager playerStatManager);

    protected void ConfigureStats()
    {
        float multiplier = 1 + (float)Level / 3;
        damage = Mathf.RoundToInt(WeaponData.GetStatValue(Stat.Attack) * multiplier);
        attackDelay = 1f / (WeaponData.GetStatValue(Stat.AttackSpeed) * multiplier);

        criticalChance = Mathf.RoundToInt(WeaponData.GetStatValue(Stat.CriticalChance) * multiplier);
        criticalPercent = WeaponData.GetStatValue(Stat.CriticalPercent) * multiplier;

        if(WeaponData.Prefab.GetType() == typeof(RangeWeapon))
            range = WeaponData.GetStatValue(Stat.Range) * multiplier;
    }

}
