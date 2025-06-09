using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class CharacterButton : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Image characterImage;
    [SerializeField] private GameObject lockObject;

    public Button Button
    {
        get { return GetComponent<Button>(); }
    }

    public void Configure(Sprite characterIcon, bool unlocked)
    {
        characterImage.sprite = characterIcon;

        if(unlocked)
            Unlock();
        else
            Lock();
        
    }

    public void Lock()
    {
        lockObject.SetActive(true);
        characterImage.color = Color.gray;
    }

    public void Unlock()
    {
        lockObject.SetActive(false);
        characterImage.color = Color.white;
    }
}
