using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSelectionManager : MonoBehaviour, IGameStateListener
{
    [Header("Elements")]
    [SerializeField] private Transform conrainersParent;
    [SerializeField] private WeaponSelectionContainer weaponContainerPrefab;

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
            case GameState.WEAPONSELECTION:
                Configure();
                break;

            default:
                break;
        }
    }

    private void Configure()
    {
        conrainersParent.Clear();

        for (int i = 0; i < 3; i++)
            GenerateWeaponContainer();
    }

    private void GenerateWeaponContainer()
    {
        WeaponSelectionContainer weaponContainer = Instantiate(weaponContainerPrefab, conrainersParent);
    }
}