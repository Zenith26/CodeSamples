using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunImage : MonoBehaviour
{
    WeaponComponent weaponComponent; // get the weapon component in order to take the current weapon

    [SerializeField] GameObject[] gunImage; // arrays of gun image
    private void Start()
    {
        weaponComponent = FindObjectOfType<WeaponComponent>();
        weaponComponent = weaponComponent.GetComponent<WeaponComponent>();

        gunImage[0].SetActive(false);
        gunImage[1].SetActive(false);
        gunImage[2].SetActive(false);
    }

    // NO CHOICE BUT TO DO IT ON Update, since Player who has WeaponComponent is not on hierachy, same thing goes for PlayerController who works for scrolling the weapon
    // I guess WeaponComponent could take WeaponBase as long as its not a GameObject
    private void Update()
    {
        if(weaponComponent.storeCurrentWeapon == 0) // if 0 Default Gun
        {
            gunImage[0].SetActive(true);
            gunImage[1].SetActive(false);
            gunImage[2].SetActive(false);
            return;
        }
        else if (weaponComponent.storeCurrentWeapon == 1) // if 1 Shotgun
        {
            gunImage[1].SetActive(true);
            gunImage[0].SetActive(false);
            gunImage[2].SetActive(false);
            return;
        }
        else if (weaponComponent.storeCurrentWeapon == 2) // if 2 Minigun
        {
            gunImage[2].SetActive(true);
            gunImage[0].SetActive(false);
            gunImage[1].SetActive(false);
            return;
        }
    }
}
