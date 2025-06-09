using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterInfoPanel : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private GameObject priceContainer;
    [SerializeField] private Transform statsParent;

    [field: SerializeField] public Button Button { get; private set; }

    public void Configure(CharacterDataSO characterData, bool unlocked)
    {
        nameText.text = characterData.CharacterName;
        priceText.text = characterData.PurchasePrice.ToString();

        priceContainer.SetActive(!unlocked);

        StatContainerManager.GenerateStatContainers(characterData.NonNeutralStats, statsParent);
    }


}
