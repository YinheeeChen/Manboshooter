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
    [SerializeField] private float angularSpeed;
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
        // Debug.Log("Shooting bullet with damage: " + damage + " in direction: " + direction);

        if (Mathf.Abs(direction.x + 1) < 0.01f)
            direction.y += 0.1f;

        transform.right = direction;
        rig.velocity = direction * speed;
        rig.angularVelocity = angularSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.TryGetComponent(out Player player))
        {
            LeanTween.cancel(gameObject);

            player.TakeDamage(damage); 
            col.enabled = false; 
            rangeEnemyAttack.ReleaseBullet(this);
        }

    }

    public void Reload()
    {
        rig.velocity = Vector2.zero;
        rig.angularVelocity = 0;
        col.enabled = true;

        LeanTween.cancel(gameObject);
        LeanTween.delayedCall(gameObject, 5f, () => rangeEnemyAttack.ReleaseBullet(this));
    }
}
