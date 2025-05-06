using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Element")]
    private Player player;

    [Header("Settings")]
    [SerializeField] private float speed;
    [SerializeField] private float playerDetectionRadius;

    [Header("Effects")]
    [SerializeField] private ParticleSystem passAwayParticles;

    // Start is called before the first frame update
    void Start()
    {
        player = FindFirstObjectByType<Player>();
        if (player == null)
        {
            Debug.LogError("Player not found in the scene.");
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            FollowPlayer();
            TryAttackPlayer();
        }
    }

    private void FollowPlayer()
    {
        Vector2 direction = (player.transform.position - transform.position).normalized;
        Vector2 targetPosition = (Vector2)transform.position + speed * direction * Time.deltaTime;
        transform.position = targetPosition;
    }

    private void TryAttackPlayer()
    {
        if (Vector2.Distance(transform.position, player.transform.position) < playerDetectionRadius)
        {
            // Attack logic here
            PassAway();
        }
        
    }

    private void PassAway()
    {
        if (passAwayParticles != null)
        {
            passAwayParticles.transform.SetParent(null);
            passAwayParticles.Play();
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, playerDetectionRadius);
    }

}
