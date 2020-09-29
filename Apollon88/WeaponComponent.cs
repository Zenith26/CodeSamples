using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponComponent : MonoBehaviour
{
    GameObject Owner = null;

    [SerializeField] List<WeaponBase> InitialWeapons = null; // the one who create the gun (foreach line)

    [SerializeField] List<WeaponBase> Weapons = null; // the one who takes the gun (add line)

    WeaponBase CurrentWeapon = null; // the current weapon in the game

    int CurrentWeaponIndex = 0;
    public int storeCurrentWeapon; // for storing the current weapon to be used in GunImage script, not using CurrentWeaponIndex itself since it's dangerous

    public AudioSource weaponSwitch; // sound for weapon switching

    void Start()
    {
        Owner = gameObject;

        SpawnWeapon();
    }

    void SpawnWeapon()
    {
        //Defining null transform
        Vector3 _loc = Vector3.zero;
        Quaternion _rot = Quaternion.identity;

        //iterating through and spawning weapons in the list
        foreach(WeaponBase gun in InitialWeapons)
        {
            // If we actually have a weapon in that array slot, continue
            if (gun) // There is a gun on the initialWeapon element in Unity? then continue 
            {
                //instantiate weapon and assign in to the value
                WeaponBase _weapon = Instantiate(gun, _loc, _rot);

                _weapon.Owner = Owner;

                //Attaching weapon to player, which is the gameobject's transform
                _weapon.transform.parent = gameObject.transform;

                _weapon.transform.localPosition = Vector3.zero; // zero means all 0 on pos, identity means all 0 on rot
                _weapon.transform.localRotation = Quaternion.identity;

                //Add our weapon to the (Weapons) list
                Weapons.Add(_weapon);

                // if we don't already have a weapon assigned as current weapon, use out spawned weapon
                if(CurrentWeapon == null)
                {
                    CurrentWeapon = _weapon;
                }
            }
        }

        InitialWeapons.Clear(); // once we spawned all of the weapon in the list (foreach) it clears the whole list.
    }

    public void StartFire()
    {
        //if weapon is valid, call weapon
        CurrentWeapon?.TriggerPulled();
    }

    
    public void StopFire()
    {
        // if weapon is valid, stop firing
        CurrentWeapon?.TriggerReleased();
    }

    public void SwitchWeapon(bool increase) // from Ternary Operator, true go up, false go down
    {
        // this has to be called before the weapon switch
        StopShooting();
        
        // value = condition ? value true : value false --- Ternary Operator
        CurrentWeaponIndex = increase ? CurrentWeaponIndex += 1 : CurrentWeaponIndex -= 1;

        if(CurrentWeaponIndex > Weapons.Count - 1) // If the CurrentWeaponIndex is higher than the list. go back to 0 (first one)
        {
            CurrentWeaponIndex = 0;
        }

        if(CurrentWeaponIndex < 0)                 // If the CurrentWeaponIndex is lower than 0 (scroll down on the first gun). change it to the last gun
        {
            CurrentWeaponIndex = Weapons.Count - 1;
        }

        CurrentWeapon = Weapons[CurrentWeaponIndex];

        weaponSwitch.Play(); // every time it switched, play the audio

        storeCurrentWeapon = CurrentWeaponIndex;
    }

    public void StopShooting()
    {
        CurrentWeapon.CancelInvoke("FireWeapon"); // FIX the problem where if you switch while shooting, it will keeps shooting the previous weapon. NOTE: CancelInvoke could be called from other script if the function is public (FireWeapon is protected)
    }
}
