using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Represents a boss enemy with its own state machine (Idle, Move, Attack).
/// Inherits from Enemy and displays its health via UI.
/// </summary>
[RequireComponent(typeof(RangeEnemyAttack))]
public class Boss : Enemy
{
    [Header("Elements")]
    [SerializeField] private Slider healthBar;                  // UI slider to show boss health
    [SerializeField] private TextMeshProUGUI healthText;        // Text to display boss health value
    [SerializeField] private Animator animator;                 // Animator for playing animations

    /// <summary>
    /// Enum to define the various AI states the boss can be in.
    /// </summary>
    enum State
    {
        None,
        Idle,
        Attacking,
        Moving
    }

    [Header("State Machine")]
    private State state;                // Current state of the boss
    private float timer;                // Timer used for managing state durations

    [Header("Idle State")]
    [SerializeField] private float maxIdleDuration;   // Max duration the boss can stay idle
    private float idleDuration;                       // Randomly chosen duration for current idle state

    [Header("Moving State")]
    [SerializeField] private float moveSpeed;         // Speed at which the boss moves
    private Vector2 targetPosistion;                  // Target position the boss moves to

    [Header("Attack State")]
    private int attackCounter;                        // Counter to control multi-directional attacks
    private RangeEnemyAttack attack;                  // Component used for ranged attacks

    /// <summary>
    /// Initializes references and subscribes to events.
    /// </summary>
    private void Awake()
    {
        attack = GetComponent<RangeEnemyAttack>();
        state = State.None;
        healthBar.gameObject.SetActive(false);

        onSpawnSequenceCompleted += SpawnSequenceConpletedCallback;
        onDamageTaken += DamageTakenCallback;
    }

    /// <summary>
    /// Unsubscribes from events to avoid memory leaks.
    /// </summary>
    private void OnDestroy()
    {
        onSpawnSequenceCompleted -= SpawnSequenceConpletedCallback;
        onDamageTaken -= DamageTakenCallback;
    }

    /// <summary>
    /// Calls base start logic from Enemy.
    /// </summary>
    protected override void Start()
    {
        base.Start();
    }

    /// <summary>
    /// Updates the boss behavior based on its current state.
    /// </summary>
    void Update()
    {
        ManageStates();
    }

    /// <summary>
    /// Main state machine controller.
    /// </summary>
    private void ManageStates()
    {
        switch (state)
        {
            case State.Idle:
                ManageIdelState();
                break;

            case State.Attacking:
                ManageAttackingState();
                break;

            case State.Moving:
                ManageMovingState();
                break;

            default:
                break;
        }
    }

    /// <summary>
    /// Transitions the boss to the Idle state.
    /// </summary>
    private void SetIdleState()
    {
        state = State.Idle;

        idleDuration = UnityEngine.Random.Range(1f, maxIdleDuration);

        animator.Play("Idle");
    }

    /// <summary>
    /// Controls behavior while boss is in Idle state.
    /// </summary>
    private void ManageIdelState()
    {
        timer += Time.deltaTime;
        if (timer >= idleDuration)
        {
            timer = 0;
            StartMovingState();
        }
    }

    /// <summary>
    /// Transitions the boss to the Moving state and sets a random destination.
    /// </summary>
    private void StartMovingState()
    {
        state = State.Moving;

        targetPosistion = GetRandomPosition();
        animator.Play("Move");
    }

    /// <summary>
    /// Controls behavior while boss is moving toward its target position.
    /// </summary>
    private void ManageMovingState()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPosistion, moveSpeed * Time.deltaTime);
        if (Vector2.Distance(transform.position, targetPosistion) < 0.01f)
            StartAttackingState();
    }

    /// <summary>
    /// Transitions the boss to the Attacking state.
    /// </summary>
    private void StartAttackingState()
    {
        state = State.Attacking;
        attackCounter = 0;
        animator.Play("Attack");
    }

    /// <summary>
    /// Chooses a random position within a third of the arena bounds.
    /// </summary>
    /// <returns>Random position inside arena.</returns>
    private Vector2 GetRandomPosition()
    {
        Vector2 targetPosition = Vector2.zero;
        targetPosition.x = UnityEngine.Random.Range(-Constants.arenaSize.x / 3, Constants.arenaSize.x / 3);
        targetPosition.y = UnityEngine.Random.Range(-Constants.arenaSize.y / 3, Constants.arenaSize.y / 3);

        return targetPosition;
    }

    /// <summary>
    /// Controls behavior during the Attacking state.
    /// Currently empty – should include attack patterns.
    /// </summary>
    private void ManageAttackingState()
    {
        // Intentionally left blank – override with attack behavior if needed
    }

    /// <summary>
    /// Performs a single radial attack in a rotated direction.
    /// </summary>
    private void Attack()
    {
        Vector2 direction = Quaternion.Euler(0, 0, -45 * attackCounter) * Vector2.up;
        attack.InstantShoot(direction);
        attackCounter++;
    }

    /// <summary>
    /// Callback triggered when spawn animation or sequence completes.
    /// Shows health bar and initiates AI behavior.
    /// </summary>
    private void SpawnSequenceConpletedCallback()
    {
        healthBar.gameObject.SetActive(true);
        UpdateHealthBar();

        SetIdleState();
    }

    /// <summary>
    /// Updates the boss health UI.
    /// </summary>
    private void UpdateHealthBar()
    {
        healthBar.value = (float)health / maxHealth;
        healthText.text = $"{health} / {maxHealth}";
    }

    /// <summary>
    /// Overrides base death behavior to notify game manager or system of boss defeat.
    /// </summary>
    public override void PassAway()
    {
        onBossPassedAway?.Invoke(transform.position);
        PassAwayAfterWave();
    }

    /// <summary>
    /// Callback for when the boss takes damage; updates UI.
    /// </summary>
    /// <param name="damage">Amount of damage taken.</param>
    /// <param name="posistion">Position where the damage occurred.</param>
    /// <param name="isCritical">Whether the damage was a critical hit.</param>
    private void DamageTakenCallback(int damage, Vector2 posistion, bool isCritical)
    {
        UpdateHealthBar();
    }
}
