using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerStatManager))]
public class PlayerObjects : MonoBehaviour
{
    [field: SerializeField] public List<ObjectDataSO> Objects { get; private set; }
    private PlayerStatManager playerStatManager;

    private void Awake()
    {
        playerStatManager = GetComponent<PlayerStatManager>();
    }

    // Start is called before the first frame update
    void Start()
    {

        foreach (ObjectDataSO data in Objects)
            playerStatManager.AddObject(data.BaseStats);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddObject(ObjectDataSO objectData)
    {
        Objects.Add(objectData);
        playerStatManager.AddObject(objectData.BaseStats);
    }

    public void RecycleObject(ObjectDataSO objectData)
    {
        Objects.Remove(objectData);
        CurrencyManager.instance.AddCurrency(objectData.RecyclePrice);
        playerStatManager.RemoveObjectStats(objectData.BaseStats);
    }
}
