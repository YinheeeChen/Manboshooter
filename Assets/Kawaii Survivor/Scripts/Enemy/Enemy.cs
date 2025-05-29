using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Enemy : MonoBehaviour
{
    [Header("Components")]
    protected EnemyMovement movement;

    [Header("Settings")]
    [SerializeField] protected int maxHealth;
    protected int health;

    [Header("Element")]
    protected Player player;

    [Header("Spawn Sequence Related")]
    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] protected SpriteRenderer spawnIndicator;
    [SerializeField] protected Collider2D spawnIndicatorCollider;
    protected bool hasSpawned;

    [Header("Attack")]
    [SerializeField] protected float playerDetectionRadius;

    [Header("Effects")]
    [SerializeField] protected ParticleSystem passAwayParticles;

    [Header("Actions")]
    public static Action<int, Vector2> onDamageTaken;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        health = maxHealth;
        movement = GetComponent<EnemyMovement>();
        player = FindFirstObjectByType<Player>();

        if (player == null)
        {
            Debug.LogError("Player not found in the scene.");
            Destroy(gameObject);
        }

        StartSpawnSequence();

    }

    // Update is called once per frame
    protected bool CanAttack()
    {
        return spriteRenderer.enabled;
    }

    private void SetRendererVisivility(bool value)
    {
        spriteRenderer.enabled = value;
        spawnIndicator.enabled = !value;
    }

    private void showEnemy()
    {
        SetRendererVisivility(true);
        hasSpawned = true;

        spawnIndicatorCollider.enabled = true;

        movement.StorePlayer(player);
    }

    private void StartSpawnSequence()
    {
        // hide the enemy until the spawn sequence is over
        SetRendererVisivility(false);

        // scale up & down the spawn indicator
        Vector3 targetScale = spawnIndicator.transform.localScale * 1.5f;
        LeanTween.scale(spawnIndicator.gameObject, targetScale, 0.5f).setLoopPingPong(4).setOnComplete(showEnemy);
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

    public void TakeDamage(int damage)
    {
        int realDamage = Mathf.Min(damage, health);
        health -= realDamage;

        onDamageTaken?.Invoke(damage, transform.position);

        if (health <= 0)
        {
            PassAway();
        }
    }
}
