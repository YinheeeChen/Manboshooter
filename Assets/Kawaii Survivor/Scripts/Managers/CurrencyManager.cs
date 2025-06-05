using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager instance;

    [field: SerializeField] public int Currency { get; private set; }

    [Header("Actions")]
    public static Action onUpdated;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        UpdateTexts();
    }

    // Update is called once per frame
    void Update()
    {

    }


    [NaughtyAttributes.Button("Add 500 Currency")]
    public void Add500Currency()
    {
        AddCurrency(500);
    }

    public void AddCurrency(int amount)
    {
        Currency += amount;
        UpdateTexts();

        onUpdated?.Invoke();
    }

    public void UseCoins(int price) => AddCurrency(-price);

    private void UpdateTexts()
    {
        CurrencyText[] currencyTexts = FindObjectsByType<CurrencyText>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (CurrencyText currencyText in currencyTexts)
            currencyText.UpdateText(Currency.ToString());
    }

    public bool HasEnoughCurrency(int price)
    {
        return Currency >= price;
    }
}
