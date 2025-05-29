using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class EnemyBullet : MonoBehaviour
{
    [Header("Elements")]
    private Rigidbody2D rig;
    private Collider2D col;
    private RangeEnemyAttack rangeEnemyAttack;

    [Header("Settings")]
    [SerializeField] private float speed = 5f;
    private int damage;


    private void Awake()
    {
        rig = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

        LeanTween.delayedCall(gameObject, 5f, () => rangeEnemyAttack.ReleaseBullet(this)); // Destroy the bullet after 5 seconds if it doesn't hit anything
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

    IEnumerator ReleasCoroutine()
    {
        yield return new WaitForSeconds(5f); // Wait for 5 seconds before releasing the bullet
        rangeEnemyAttack.ReleaseBullet(this);
    }

    public void Configure(RangeEnemyAttack rangeEnemyAttack)
    {
        this.rangeEnemyAttack = rangeEnemyAttack;
    }

    public void Shoot(int damage, Vector2 direction)
    {
        this.damage = damage;

        transform.right = direction;
        rig.velocity = direction * speed; // Set bullet speed, adjust as needed
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.TryGetComponent(out Player player))
        {
            LeanTween.cancel(gameObject); // Cancel any existing tweens on the bullet
            // StopCoroutine(ReleasCoroutine()); // Stop the release coroutine
            // StopAllCoroutines(); // Stop all coroutines to prevent further actions

            player.TakeDamage(1); // Assuming the bullet deals 1 damage
            col.enabled = false; // Disable the collider to prevent multiple hits

            rangeEnemyAttack.ReleaseBullet(this);
        }
        else if (collider.TryGetComponent(out MeleeEnemy enemy))
        {
            // If the bullet hits another enemy, you can decide to destroy it or handle it differently
            Destroy(gameObject); // Destroy the bullet if it hits another enemy
        }
        else if (collider.CompareTag("Wall"))
        {
            // If the bullet hits a wall, destroy it
            Destroy(gameObject);
        }

    }

    public void Reload()
    {
        rig.velocity = Vector2.zero; // Reset velocity
        col.enabled = true; // Enable the collider for the next use
    }
}
