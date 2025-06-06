using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPosition : MonoBehaviour
{
    [Header("Elements")]
    public Weapon Weapon { get; private set; }

    public void AssignWeapon(Weapon weapon, int weaponLevel)
    {
        Weapon = Instantiate(weapon, transform);

        Weapon.transform.localPosition = Vector3.zero;
        Weapon.transform.localRotation = Quaternion.identity;

        Weapon.UpgradeTo(weaponLevel);
    }

    public void RemoveWeapon()
    {
        Destroy(Weapon.gameObject);
        Weapon = null;
    }

}
