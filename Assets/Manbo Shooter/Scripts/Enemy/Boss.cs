using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RangeEnemyAttack))]
public class Boss : Enemy
{
    [Header("Elements")]
    [SerializeField] private Slider healthBar;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private Animator animator;

    enum State
    {
        None,
        Idle,
        Attacking,
        Moving
    }

    [Header("State Machine")]
    private State state;
    private float timer;

    [Header("Idle State")]
    [SerializeField] private float maxIdleDuration;
    private float idleDuration;

    [Header("Moving State")]
    [SerializeField] private float moveSpeed;
    private Vector2 targetPosistion;

    [Header("Attack State")]
    private int attackCounter;
    private RangeEnemyAttack attack;

    private void Awake()
    {
        attack = GetComponent<RangeEnemyAttack>();
        state = State.None;
        healthBar.gameObject.SetActive(false);

        onSpawnSequenceCompleted += SpawnSequenceConpletedCallback;
        onDamageTaken += DamageTakenCallback;
    }


    private void OnDestroy()
    {
        onSpawnSequenceCompleted -= SpawnSequenceConpletedCallback;
        onDamageTaken -= DamageTakenCallback;
    }


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        ManageStates();
    }

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

    private void SetIdleState()
    {
        state = State.Idle;

        idleDuration = UnityEngine.Random.Range(1f, maxIdleDuration);

        animator.Play("Idle");
    }

    private void ManageIdelState()
    {
        timer += Time.deltaTime;
        if (timer >= idleDuration)
        {
            timer = 0;
            StartMovingState();
        }
    }

    private void StartMovingState()
    {
        state = State.Moving;

        targetPosistion = GetRandomPosition();
        animator.Play("Move");
    }

    private void ManageMovingState()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPosistion, moveSpeed * Time.deltaTime);
        if (Vector2.Distance(transform.position, targetPosistion) < 0.01f)
            StartAttackingState();
    }

    private void StartAttackingState()
    {
        state = State.Attacking;
        attackCounter = 0;
        animator.Play("Attack");
    }

    private Vector2 GetRandomPosition()
    {
        Vector2 targetPosition = Vector2.zero;
        targetPosition.x = UnityEngine.Random.Range(-Constants.arenaSize.x / 3, Constants.arenaSize.x / 3);
        targetPosition.y = UnityEngine.Random.Range(-Constants.arenaSize.y / 3, Constants.arenaSize.y / 3);

        return targetPosition;
    }

    private void ManageAttackingState()
    {

    }

    private void Attack()
    {
        Vector2 direction = Quaternion.Euler(0, 0, -45 * attackCounter) * Vector2.up;
        attack.InstantShoot(direction);
        attackCounter++;
    }

    private void SpawnSequenceConpletedCallback()
    {
        healthBar.gameObject.SetActive(true);
        UpdateHealthBar();

        SetIdleState();
    }

    private void UpdateHealthBar()
    {
        healthBar.value = (float)health / maxHealth;
        healthText.text = $"{health} / {maxHealth}";
    }

    public override void PassAway()
    {
        onBossPassedAway?.Invoke(transform.position);
        PassAwayAfterWave();
    }

    private void DamageTakenCallback(int damage, Vector2 posistion, bool isCritical)
    {
        UpdateHealthBar();
    }

}
