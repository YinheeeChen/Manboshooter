using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager instance;

    [field: SerializeField] public int Currency { get; private set; }

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

    public void AddCurrency(int amount)
    {
        Currency += amount;
        UpdateTexts();
    }

    private void UpdateTexts()
    {
        CurrencyText[] currencyTexts = FindObjectsByType<CurrencyText>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (CurrencyText currencyText in currencyTexts)
            currencyText.UpdateText(Currency.ToString());

    }
}
