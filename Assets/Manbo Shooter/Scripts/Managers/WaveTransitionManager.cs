using System;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;

using Random = UnityEngine.Random;

/// <summary>
/// Manages the transition phase between waves, where the player receives upgrades or chests.
/// Handles both stat bonus selection and object rewards.
/// Implements IGameStateListener to respond to wave transition state.
/// </summary>
public class WaveTransitionManager : MonoBehaviour, IGameStateListener
{
    public static WaveTransitionManager instance;

    [Header("Player")]
    [SerializeField] private PlayerObjects playerObjects;                // Reference to player's object inventory

    [Header("Elements")]
    [SerializeField] private PlayerStatManager playerStatManager;       // Reference to player stat system
    [SerializeField] private GameObject upgradeContainerParent;         // Parent container for stat bonus UI
    [SerializeField] private UpgradeContainer[] upgradeContainers;      // UI containers for stat upgrades

    [Header("Chest Related Stuff")]
    [SerializeField] private ChestObjectContainer chestContainerPrefab; // Prefab for chest object UI container
    [SerializeField] private Transform chestContainerParent;            // Parent for instantiated chest containers

    [Header("Settings")]
    private int chestCollected;                                         // Counter for how many chests were collected

    /// <summary>
    /// Initializes singleton and subscribes to chest collection event.
    /// </summary>
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        Chest.onCollected += ChestCollectedCallback;
    }

    /// <summary>
    /// Unsubscribes from chest collection event on destruction.
    /// </summary>
    private void OnDestroy()
    {
        Chest.onCollected -= ChestCollectedCallback;
    }

    void Start() { }
    void Update() { }

    /// <summary>
    /// Called when game state changes. Activates upgrade/reward UI when entering wave transition.
    /// </summary>
    public void GmaeStateChangeCallback(GameState gameState)
    {
        if (gameState == GameState.WAVETRANSITION)
            TryOpenChest();
    }

    /// <summary>
    /// Decides whether to show a chest reward or upgrade selection, based on chest count.
    /// </summary>
    private void TryOpenChest()
    {
        chestContainerParent.Clear();

        if (chestCollected > 0)
        {
            ShowObject();
        }
        else
        {
            ConfigureUpgradeContainers();
        }
    }

    /// <summary>
    /// Displays a random object reward from a collected chest.
    /// </summary>
    private void ShowObject()
    {
        chestCollected--;

        upgradeContainerParent.SetActive(false);

        ObjectDataSO[] objectDatas = ResourcesManager.Objects;
        ObjectDataSO randomObjectData = objectDatas[Random.Range(0, objectDatas.Length)];

        ChestObjectContainer chestObjectContainer = Instantiate(chestContainerPrefab, chestContainerParent);
        chestObjectContainer.Configure(randomObjectData);

        chestObjectContainer.TakeButton.onClick.AddListener(() => TakeButtonCallback(randomObjectData));
        chestObjectContainer.RecycleButton.onClick.AddListener(() => RecycleButtonCallback(randomObjectData));
    }

    /// <summary>
    /// Called when the player accepts a chest reward.
    /// Adds object and continues transition flow.
    /// </summary>
    private void TakeButtonCallback(ObjectDataSO objectToTake)
    {
        playerObjects.AddObject(objectToTake);
        TryOpenChest();
    }

    /// <summary>
    /// Called when the player recycles a chest reward.
    /// Adds currency and continues transition flow.
    /// </summary>
    private void RecycleButtonCallback(ObjectDataSO objectToRecycle)
    {
        CurrencyManager.instance.AddCurrency(objectToRecycle.RecyclePrice);
        TryOpenChest();
    }

    /// <summary>
    /// Randomly configures the upgrade containers with stat bonuses.
    /// </summary>
    [Button]
    private void ConfigureUpgradeContainers()
    {
        upgradeContainerParent.SetActive(true);

        for (int i = 0; i < upgradeContainers.Length; i++)
        {
            int randomIndex = Random.Range(0, Enum.GetValues(typeof(Stat)).Length);
            Stat stat = (Stat)Enum.GetValues(typeof(Stat)).GetValue(randomIndex);
            string randomStatString = Enums.FormatStatName(stat);
            Sprite upgradeSprite = ResourcesManager.GetStatIcon(stat);

            string buttonString;
            Action action = GetActionToPerform(stat, out buttonString);

            upgradeContainers[i].Configure(upgradeSprite, randomStatString, buttonString);
            upgradeContainers[i].Button.onClick.RemoveAllListeners();
            upgradeContainers[i].Button.onClick.AddListener(() => action?.Invoke());
            upgradeContainers[i].Button.onClick.AddListener(() => BonusSelectedCallback());
        }
    }

    /// <summary>
    /// Callback after a bonus is selected. Notifies game manager to continue to next wave.
    /// </summary>
    public void BonusSelectedCallback()
    {
        GameManager.instance.WaveCompletedCallback();
    }

    /// <summary>
    /// Returns an action that applies the stat bonus, and outputs button label string.
    /// </summary>
    private Action GetActionToPerform(Stat stat, out string buttonString)
    {
        buttonString = "";
        float value;

        switch (stat)
        {
            case Stat.Attack:
            case Stat.AttackSpeed:
            case Stat.CriticalChance:
            case Stat.MoveSpeed:
            case Stat.HealthRecoverySpeed:
            case Stat.Armor:
            case Stat.Luck:
            case Stat.Dodge:
            case Stat.LifeSteal:
                value = Random.Range(1, 10);
                buttonString = "+" + value + " %";
                break;

            case Stat.CriticalPercent:
                value = Random.Range(1f, 2f);
                buttonString = "+" + value.ToString("F2") + " x";
                break;

            case Stat.MaxHealth:
                value = Random.Range(1, 5);
                buttonString = "+" + value;
                break;

            case Stat.Range:
                value = Random.Range(1f, 5f);
                buttonString = "+" + value.ToString();
                break;

            default:
                return () => Debug.LogWarning("No action defined for this stat: " + stat);
        }

        return () => playerStatManager.AddPlayerStat(stat, value);
    }

    /// <summary>
    /// Called when a chest is collected during gameplay.
    /// </summary>
    private void ChestCollectedCallback()
    {
        chestCollected++;
    }

    /// <summary>
    /// Returns whether the player has unclaimed chests.
    /// </summary>
    public bool HasCollectedChest()
    {
        return chestCollected > 0;
    }
}
