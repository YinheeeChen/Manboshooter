using System;
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

    private void OnDestroy()
    {
        Enemy.onDamageTaken -= EnemyHitCallback;
        PlayerHealth.onAttackDodged -= AttackDodgedCallback;
        
    }

    private void Awake()
    {
        Enemy.onDamageTaken += EnemyHitCallback;
        PlayerHealth.onAttackDodged += AttackDodgedCallback;
    }

    private void AttackDodgedCallback(Vector2 playerPosition)
    {
        DamageText damageTextInstance = damageTextPool.Get();

        Vector3 spawnPosition = playerPosition + Vector2.up * 1.5f;
        damageTextInstance.transform.position = spawnPosition;

        damageTextInstance.Animate("Dodged", false);

        LeanTween.delayedCall(1, () => {
            damageTextPool.Release(damageTextInstance);
        });
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
        // instance.GetComponent<Animator>().Play("Animate", -1, 0f);
    }

    private void ActionOnRelease(DamageText instance)
    {
        if (instance != null)
            instance.gameObject.SetActive(false);
    }

    private void ActionOnDestroy(DamageText instance)
    {
        Destroy(instance.gameObject);
    }

    private void EnemyHitCallback(int damage, Vector2 enemyPosition, bool isCriticalHit)
    {
        DamageText damageTextInstance = damageTextPool.Get();

        Vector3 spawnPosition = enemyPosition + Vector2.up * 1.5f;
        damageTextInstance.transform.position = spawnPosition;

        damageTextInstance.Animate(damage.ToString(), isCriticalHit);

        LeanTween.delayedCall(1, () => {
            damageTextPool.Release(damageTextInstance);
        });
    }
}
