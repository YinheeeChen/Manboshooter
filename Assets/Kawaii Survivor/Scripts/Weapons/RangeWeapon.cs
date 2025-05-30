using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class RangeWeapon : Weapon
{
    [Header("Elements")]
    [SerializeField] private Bullet bullet;
    [SerializeField] private Transform shootingPoint;

    [Header("Pooling")]
    private ObjectPool<Bullet> bulletPool;

    // Start is called before the first frame update
    void Start()
    {
        bulletPool = new ObjectPool<Bullet>(
            CreateBulletInstance,
            ActionOnGet,
            ActionOnRelease,
            ActionOnDestroy
        );
    }

    // Update is called once per frame
    void Update()
    {
        AutoAim();
    }


    private Bullet CreateBulletInstance()
    {
        Bullet bullet = Instantiate(this.bullet, shootingPoint.position, Quaternion.identity);
        bullet.Configure(this);

        return bullet;
    }

    public void ReleaseBullet(Bullet bullet)
    {
        bulletPool.Release(bullet);
    }

    private void ActionOnGet(Bullet bullet)
    {
        bullet.Reload();
        bullet.transform.position = shootingPoint.position;

        bullet.gameObject.SetActive(true);
    }

    private void ActionOnRelease(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
    }

    private void ActionOnDestroy(Bullet bullet)
    {
        Destroy(bullet.gameObject);
    }

    private void AutoAim()
    {
        Enemy closestEnemy = GetClosestEnemy();

        Vector2 targetUpVector = Vector3.up;

        if (closestEnemy != null)
        {
            targetUpVector = (closestEnemy.transform.position - transform.position).normalized;
            transform.up = targetUpVector;
            ManageShooting();
            return;
        }

        transform.up = Vector3.Lerp(transform.up, targetUpVector, Time.deltaTime * aimLerp);

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
        int damage = GetDamage(out bool isCriticalHit);

        Bullet bulletInstance = bulletPool.Get();
        bulletInstance.Shoot(damage, transform.up, isCriticalHit);
    }
}
