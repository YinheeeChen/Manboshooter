using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLevel : MonoBehaviour
{
    [Header("Settings")]
    private float requiredXp;
    private float currentXp;
    private int level;

    [Header("Visuals")]
    [SerializeField] private Slider xpBar;
    [SerializeField] private TextMeshProUGUI levelText;

    private void Awake()
    {
        Candy.onCollected += CandyCollectedCallback;
    }

    private void OnDestroy()
    {
        Candy.onCollected -= CandyCollectedCallback;
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateRequiredXp();
        UpdateVisuals();
    }


    private void UpdateRequiredXp() => requiredXp = (level + 1) * 5;

    private void UpdateVisuals()
    {
        xpBar.value = (float)currentXp / requiredXp;
        levelText.text = "lvl: " + (level + 1);
    }

    private void CandyCollectedCallback(Candy candy)
    {
        currentXp++;
        if (currentXp >= requiredXp)
            LevelUp();

        UpdateVisuals();
    }

    private void LevelUp()
    {
        level++;
        currentXp = 0;
        UpdateRequiredXp();
    }
}
