using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapons : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private WeaponPosition[] weaponPositions;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool TryAddWeapon(WeaponDataSO weapon, int weaponLevel)
    {
        for (int i = 0; i < weaponPositions.Length; i++)
        {
            if (weaponPositions[i].Weapon != null)
                continue;

            weaponPositions[i].AssignWeapon(weapon.Prefab, weaponLevel);
            return true;
        }
        return false;
    }

    public Weapon[] GetWeapons()
    {
        List<Weapon> weapons = new List<Weapon>();
        for (int i = 0; i < weaponPositions.Length; i++)
        {
            if (weaponPositions[i].Weapon == null)
                weapons.Add(null); 
            else
                weapons.Add(weaponPositions[i].Weapon);
        }
        return weapons.ToArray();
    }

    public void RecycleWeapon(int weaponIndex)
    {
        for (int i = 0; i < weaponPositions.Length; i++)
        {
            if (i != weaponIndex)
                continue;

            int recyclePrice = weaponPositions[i].Weapon.GetRecyclePrice();
            CurrencyManager.instance.AddCurrency(recyclePrice);
            weaponPositions[i].RemoveWeapon();
            return;
        }
    }
}
