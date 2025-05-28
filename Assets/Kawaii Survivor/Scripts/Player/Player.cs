using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerHealth))]
public class Player : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private CircleCollider2D collider;
    private PlayerHealth playerHealth;

    private void Awake()
    {
        playerHealth = GetComponent<PlayerHealth>();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(int damage)
    {
        // Handle player taking damage
        playerHealth.TakeDamage(damage);
    }

    public Vector2 GetCenter()
    {
        // Return the center position of the player
        return (Vector2)transform.position + collider.offset;
    }
}
