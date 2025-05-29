using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

[RequireComponent(typeof(EnemyMovement))]
public class MeleeEnemy : Enemy
{

    [Header("Attack")]
    [SerializeField] private int damage;
    [SerializeField] private float attackRate;
    private float attackDelay;
    private float attackTimer;


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        attackDelay = 1f / attackRate;
    }

    // Update is called once per frame
    void Update()
    {
        if (attackTimer >= attackDelay)
            TryAttackPlayer();
        else
            Wait();

        if (hasSpawned)
            movement.FollowPlayer();
    }

    private void Wait()
    {
        attackTimer += Time.deltaTime;
    }

    private void Attack()
    {
        attackTimer = 0f;
        player.TakeDamage(damage);
    }

    private void TryAttackPlayer()
    {
        if (Vector2.Distance(transform.position, player.transform.position) < playerDetectionRadius)
        {
            // Attack logic here
            Attack();
        }

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, playerDetectionRadius);

        Gizmos.color = Color.green;

    }
}
