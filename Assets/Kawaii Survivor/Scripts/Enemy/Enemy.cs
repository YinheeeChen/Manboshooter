using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

[RequireComponent(typeof(EnemyMovement))]
public class MeleeEnemy : MonoBehaviour
{

    [Header("Components")]
    private EnemyMovement movement;

    [Header("Settings")]
    [SerializeField] private int maxHealth;
    private int health;
    [SerializeField] private TextMeshPro healthText;

    [Header("Element")]
    private Player player;

    [Header("Spawn Sequence Related")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private SpriteRenderer spawnIndicator;
    [SerializeField] private Collider2D spawnIndicatorCollider;
    private bool hasSpawned;

    [Header("Effects")]
    [SerializeField] private ParticleSystem passAwayParticles;

    [Header("Attack")]
    [SerializeField] private int damage;
    [SerializeField] private float attackRate;
    [SerializeField] private float playerDetectionRadius;
    private float attackDelay;
    private float attackTimer;

    [Header("Actions")]
    public static Action<int, Vector2> onDamageTaken;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        healthText.text = health.ToString();

        movement = GetComponent<EnemyMovement>();

        player = FindFirstObjectByType<Player>();
        if (player == null)
        {
            Debug.LogError("Player not found in the scene.");
            Destroy(gameObject);
        }

        StartSpawnSequence();

        attackDelay = 1f / attackRate;
    }

    private void StartSpawnSequence()
    {
        // hide the enemy until the spawn sequence is over
        SetRendererVisivility(false);

        // scale up & down the spawn indicator
        Vector3 targetScale = spawnIndicator.transform.localScale * 1.5f;
        LeanTween.scale(spawnIndicator.gameObject, targetScale, 0.5f).setLoopPingPong(4).setOnComplete(showEnemy);
    }

    public void TakeDamage(int damage)
    {
        int realDamage = Mathf.Min(damage, health);
        health -= realDamage; 

        healthText.text = health.ToString();

        onDamageTaken?.Invoke(damage, transform.position);

        if (health <= 0)
        {
            PassAway();
        }
    }

    private void showEnemy()
    {
        SetRendererVisivility(true);
        hasSpawned = true;

        spawnIndicatorCollider.enabled = true;

        movement.StorePlayer(player);
    }

    private void SetRendererVisivility(bool value)
    {
        spriteRenderer.enabled = value;
        spawnIndicator.enabled = !value;
    }

    private void Wait()
    {
        attackTimer += Time.deltaTime;
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
        attackTimer = 0f;
        player.TakeDamage(damage);
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

    // Update is called once per frame
    void Update()
    {
        if (attackTimer >= attackDelay)
            TryAttackPlayer();
        else
            Wait();
            
        if (hasSpawned)
            movement.FollowPlayer();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, playerDetectionRadius);

        Gizmos.color = Color.green;
        
    }
}
