using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using Unity.VisualScripting;
using UnityEditor.U2D.Animation;
using UnityEngine;
using UnityEngine.UI;
using Tabsil.Sijil;
using UnityEngine.TextCore.Text;

public class CharacterSelectionManager : MonoBehaviour, IWantToBeSaved
{
    [Header("Elements")]
    [SerializeField] private Transform characterButtonParent;
    [SerializeField] private CharacterButton characterButtonPrefab;
    [SerializeField] private Image centerCharacterImage;

    [SerializeField] private CharacterInfoPanel characterInfo;

    [Header("Data")]
    private CharacterDataSO[] characterDatas;
    private List<bool> unlockedStats = new List<bool>();
    private const string unlockedStatsKey = "UnlockedStatsKey";
    private const string lastSelectedCharacterKey = "LastSelectedCharacterKey";

    [Header("Settings")]
    private int selectedCharacterIndex;
    private int lastSelectedCharacterIndex;

    [Header("Actions")]
    public static Action<CharacterDataSO> OnCharacterSelected;   

    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        characterInfo.Button.onClick.RemoveAllListeners();
        characterInfo.Button.onClick.AddListener(PurchaseCharacterCallback);

        CharacterSelectedCallback(lastSelectedCharacterIndex);
    }

    private void Initialize()
    {
        for (int i = 0; i < characterDatas.Length; i++)
            CreateCharacterButton(i);
    }

    private void CreateCharacterButton(int index)
    {
        CharacterDataSO characterData = characterDatas[index];
        CharacterButton characterButton = Instantiate(characterButtonPrefab, characterButtonParent);

        characterButton.Configure(characterData.CharacterIcon, unlockedStats[index]);
        characterButton.Button.onClick.RemoveAllListeners();
        characterButton.Button.onClick.AddListener(() => CharacterSelectedCallback(index));
    }

    private void CharacterSelectedCallback(int index)
    {
        selectedCharacterIndex = index;

        CharacterDataSO characterData = characterDatas[index];
        centerCharacterImage.sprite = characterDatas[index].CharacterIcon;

        if (unlockedStats[index])
        {
            lastSelectedCharacterIndex = index;
            characterInfo.Button.interactable = false;
            Save();

            OnCharacterSelected?.Invoke(characterData);
        }
        else
            characterInfo.Button.interactable = CurrencyManager.instance.HasEnoughProCurrency(characterData.PurchasePrice);

        characterInfo.Configure(characterData, unlockedStats[index]);
    }

    private void PurchaseCharacterCallback()
    {
        int price = characterDatas[selectedCharacterIndex].PurchasePrice;
        CurrencyManager.instance.UseProCurrency(price);

        unlockedStats[selectedCharacterIndex] = true;

        characterButtonParent.GetChild(selectedCharacterIndex).GetComponent<CharacterButton>().Unlock();

        CharacterSelectedCallback(selectedCharacterIndex);

        Save();
    }

    public void Load()
    {
        characterDatas = ResourcesManager.Characters;
        for (int i = 0; i < characterDatas.Length; i++)
            unlockedStats.Add(i == 0);

        if (Sijil.TryLoad(this, unlockedStatsKey, out object unlockedStatsObj))
        {
            unlockedStats = (List<bool>)unlockedStatsObj;
        }

        if (Sijil.TryLoad(this, lastSelectedCharacterKey, out object lastSelectedCharacterObj))
            lastSelectedCharacterIndex = (int)lastSelectedCharacterObj;

        Initialize();

        // CharacterSelectedCallback(lastSelectedCharacterIndex);
    }

    public void Save()
    {
        Sijil.Save(this, unlockedStatsKey, unlockedStats);
        Sijil.Save(this, lastSelectedCharacterKey, lastSelectedCharacterIndex);
    }
}
