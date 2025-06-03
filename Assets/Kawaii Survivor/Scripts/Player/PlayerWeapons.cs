using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapons : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private WeaponPosition[] WeaponPositions;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void AddWeapon(WeaponDataSO selectedWeapon, int weaponLevel)
    {
        WeaponPositions[Random.Range(0, WeaponPositions.Length)].AssignWeapon(selectedWeapon.Prefab, weaponLevel);
    }
}
