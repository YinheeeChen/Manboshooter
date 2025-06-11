using System;
using Tabsil.Sijil;
using UnityEngine;

/// <summary>
/// Manages both regular and premium ("Pro") currency.
/// Supports collection, usage, UI updating, and persistent save/load functionality.
/// Implements IWantToBeSaved for integration with the Sijil save system.
/// </summary>
public class CurrencyManager : MonoBehaviour, IWantToBeSaved
{
    public static CurrencyManager instance;   // Singleton instance for global access

    private const string ProCurrencyKey = "ProCurrency"; // Key used for saving ProCurrency

    [field: SerializeField] public int Currency { get; private set; }       // Regular currency amount
    [field: SerializeField] public int ProCurrency { get; private set; }    // Pro (premium) currency amount

    [Header("Actions")]
    public static Action onUpdated;   // Event triggered when currency values change

    /// <summary>
    /// Initializes the singleton instance and subscribes to collection events.
    /// </summary>
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        // Subscribe to pickup events
        Candy.onCollected += CandyCollectedCallback;
        Cash.onCollected += CashCollectedCallback;
    }

    /// <summary>
    /// Unsubscribes from collection events to prevent memory leaks.
    /// </summary>
    private void OnDestroy()
    {
        Candy.onCollected -= CandyCollectedCallback;
        Cash.onCollected -= CashCollectedCallback;
    }

    /// <summary>
    /// Saves the ProCurrency value using the Sijil save system.
    /// </summary>
    public void Save()
    {
        Sijil.Save(this, ProCurrencyKey, ProCurrency);
    }

    /// <summary>
    /// Loads the ProCurrency value using Sijil. If none exists, initializes with 100.
    /// </summary>
    public void Load()
    {
        if (Sijil.TryLoad(this, ProCurrencyKey, out object proCurrencyValue))
            AddProCurrency((int)proCurrencyValue, false);
        else
            AddProCurrency(100, false);
    }

    /// <summary>
    /// Called on game start; updates all visual currency displays.
    /// </summary>
    void Start()
    {
        UpdateTexts();
    }

    void Update()
    {
        // No logic in update loop
    }

    /// <summary>
    /// Editor-only button to quickly add 500 regular currency.
    /// </summary>
    [NaughtyAttributes.Button("Add 500 Currency")]
    public void Add500Currency() => AddCurrency(500);

    /// <summary>
    /// Editor-only button to quickly add 500 Pro currency.
    /// </summary>
    [NaughtyAttributes.Button("Add 500 Pro Currency")]
    public void Add500ProCurrency() => AddProCurrency(500);

    /// <summary>
    /// Increases regular currency and updates visuals.
    /// </summary>
    /// <param name="amount">Amount to add (or subtract if negative).</param>
    public void AddCurrency(int amount)
    {
        Currency += amount;
        UpdateVisuals();
    }

    /// <summary>
    /// Increases Pro currency and optionally saves it.
    /// </summary>
    /// <param name="amount">Amount to add.</param>
    /// <param name="save">Whether to persist the new value.</param>
    public void AddProCurrency(int amount, bool save = true)
    {
        ProCurrency += amount;
        UpdateVisuals();

        // Optionally persist using PlayerPrefs (commented out)
        // PlayerPrefs.SetInt(ProCurrencyKey, ProCurrency);
    }

    /// <summary>
    /// Triggers UI update and invokes onUpdated event. Also calls Save().
    /// </summary>
    public void UpdateVisuals()
    {
        UpdateTexts();
        onUpdated?.Invoke();
        Save();
    }

    /// <summary>
    /// Updates all UI elements that display currency values.
    /// Searches for all CurrencyText and ProCurrencyText objects.
    /// </summary>
    private void UpdateTexts()
    {
        CurrencyText[] currencyTexts = FindObjectsByType<CurrencyText>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (CurrencyText currencyText in currencyTexts)
            currencyText.UpdateText(Currency.ToString());

        ProCurrencyText[] proCurrencyTexts = FindObjectsByType<ProCurrencyText>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (ProCurrencyText proCurrencyText in proCurrencyTexts)
            proCurrencyText.UpdateText(ProCurrency.ToString());
    }

    /// <summary>
    /// Deducts regular currency by the given price.
    /// </summary>
    public void UseCurrency(int price) => AddCurrency(-price);

    /// <summary>
    /// Deducts Pro currency by the given price.
    /// </summary>
    public void UseProCurrency(int price) => AddProCurrency(-price);

    /// <summary>
    /// Checks if the player has enough regular currency.
    /// </summary>
    /// <param name="price">Amount required.</param>
    /// <returns>True if sufficient.</returns>
    public bool HasEnoughCurrency(int price) => Currency >= price;

    /// <summary>
    /// Checks if the player has enough Pro currency.
    /// </summary>
    /// <param name="price">Amount required.</param>
    /// <returns>True if sufficient.</returns>
    public bool HasEnoughProCurrency(int price) => ProCurrency >= price;

    /// <summary>
    /// Called when a candy is collected; adds 1 regular currency.
    /// </summary>
    private void CandyCollectedCallback(Candy candy) => AddCurrency(1);

    /// <summary>
    /// Called when cash is collected; adds 1 Pro currency.
    /// </summary>
    private void CashCollectedCallback(Cash cash) => AddProCurrency(1);
}
