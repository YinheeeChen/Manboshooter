using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

[RequireComponent(typeof(EnemyMovement), typeof(RangeEnemyAttack))]
public class RangeEnemy : Enemy
{
    private RangeEnemyAttack rangeEnemyAttack;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        rangeEnemyAttack = GetComponent<RangeEnemyAttack>();
        rangeEnemyAttack.StorePlayer(player);
    }


    // Update is called once per frame
    void Update()
    {
        if (!CanAttack())
            return;

        ManageAttack();
        
        transform.localScale = player.transform.position.x > transform.position.x ? Vector3.one : Vector3.one.With(x: -1);
    }

    private void TryAttackPlayer()
    {
        rangeEnemyAttack.AutoAim();

    }

    private void ManageAttack()
    {
        if (Vector2.Distance(transform.position, player.transform.position) < playerDetectionRadius)
            TryAttackPlayer();
        else
            movement.FollowPlayer();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, playerDetectionRadius);

        Gizmos.color = Color.green;

    }
}
