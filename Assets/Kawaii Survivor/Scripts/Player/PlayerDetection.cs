using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerDetection : MonoBehaviour
{
    [Header("Colliders")]
    [SerializeField] private CircleCollider2D detectionCollider;

    private void FixedUpdate()
    {
        Collider2D[] candyColliders = Physics2D.OverlapCircleAll(
            (Vector2)transform.position + detectionCollider.offset,
            detectionCollider.radius);

        foreach (Collider2D collider in candyColliders)
        {
            if (collider.TryGetComponent(out Candy candy))
            {
                candy.Collect(GetComponent<Player>());
            }
        }
    }
}
