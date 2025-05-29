using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeWeapon : Weapon
{
    [Header("Elements")]
    [SerializeField] private Bullet bullet;
    [SerializeField] private Transform shootingPoint;

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
            transform.up = Vector3.Lerp(transform.up, targetUpVector, Time.deltaTime * aimLerp);
            ManageShooting();
            return;
        }

        targetUpVector = (closestEnemy.transform.position - transform.position).normalized;
        transform.up = targetUpVector;

        

    }

    private void ManageShooting()
    {
        attackTimer += Time.deltaTime;

        if (attackTimer >= attackDelay)
        {
            Shoot();
            attackTimer = 0f;
        }

    }
    

    private void Shoot()
    {
        Bullet bulletInstance = Instantiate(bullet, shootingPoint.position, Quaternion.identity);
        bulletInstance.Shoot(damage, transform.up);
    }
}
