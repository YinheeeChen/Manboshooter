using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerDetection : MonoBehaviour
{
    [Header("Colliders")]
    [SerializeField] private CircleCollider2D detectionCollider;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.TryGetComponent(out Candy candy))
        {
            if (!collider.IsTouching(detectionCollider))
                return;

            candy.Collect(GetComponent<Player>());
        }
        
        if (collider.TryGetComponent(out Cash cash))
        {
            if (!collider.IsTouching(detectionCollider))
                return;

            cash.Collect(GetComponent<Player>());
        }
    }
}
