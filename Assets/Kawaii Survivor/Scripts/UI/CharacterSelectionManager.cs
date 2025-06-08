using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectionManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Transform characterButtonParent;
    [SerializeField] private GameObject characterButtonPrefab;

    [Header("Data")]
    private CharacterDataSO[] characterDatas;

    private void Awake()
    {
        characterDatas = ResourcesManager.Characters;
    }
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Initialize()
    {
        for (int i = 0; i < characterDatas.Length; i++)
            CreateCharacterButton(i);
    }

    private void CreateCharacterButton(int index)
    {
        CharacterDataSO characterData = characterDatas[index];
        GameObject characterButton = Instantiate(characterButtonPrefab, characterButtonParent);
        characterButton.transform.GetChild(0).GetComponent<Image>().sprite = characterData.CharacterIcon;
    }
}
