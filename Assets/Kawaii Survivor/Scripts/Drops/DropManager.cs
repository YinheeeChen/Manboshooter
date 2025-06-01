using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class DropManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Candy candyPrefab;
    [SerializeField] private Cash cashPrefab;
    [SerializeField] private Chest chestPrefab;

    [Header("Settings")]
    [SerializeField] [Range(0,100)]private int cashDropChance;
    [SerializeField] [Range(0,100)]private int chestDropChance;

    [Header("Pooling")]
    private ObjectPool<Candy> candyPool;
    private ObjectPool<Cash> cashPool;

    private void Awake()
    {
        Enemy.onPassedAway += EnemyPassedAwayCallback;
        Candy.onCollected += RelaeaseCandy;
        Cash.onCollected += RelaeaseCash;
    }

    private void OnDestroy()
    {
        Enemy.onPassedAway -= EnemyPassedAwayCallback;
        Candy.onCollected -= RelaeaseCandy;
        Cash.onCollected -= RelaeaseCash;
    }
    // Start is called before the first frame update
    void Start()
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

    // Update is called once per frame
    void Update()
    {

    }

    private Candy CandyCreateFunction() => Instantiate(candyPrefab, transform);
    private void CandyActionOnGet(Candy candy) => candy.gameObject.SetActive(true);
    private void CandyActionOnRelease(Candy candy) => candy.gameObject.SetActive(false);
    private void CandyActionOnDestory(Candy candy) => Destroy(candy.gameObject);

    private Cash CashCreateFunction() => Instantiate(cashPrefab, transform);
    private void CashActionOnGet(Cash cash) => cash.gameObject.SetActive(true);
    private void CashActionOnRelease(Cash cash) => cash.gameObject.SetActive(false);
    private void CashActionOnDestory(Cash cash) => Destroy(cash.gameObject);

    private void EnemyPassedAwayCallback(Vector2 enemyPosition)
    {
        bool shouldSpawnCash = Random.Range(0, 101) <= cashDropChance;

        DroppableCurrency droppable = shouldSpawnCash ? cashPool.Get() : candyPool.Get();
        droppable.transform.position = enemyPosition;

        TryDropChest(enemyPosition);
    }

    private void TryDropChest(Vector2 spawnPosition)
    {
        bool shouldDropChest = Random.Range(0, 101) <= chestDropChance;

        if (!shouldDropChest) return;

        Instantiate(chestPrefab, spawnPosition, Quaternion.identity, transform);
    }

    private void RelaeaseCandy(Candy candy) => candyPool.Release(candy);
    private void RelaeaseCash(Cash cash) => cashPool.Release(cash);


}
