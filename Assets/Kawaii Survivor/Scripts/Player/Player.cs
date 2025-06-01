using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerHealth), typeof(PlayerLevel))]
public class Player : MonoBehaviour
{
    public static Player instance;

    [Header("Components")]
    [SerializeField] private CircleCollider2D collider;
    private PlayerHealth playerHealth;
    private PlayerLevel playerLevel;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        playerHealth = GetComponent<PlayerHealth>();
        playerLevel = GetComponent<PlayerLevel>();
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

    public bool HasLeveledUp()
    {
        return playerLevel.HasLeveledUp();
    }
}
