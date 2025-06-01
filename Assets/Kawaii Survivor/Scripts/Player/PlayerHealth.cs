using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour, IPlayerStatDependency
{

    [Header("Settings")]
    [SerializeField] private int baseMaxHealth;
    private int maxHealth;
    private int health;

    [Header("Elements")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TextMeshProUGUI healthText;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void UpdateHealthUI()
    {
        healthSlider.value = (float)health / maxHealth;
        healthText.text = $"{health} / {maxHealth}";
    }

    public void TakeDamage(int damage)
    {
        int realDamage = Mathf.Min(damage, health);
        health -= realDamage; 

        UpdateHealthUI();

        if (health <= 0)
        {
            PassAway();
        }
        else
        {
            // Handle player taking damage
            Debug.Log($"Player took {realDamage} damage. Remaining health: {health}");
        }
    }

    private void PassAway()
    {
        GameManager.instance.SetGmaeState(GameState.GAMEOVER);
    }

    

    public void UpdateStats(PlayerStatManager playerStatManager)
    {
        float addedHealth = playerStatManager.GetStatVlaue(Stat.MaxHealth);
        maxHealth = baseMaxHealth + (int)addedHealth;
        maxHealth = Mathf.Max(maxHealth, 1);

        health = maxHealth;
        UpdateHealthUI();
    }
}
