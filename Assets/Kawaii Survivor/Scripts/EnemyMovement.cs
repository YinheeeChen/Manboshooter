using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Element")]
    private Player player;

    [Header("Spawn Sequence Related")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private SpriteRenderer spawnIndicator;
    private bool hasSpawned;


    [Header("Settings")]
    [SerializeField] private float speed;
    [SerializeField] private float playerDetectionRadius;

    [Header("Effects")]
    [SerializeField] private ParticleSystem passAwayParticles;

    [Header("Attack")]
    [SerializeField] private int damage;
    [SerializeField] private float attackRate;
    private float attackDelay;
    private float attackTimer;

    // Start is called before the first frame update
    void Start()
    {
        player = FindFirstObjectByType<Player>();
        if (player == null)
        {
            Debug.LogError("Player not found in the scene.");
            Destroy(gameObject);
        }

        // hide the enemy until the spawn sequence is over
        spriteRenderer.enabled = false;
        spawnIndicator.enabled = true;

        // scale up & down the spawn indicator
        Vector3 targetScale = spawnIndicator.transform.localScale * 1.5f;
        LeanTween.scale(spawnIndicator.gameObject, targetScale, 0.5f).setLoopPingPong(4).setOnComplete(showEnemy);

        attackDelay = 1f / attackRate;

        // prevent the indictor from moving to the player
    }

    // Update is called once per frame
    void Update()
    {
        if(!hasSpawned) return;

        FollowPlayer();

        if(attackTimer >= attackDelay)
            TryAttackPlayer();
        else
            Wait();
    }

    private void Wait()
    {
        attackTimer += Time.deltaTime;
    }

    private void showEnemy()
    {
        spriteRenderer.enabled = true;
        spawnIndicator.enabled = false;
        hasSpawned = true;
    }

    private void FollowPlayer()
    {
        Vector2 direction = (player.transform.position - transform.position).normalized;
        Vector2 targetPosition = (Vector2)transform.position + speed * direction * Time.deltaTime;
        transform.position = targetPosition;
    }

    private void TryAttackPlayer()
    {
        if (Vector2.Distance(transform.position, player.transform.position) < playerDetectionRadius)
        {
            // Attack logic here
            Attack();
        }
        
    }

    private void Attack()
    {
        Debug.Log("Dealing " + damage + " damage to player.");
        attackTimer = 0f;
    }

    private void PassAway()
    {
        if (passAwayParticles != null)
        {
            passAwayParticles.transform.SetParent(null);
            passAwayParticles.Play();
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, playerDetectionRadius);
    }

}
