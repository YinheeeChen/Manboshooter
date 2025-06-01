using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;

using Random = UnityEngine.Random;

public class WaveTransitionManager : MonoBehaviour, IGameStateListener
{
    [Header("Elements")]
    [SerializeField] private PlayerStatManager playerStatManager;
    [SerializeField] private UpgradeContainer[] upgradeContainers;
    

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GmaeStateChangeCallback(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.WAVETRANSITION:
                ConfigureUpgradeContainers();
                break;
        }
    }

    [Button]
    private void ConfigureUpgradeContainers()
    {
        for (int i = 0; i < upgradeContainers.Length; i++)
        {
            int randomIndex = Random.Range(0, Enum.GetValues(typeof(Stat)).Length);
            Stat stat = (Stat)Enum.GetValues(typeof(Stat)).GetValue(randomIndex);
            string randomStatString = Enums.FormatStatName(stat);

            string buttonString;
            Action action = GetActionToPerform(stat, out buttonString);

            upgradeContainers[i].Configure(null, randomStatString, buttonString);
            upgradeContainers[i].Button.onClick.RemoveAllListeners();
            upgradeContainers[i].Button.onClick.AddListener(() => action?.Invoke());
            upgradeContainers[i].Button.onClick.AddListener(() => BonusSelectedCallback());
        }
    }

    public void BonusSelectedCallback()
    {
        GameManager.instance.WaveCompletedCallback();
    }

    private Action GetActionToPerform(Stat stat, out string buttonString)
    {
        buttonString = "";
        float value;

        value = Random.Range(1, 10);
        buttonString = "+" + value.ToString() + " %";

        switch (stat)
        {
            case Stat.Attack:
                value = Random.Range(1, 10);
                break;
            case Stat.AttackSpeed:
                value = Random.Range(1, 10);
                break;
            case Stat.CriticalChance:
                value = Random.Range(1, 10);
                break;
            case Stat.CriticalPercent:
                value = Random.Range(1f, 2f);
                buttonString = "+" + value.ToString("F2") + " x";
                break;
            case Stat.MoveSpeed:
                value = Random.Range(1, 10);
                break;
            case Stat.MaxHealth:
                value = Random.Range(1, 5);
                buttonString = "+" + value;
                break;
            case Stat.Range:
                value = Random.Range(1f, 5f);
                buttonString = "+" + value.ToString();
                break;
            case Stat.HealthRecoverySpeed:
                value = Random.Range(1, 10);
                break;
            case Stat.Armor:
                value = Random.Range(1, 10);
                break;
            case Stat.Luck:
                value = Random.Range(1, 10);
                break;
            case Stat.Dodge:
                value = Random.Range(1, 10);
                break;
            case Stat.LifeSteal:
                value = Random.Range(1, 10);
                break;
            default:
                return () => Debug.LogWarning("No action defined for this stat: " + stat);

        }
        return () => playerStatManager.AddPlayerStat(stat, value);

    }
}
