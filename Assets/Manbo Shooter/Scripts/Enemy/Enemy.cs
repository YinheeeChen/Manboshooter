using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Base abstract class for all enemy types.
/// Handles core enemy functionality such as health, spawn sequence, player detection, and death effects.
/// </summary>
public abstract class Enemy : MonoBehaviour
{
    [Header("Components")]
    protected EnemyMovement movement;                          // Reference to enemy movement logic

    [Header("Settings")]
    [SerializeField] protected int maxHealth;                  // Maximum health of the enemy
    protected int health;                                      // Current health of the enemy

    [Header("Element")]
    protected Player player;                                   // Reference to the player instance

    [Header("Spawn Sequence Related")]
    [SerializeField] protected SpriteRenderer spriteRenderer;         // Main sprite renderer for the enemy
    [SerializeField] protected SpriteRenderer spawnIndicator;         // Spawn animation indicator
    [SerializeField] protected Collider2D spawnIndicatorCollider;     // Collider used during spawn sequence
    protected bool hasSpawned;                                         // Flag indicating if the spawn sequence is complete

    [Header("Attack")]
    [SerializeField] protected float playerDetectionRadius;    // Radius for detecting the player

    [Header("Effects")]
    [SerializeField] protected ParticleSystem passAwayParticles; // Particle effect for when the enemy dies

    [Header("Actions")]
    public static Action<int, Vector2, bool> onDamageTaken;      // Called when any enemy takes damage
    public static Action<Vector2> onPassedAway;                  // Called when a regular enemy dies
    public static Action<Vector2> onBossPassedAway;              // Called when a boss enemy dies
    protected Action onSpawnSequenceCompleted;                   // Local callback for end of spawn sequence

    /// <summary>
    /// Initializes core enemy properties and starts the spawn sequence.
    /// </summary>
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

    /// <summary>
    /// Determines whether the enemy can currently attack.
    /// </summary>
    /// <returns>True if the enemy is visible and active; otherwise false.</returns>
    protected bool CanAttack()
    {
        return spriteRenderer.enabled;
    }

    /// <summary>
    /// Sets the visibility of the enemy and spawn indicator.
    /// </summary>
    /// <param name="value">If true, enemy is visible; otherwise spawn indicator is shown.</param>
    private void SetRendererVisivility(bool value)
    {
        spriteRenderer.enabled = value;
        spawnIndicator.enabled = !value;
    }

    /// <summary>
    /// Called after the spawn animation finishes to activate the enemy.
    /// </summary>
    private void showEnemy()
    {
        SetRendererVisivility(true);
        hasSpawned = true;

        spawnIndicatorCollider.enabled = true;

        if (movement != null)
            movement.StorePlayer(player);

        onSpawnSequenceCompleted?.Invoke();
    }

    /// <summary>
    /// Starts the spawn animation sequence using LeanTween.
    /// </summary>
    private void StartSpawnSequence()
    {
        // Hide the enemy during spawn
        SetRendererVisivility(false);

        // Animate spawn indicator scaling
        Vector3 targetScale = spawnIndicator.transform.localScale * 1.5f;
        LeanTween.scale(spawnIndicator.gameObject, targetScale, 0.5f)
                 .setLoopPingPong(2)
                 .setOnComplete(showEnemy);
    }

    /// <summary>
    /// Called when the enemy dies. Triggers pass away event and cleanup.
    /// </summary>
    public virtual void PassAway()
    {
        onPassedAway?.Invoke(transform.position);
        PassAwayAfterWave();
    }

    /// <summary>
    /// Handles death effects and destruction of the enemy object.
    /// </summary>
    public void PassAwayAfterWave()
    {
        passAwayParticles.transform.SetParent(null);  // Detach particle system so it can finish playing
        passAwayParticles.Play();
        Destroy(gameObject);
    }

    /// <summary>
    /// Applies damage to the enemy, triggers effects, and checks for death.
    /// </summary>
    /// <param name="damage">The amount of damage dealt.</param>
    /// <param name="isCriticalHit">Whether the damage is a critical hit.</param>
    public void TakeDamage(int damage, bool isCriticalHit)
    {
        int realDamage = Mathf.Min(damage, health);
        health -= realDamage;

        onDamageTaken?.Invoke(damage, transform.position, isCriticalHit);

        if (health <= 0)
        {
            PassAway();
        }
    }

    /// <summary>
    /// Returns the center position of the enemy, adjusted by the spawn indicator's offset.
    /// </summary>
    /// <returns>Enemy center position as Vector2.</returns>
    public Vector2 GetCenter()
    {
        return (Vector2)transform.position + spawnIndicatorCollider.offset;
    }
}
