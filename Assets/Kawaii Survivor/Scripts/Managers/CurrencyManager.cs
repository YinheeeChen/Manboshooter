using System;
using System.Collections;
using System.Collections.Generic;
using Tabsil.Sijil;
using Unity.VisualScripting;
using UnityEngine;

public class CurrencyManager : MonoBehaviour, IWantToBeSaved
{
    public static CurrencyManager instance;

    private const string ProCurrencyKey = "ProCurrency";

    [field: SerializeField] public int Currency { get; private set; }
    [field: SerializeField] public int ProCurrency { get; private set; }


    [Header("Actions")]
    public static Action onUpdated;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        Load();

        //AddProCurrency(PlayerPrefs.GetInt(ProCurrencyKey, 100), false);

        Candy.onCollected += CandyCollectedCallback;
        Cash.onCollected += CashCollectedCallback;
    }

    private void OnDestroy()
    {
        Candy.onCollected -= CandyCollectedCallback;
        Cash.onCollected -= CashCollectedCallback;
    }

    
    public void Save()
    {
        Sijil.Save(this, ProCurrencyKey, ProCurrency);
    }

    public void Load()
    {
        if (Sijil.TryLoad(this, ProCurrencyKey, out object proCurrencyValue))
            AddProCurrency((int)proCurrencyValue, false);
        else
            AddProCurrency(100);
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
    public void Add500Currency() => AddCurrency(500);

    [NaughtyAttributes.Button("Add 500 Pro Currency")]
    public void Add500ProCurrency() => AddProCurrency(500);


    public void AddCurrency(int amount)
    {
        Currency += amount;
        UpdateVisuals();
    }

    public void AddProCurrency(int amount, bool save = true)
    {
        ProCurrency += amount;
        UpdateVisuals();

        //PlayerPrefs.SetInt(ProCurrencyKey, ProCurrency);
    }

    public void UpdateVisuals()
    {
        UpdateTexts();
        onUpdated?.Invoke();
        Save()
;    }

    private void UpdateTexts()
    {
        CurrencyText[] currencyTexts = FindObjectsByType<CurrencyText>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (CurrencyText currencyText in currencyTexts)
            currencyText.UpdateText(Currency.ToString());

        ProCurrencyText[] proCurrencyTexts = FindObjectsByType<ProCurrencyText>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (ProCurrencyText proCurrencyText in proCurrencyTexts)
            proCurrencyText.UpdateText(ProCurrency.ToString());
    }

    public void UseCurrency(int price) => AddCurrency(-price);
    public void UseProCurrency(int price) => AddProCurrency(-price);

    public bool HasEnoughCurrency(int price) => Currency >= price;
    public bool HasEnoughProCurrency(int price) => ProCurrency >= price;

    private void CandyCollectedCallback(Candy candy) => AddCurrency(1);
    private void CashCollectedCallback(Cash cash) => AddProCurrency(1);

    
}
