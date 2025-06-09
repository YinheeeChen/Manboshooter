using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerHealth), typeof(PlayerLevel))]
public class Player : MonoBehaviour
{
    public static Player instance;

    [Header("Components")]
    [SerializeField] private CircleCollider2D collider2d;
    [SerializeField] private SpriteRenderer playerRenderer;
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

        CharacterSelectionManager.OnCharacterSelected += CharacterSelectedCallback;
    }

    private void OnDestroy()
    {
        CharacterSelectionManager.OnCharacterSelected -= CharacterSelectedCallback;
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
        return (Vector2)transform.position + collider2d.offset;
    }

    public bool HasLeveledUp()
    {
        return playerLevel.HasLeveledUp();
    }


    private void CharacterSelectedCallback(CharacterDataSO characterData)
    {
        playerRenderer.sprite = characterData.CharacterIcon;
        
        Gifplayer gifplayer = GetComponent<Gifplayer>();

        if (gifplayer != null && characterData.CharacterFrames != null && characterData.CharacterFrames.Length > 0)
        {
            gifplayer.SetFrames(characterData.CharacterFrames);
        }
    }

}
