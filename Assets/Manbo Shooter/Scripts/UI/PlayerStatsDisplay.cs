using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsDisplay : MonoBehaviour, IPlayerStatDependency
{
    [Header("Elements")]
    [SerializeField] private Transform playerStatContainersParent;

    public void UpdateStats(PlayerStatManager playerStatManager)
    {
        int index = 0;

        foreach (Stat stat in Enum.GetValues(typeof(Stat)))
        {
            StatContainer statContainer = playerStatContainersParent.GetChild(index).GetComponent<StatContainer>();
            statContainer.gameObject.SetActive(true);

            Sprite icon = ResourcesManager.GetStatIcon(stat);

            statContainer.Configure(
                icon,
                Enums.FormatStatName(stat),
                playerStatManager.GetStatVlaue(stat),
                true
            );

            index++;
        }

        for (int i = index; i < playerStatContainersParent.childCount; i++)
            playerStatContainersParent.GetChild(i).gameObject.SetActive(false);
    }


}
