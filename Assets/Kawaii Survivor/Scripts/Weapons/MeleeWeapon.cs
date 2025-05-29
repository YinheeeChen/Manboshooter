using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : Weapon
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

    [Header("Settings")]
    private List<Enemy> damagedEnemies = new List<Enemy>();

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
        Enemy closestEnemy = GetClosestEnemy();

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
            Enemy enemy = enemies[i].GetComponent<Enemy>();
            if (!damagedEnemies.Contains(enemy))
            {
                enemy.TakeDamage(damage);
                damagedEnemies.Add(enemy);
            }
        }
    }

}
