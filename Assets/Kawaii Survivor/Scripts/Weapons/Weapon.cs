using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Transform enemy;


    [Header("Settings")]
    [SerializeField] private float range;
    [SerializeField] private LayerMask enemyMask;

    [Header("Aiming")]
    [SerializeField] private float aimLerp;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        AutoAim();
    }

    private void AutoAim()
    {
        Enemy closestEnemy = GetClosestEnemy();

        Vector2 targetUpVector = Vector3.up;

        if (closestEnemy != null)
        {
            targetUpVector = (enemy.transform.position - transform.position).normalized;
        }

        transform.up = Vector3.Lerp(transform.up, targetUpVector, Time.deltaTime * aimLerp);

    }

    private Enemy GetClosestEnemy()
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
