using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class RangeEnemyAttack : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Transform shootingPoint;
    [SerializeField] private EnemyBullet bulletPrefab;
    private Player player;

    [Header("Settings")]
    [SerializeField] private int damage;
    [SerializeField] private float attackRate;
    private float attackDelay;
    private float attackTimer;

    [Header("Bullet Pooling")]
    private ObjectPool<EnemyBullet> bulletPool;


    // Start is called before the first frame update
    void Start()
    {
        attackDelay = 1f / attackRate;
        attackTimer = attackDelay;

        bulletPool = new ObjectPool<EnemyBullet>(
            CreateBulletInstance,
            ActionOnGet,
            ActionOnRelease,
            ActionOnDestroy
        );
    }

    // Update is called once per frame
    void Update()
    {

    }

    private EnemyBullet CreateBulletInstance()
    {
        EnemyBullet bullet = Instantiate(bulletPrefab, shootingPoint.position, Quaternion.identity);
        bullet.Configure(this);

        return bullet;
    }

    public void ReleaseBullet(EnemyBullet bullet)
    {
        bulletPool.Release(bullet);
    }

    private void ActionOnGet(EnemyBullet bullet)
    {
        bullet.Reload();
        bullet.transform.position = shootingPoint.position;

        bullet.gameObject.SetActive(true);
    }

    private void ActionOnRelease(EnemyBullet bullet)
    {
        bullet.gameObject.SetActive(false);
    }

    private void ActionOnDestroy(EnemyBullet bullet)
    {
        Destroy(bullet.gameObject);
    }

    public void StorePlayer(Player player)
    {
        this.player = player;
    }



    public void AutoAim()
    {
        // if (player == null)
        // {
        //     player = FindFirstObjectByType<Player>();
        //     if (player == null)
        //     {
        //         Debug.LogError("Player not found in the scene.");
        //         return;
        //     }
        // }

        // Vector2 direction = (player.transform.position - shootingPoint.position).normalized;
        ManageShooting();
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

    Vector2 gizmosDirection;
    private void Shoot()
    {
        Vector2 direction = (player.GetCenter() - (Vector2)shootingPoint.position).normalized;

        EnemyBullet bullet = bulletPool.Get();
        
        bullet.Shoot(damage, direction);
    }

    
}
