using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour, IGameStateListener
{

    [Header("Elements")]
    [SerializeField] private Transform containerParent;
    [SerializeField] private GameObject shopItemContainerPrefab;


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
        if (gameState == GameState.SHOP)
            Configure();
    }

    private void Configure()
    {
        containerParent.Clear();

        int containersToAdd = 6;
        int weaponContainerCount = Random.Range(Mathf.Min(2, containersToAdd), containersToAdd);
        int objectContainerCount = containersToAdd - weaponContainerCount;


        for (int i = 0; i < containersToAdd; i++)
        {
            GameObject weaponContainerInstance = Instantiate(shopItemContainerPrefab, containerParent);
        }

        for (int i = 0; i < containersToAdd; i++)
        {
            GameObject objectContainerInstance = Instantiate(shopItemContainerPrefab, containerParent);
        }
    }
}
