using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Elements")]
    private Rigidbody2D rig;
    private Collider2D col;

    [Header("Settings")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private LayerMask enemyMask;
    private int damage;

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

    public void Shoot(int damage, Vector2 direction)
    {
        this.damage = damage;

        transform.right = direction;
        rig.velocity = direction * speed; // Set bullet speed, adjust as needed
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (IsInLayerMask(collider.gameObject.layer, enemyMask))
        {
            Attack(collider.GetComponent<Enemy>()); 
            Destroy(gameObject); 

            if (collider.TryGetComponent(out Enemy enemy))
            {
                enemy.TakeDamage(damage);
            }
            else if (collider.TryGetComponent(out Player player))
            {
                player.TakeDamage(damage);
            }

            Destroy(gameObject); // Destroy the bullet on hit
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
}
