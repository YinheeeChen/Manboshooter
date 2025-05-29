using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class DamageTextManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private DamageText damageTextPrefab;

    [Header("Pooling")]
    [SerializeField] private ObjectPool<DamageText> damageTextPool;

    private void OnDestroy() {
        MeleeEnemy.onDamageTaken -= EnemyHitCallback;
    }

    private void Awake() {
        MeleeEnemy.onDamageTaken += EnemyHitCallback;
    }


    // Start is called before the first frame update
    void Start()
    {
        damageTextPool = new ObjectPool<DamageText>(
            CreateDamageTextInstance,
            ActionOnGet,
            ActionOnRelease,
            ActionOnDestroy
        );
    }

    private DamageText CreateDamageTextInstance()
    {
        return Instantiate(damageTextPrefab, transform);
    }

    private void ActionOnGet(DamageText instance)
    {
        instance.gameObject.SetActive(true);
    }

    private void ActionOnRelease(DamageText instance)
    {
        instance.gameObject.SetActive(false);
    }

    private void ActionOnDestroy(DamageText instance)
    {
        Destroy(instance.gameObject);
    }

    private void EnemyHitCallback(int damage, Vector2 enemyPosition)
    {
        DamageText damageTextInstance = damageTextPool.Get();

        Vector3 spawnPosition = enemyPosition + Vector2.up * 1.5f;
        damageTextInstance.transform.position = spawnPosition;

        damageTextInstance.Animate(damage);

        LeanTween.delayedCall(1, () => {
            damageTextPool.Release(damageTextInstance);
        });
    }
}
