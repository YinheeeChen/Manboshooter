using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Elements")]
    private Rigidbody2D rig;
    private Collider2D col;
    private RangeWeapon rangeWeapon;

    [Header("Settings")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private LayerMask enemyMask;
    private int damage;
    private Enemy target;

    private void Awake()
    {
        rig = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

        // LeanTween.delayedCall(gameObject, 5f, () => rangeEnemyAttack.ReleaseBullet(this)); // Destroy the bullet after 5 seconds if it doesn't hit anything
        // StartCoroutine(ReleasCoroutine());
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Configure(RangeWeapon rangeWeapon)
    {
        this.rangeWeapon = rangeWeapon;
    }

    public void Shoot(int damage, Vector2 direction)
    {
        Invoke("Release", 1); // Automatically release the bullet after 5 seconds if it doesn't hit anything
        this.damage = damage;

        transform.right = direction;
        rig.velocity = direction * speed; // Set bullet speed, adjust as needed
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (target != null) return; // If the bullet has already hit a target, ignore further collisions

        if (IsInLayerMask(collider.gameObject.layer, enemyMask))
        {
            target = collider.GetComponent<Enemy>();

            CancelInvoke();

            Attack(target);
            Release();
        }
    }

    private bool IsInLayerMask(int layer, LayerMask layerMask)
    {
        return (layerMask.value & (1 << layer)) != 0;
    }

    private void Attack(Enemy enemy)
    {
        enemy.TakeDamage(damage);
    }

    public void Reload()
    {
        target = null; // Reset target to allow the bullet to hit a new enemy
        rig.velocity = Vector2.zero; // Reset velocity
        col.enabled = true; // Enable the collider for the next use
    }
    
    private void Release()
    {
        if(!gameObject.activeSelf) return; // Check if the bullet is still active
        rangeWeapon.ReleaseBullet(this);
    }
}
