using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

public class DropManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Candy candyPrefab;

    private void Awake()
    {
        Enemy.onPassedAway += EnemyPassedAwayCallback;
    }

    private void OnDestroy()
    {
        Enemy.onPassedAway -= EnemyPassedAwayCallback;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void EnemyPassedAwayCallback(Vector2 enemyPosition)
    {
        Candy candyInstance = Instantiate(candyPrefab, enemyPosition, Quaternion.identity, transform);
        candyInstance.name = "Candy" + Random.Range(0, 5000).ToString();
    }

}
