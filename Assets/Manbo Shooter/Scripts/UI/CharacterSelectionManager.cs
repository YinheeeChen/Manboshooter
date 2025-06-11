using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Tabsil.Sijil;

/// <summary>
/// Manages character selection UI and logic, including character unlocking, selection, and persistence.
/// Implements IWantToBeSaved to support save/load using Sijil.
/// </summary>
public class CharacterSelectionManager : MonoBehaviour, IWantToBeSaved
{
    [Header("Elements")]
    [SerializeField] private Transform characterButtonParent;            // Parent object for all character buttons
    [SerializeField] private CharacterButton characterButtonPrefab;      // Prefab for character button
    [SerializeField] private Image centerCharacterImage;                 // Central character image display

    [SerializeField] private CharacterInfoPanel characterInfo;          // Panel showing character info and purchase button

    [Header("Data")]
    private CharacterDataSO[] characterDatas;                            // All character data loaded from resources
    private List<bool> unlockedStats = new List<bool>();                // Unlock status of each character
    private const string unlockedStatsKey = "UnlockedStatsKey";         // Key for saving unlocked stats
    private const string lastSelectedCharacterKey = "LastSelectedCharacterKey"; // Key for saving selected character

    [Header("Settings")]
    private int selectedCharacterIndex;                                  // Currently selected character index
    private int lastSelectedCharacterIndex;                              // Last selected character index (persistent)

    [Header("Actions")]
    public static Action<CharacterDataSO> OnCharacterSelected;           // Triggered when a character is selected

    private void Awake()
    {
        // Nothing to initialize here yet
    }

    // Start is called before the first frame update
    void Start()
    {
        // Setup purchase button
        characterInfo.Button.onClick.RemoveAllListeners();
        characterInfo.Button.onClick.AddListener(PurchaseCharacterCallback);

        // Select the last used character on start
        CharacterSelectedCallback(lastSelectedCharacterIndex);
    }

    /// <summary>
    /// Initializes character buttons and UI.
    /// </summary>
    private void Initialize()
    {
        for (int i = 0; i < characterDatas.Length; i++)
            CreateCharacterButton(i);
    }

    /// <summary>
    /// Instantiates and configures a character button.
    /// </summary>
    private void CreateCharacterButton(int index)
    {
        CharacterDataSO characterData = characterDatas[index];
        CharacterButton characterButton = Instantiate(characterButtonPrefab, characterButtonParent);

        characterButton.Configure(characterData.CharacterIcon, unlockedStats[index]);
        characterButton.Button.onClick.RemoveAllListeners();
        characterButton.Button.onClick.AddListener(() => CharacterSelectedCallback(index));
    }

    /// <summary>
    /// Handles character selection: updates center image, info panel, and applies selection or purchase logic.
    /// </summary>
    private void CharacterSelectedCallback(int index)
    {
        selectedCharacterIndex = index;

        CharacterDataSO characterData = characterDatas[index];
        centerCharacterImage.sprite = characterData.CharacterIcon;

        if (unlockedStats[index])
        {
            lastSelectedCharacterIndex = index;
            characterInfo.Button.interactable = false;
            Save(); // Save the last selected character

            OnCharacterSelected?.Invoke(characterData);
        }
        else
        {
            characterInfo.Button.interactable = CurrencyManager.instance.HasEnoughProCurrency(characterData.PurchasePrice);
        }

        characterInfo.Configure(characterData, unlockedStats[index]);
    }

    /// <summary>
    /// Handles character purchasing and unlocks the selected character.
    /// </summary>
    private void PurchaseCharacterCallback()
    {
        int price = characterDatas[selectedCharacterIndex].PurchasePrice;

        CurrencyManager.instance.UseProCurrency(price);

        unlockedStats[selectedCharacterIndex] = true;

        // Visually unlock the character button
        characterButtonParent.GetChild(selectedCharacterIndex).GetComponent<CharacterButton>().Unlock();

        CharacterSelectedCallback(selectedCharacterIndex); // Re-trigger selection to apply changes

        Save(); // Save unlock state
    }

    /// <summary>
    /// Loads saved data including unlock status and last selected character.
    /// </summary>
    public void Load()
    {
        characterDatas = ResourcesManager.Characters;

        // Default: only the first character is unlocked
        for (int i = 0; i < characterDatas.Length; i++)
            unlockedStats.Add(i == 0);

        // Try loading saved unlock states
        if (Sijil.TryLoad(this, unlockedStatsKey, out object unlockedStatsObj))
            unlockedStats = (List<bool>)unlockedStatsObj;

        // Try loading last selected character
        if (Sijil.TryLoad(this, lastSelectedCharacterKey, out object lastSelectedCharacterObj))
            lastSelectedCharacterIndex = (int)lastSelectedCharacterObj;

        Initialize(); // Set up buttons
    }

    /// <summary>
    /// Saves current character unlock states and last selected character index.
    /// </summary>
    public void Save()
    {
        Sijil.Save(this, unlockedStatsKey, unlockedStats);
        Sijil.Save(this, lastSelectedCharacterKey, lastSelectedCharacterIndex);
    }
}
