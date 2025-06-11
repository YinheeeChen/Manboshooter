using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

/// <summary>
/// Manages the drop system for defeated enemies, including candy, cash, and chests.
/// Utilizes object pooling for candy and cash to improve performance.
/// </summary>
public class DropManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Candy candyPrefab;   // Prefab for candy drops
    [SerializeField] private Cash cashPrefab;     // Prefab for cash drops
    [SerializeField] private Chest chestPrefab;   // Prefab for chest drops

    [Header("Settings")]
    [SerializeField][Range(0, 100)] private int cashDropChance;   // Chance (%) to drop cash instead of candy
    [SerializeField][Range(0, 100)] private int chestDropChance;  // Chance (%) to drop a chest on enemy death

    [Header("Pooling")]
    private ObjectPool<Candy> candyPool;  // Object pool for candy
    private ObjectPool<Cash> cashPool;    // Object pool for cash

    /// <summary>
    /// Subscribes to events and initializes drop collection callbacks.
    /// </summary>
    private void Awake()
    {
        Enemy.onPassedAway += EnemyPassedAwayCallback;
        Enemy.onBossPassedAway += BossEnemyPassedAwayCallback;
        Candy.onCollected += RelaeaseCandy;
        Cash.onCollected += RelaeaseCash;
    }

    /// <summary>
    /// Unsubscribes from events to avoid memory leaks.
    /// </summary>
    private void OnDestroy()
    {
        Enemy.onPassedAway -= EnemyPassedAwayCallback;
        Enemy.onBossPassedAway -= BossEnemyPassedAwayCallback;
        Candy.onCollected -= RelaeaseCandy;
        Cash.onCollected -= RelaeaseCash;
    }

    /// <summary>
    /// Initializes the object pools for candy and cash.
    /// </summary>
    private void Start()
    {
        candyPool = new ObjectPool<Candy>(
            CandyCreateFunction,
            CandyActionOnGet,
            CandyActionOnRelease,
            CandyActionOnDestory
        );

        cashPool = new ObjectPool<Cash>(
            CashCreateFunction,
            CashActionOnGet,
            CashActionOnRelease,
            CashActionOnDestory
        );
    }

    private void Update()
    {
        // No update logic needed currently
    }

    // ----------- Candy Pool Methods -----------

    private Candy CandyCreateFunction() => Instantiate(candyPrefab, transform);        // Create new candy instance
    private void CandyActionOnGet(Candy candy) => candy.gameObject.SetActive(true);    // Activate candy on get
    private void CandyActionOnRelease(Candy candy) => candy.gameObject.SetActive(false); // Deactivate candy on release
    private void CandyActionOnDestory(Candy candy) => Destroy(candy.gameObject);       // Destroy candy on disposal

    // ----------- Cash Pool Methods -----------

    private Cash CashCreateFunction() => Instantiate(cashPrefab, transform);           // Create new cash instance
    private void CashActionOnGet(Cash cash) => cash.gameObject.SetActive(true);        // Activate cash on get
    private void CashActionOnRelease(Cash cash) => cash.gameObject.SetActive(false);   // Deactivate cash on release
    private void CashActionOnDestory(Cash cash) => Destroy(cash.gameObject);           // Destroy cash on disposal

    /// <summary>
    /// Handles drop logic when a regular enemy dies.
    /// </summary>
    /// <param name="enemyPosition">The position where the enemy died.</param>
    private void EnemyPassedAwayCallback(Vector2 enemyPosition)
    {
        // Decide whether to drop cash or candy
        bool shouldSpawnCash = Random.Range(0, 101) <= cashDropChance;

        DroppableCurrency droppable = shouldSpawnCash ? cashPool.Get() : candyPool.Get();
        droppable.transform.position = enemyPosition;

        TryDropChest(enemyPosition);
    }

    /// <summary>
    /// Always drops a chest when a boss enemy dies.
    /// </summary>
    /// <param name="enemyPosition">The position where the boss died.</param>
    private void BossEnemyPassedAwayCallback(Vector2 enemyPosition) => DropChest(enemyPosition);

    /// <summary>
    /// Attempts to drop a chest based on the configured chest drop chance.
    /// </summary>
    /// <param name="spawnPosition">The position to potentially drop a chest.</param>
    private void TryDropChest(Vector2 spawnPosition)
    {
        bool shouldDropChest = Random.Range(0, 101) <= chestDropChance;

        if (!shouldDropChest) return;

        DropChest(spawnPosition);
    }

    /// <summary>
    /// Instantiates a chest at the specified position.
    /// </summary>
    /// <param name="spawnPosition">The position to drop the chest.</param>
    private void DropChest(Vector2 spawnPosition) =>
        Instantiate(chestPrefab, spawnPosition, Quaternion.identity, transform);

    /// <summary>
    /// Returns the collected candy object to the pool.
    /// </summary>
    /// <param name="candy">The collected candy object.</param>
    private void RelaeaseCandy(Candy candy) => candyPool.Release(candy);

    /// <summary>
    /// Returns the collected cash object to the pool.
    /// </summary>
    /// <param name="cash">The collected cash object.</param>
    private void RelaeaseCash(Cash cash) => cashPool.Release(cash);
}
