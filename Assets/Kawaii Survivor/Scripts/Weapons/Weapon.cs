using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    enum State
    {
        Idle,
        Attacking
    }

    private State state;

    [Header("Elements")]
    [SerializeField] private Transform hitDetectionTransform;
    [SerializeField] private BoxCollider2D hitDetectionBoxCollider;
    [SerializeField] private float hitDetectionRadius;

    [Header("Settings")]
    [SerializeField] private float range;
    [SerializeField] private LayerMask enemyMask;

    [Header("Attack")]
    [SerializeField] private int damage;
    [SerializeField] private float attackDelay;
    [SerializeField] private Animator animator;

    private float attackTimer;
    private List<MeleeEnemy> damagedEnemies = new List<MeleeEnemy>();

    [Header("Aiming")]
    [SerializeField] private float aimLerp;

    // Start is called before the first frame update
    void Start()
    {
        state = State.Idle;

    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.Idle:
                AutoAim();
                break;
            case State.Attacking:
                Attacking();
                break;
        }
    }

    private void AutoAim()
    {
        MeleeEnemy closestEnemy = GetClosestEnemy();

        Vector2 targetUpVector = Vector3.up;

        if (closestEnemy != null)
        {
            targetUpVector = (closestEnemy.transform.position - transform.position).normalized;
            transform.up = targetUpVector;
            MangeAttack();
        }

        transform.up = Vector3.Lerp(transform.up, targetUpVector, Time.deltaTime * aimLerp);

        IncrementAttackTimer();

    }

    private void MangeAttack()
    {
        attackTimer += Time.deltaTime;

        if (attackTimer >= attackDelay)
        {
            attackTimer = 0f;
            StartAttack();
        }
    }

    private void IncrementAttackTimer()
    {
        attackTimer += Time.deltaTime;
    }

    [NaughtyAttributes.Button]
    private void StartAttack()
    {
        animator.Play("Attack");
        state = State.Attacking;

        damagedEnemies.Clear();
        animator.speed = 1f / attackDelay;
    }

    private void Attacking()
    {
        Attack();
    }


    private void StopAttack()
    {
        state = State.Idle;
        damagedEnemies.Clear();
    }
        

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
            MeleeEnemy enemy = enemies[i].GetComponent<MeleeEnemy>();
            if (!damagedEnemies.Contains(enemy))
            {
                enemy.TakeDamage(damage);
                damagedEnemies.Add(enemy);
            }
        }
    }

    private MeleeEnemy GetClosestEnemy()
    {
        MeleeEnemy closestEnemy = null;
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
            MeleeEnemy enemyChecked = enemies[i].GetComponent<MeleeEnemy>();

            float distance = Vector2.Distance(transform.position, enemyChecked.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemyChecked;
            }
        }

        return closestEnemy;

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(hitDetectionTransform.position, hitDetectionRadius);
    }
}
