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
    new void Start()
    {
        base.Start();
        rangeEnemyAttack = GetComponent<RangeEnemyAttack>();
        rangeEnemyAttack.StorePlayer(player);
    }


    // Update is called once per frame
    void Update()
    {
        if (!spriteRenderer.enabled)
            return;

        ManageAttack();
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
