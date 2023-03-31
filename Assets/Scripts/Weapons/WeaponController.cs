using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FireMode
{
    Null,
    Semi,
    Burst,
}

public class WeaponController : MonoBehaviour
{
    public enum WeaponType
    {
        Sword,
        Hammer,
        Scythe,
        Bow,
        Gun,
        Rifle,
    }

    public GameObject projectilePrefab = null;
    public WeaponType weaponType = WeaponType.Gun;
    public FireMode fireMode = FireMode.Null;

    private void Update()
    {
        if (weaponType == WeaponType.Sword || 
        weaponType == WeaponType.Hammer || 
        weaponType == WeaponType.Scythe)
        {
            fireMode = FireMode.Null;
        }
        else if (weaponType == WeaponType.Gun || 
        weaponType == WeaponType.Bow)
        {
            fireMode = FireMode.Semi;
        }
        else if (weaponType == WeaponType.Rifle)
        {
            fireMode = FireMode.Burst;
        }
    }

    public WeaponType GetWeaponType()
    {
        return weaponType;
    }

    public FireMode GetWeaponFireMode()
    {
        return fireMode;
    }
}