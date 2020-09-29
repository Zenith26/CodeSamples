using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    [HideInInspector] // opposite of SerializedField
    public GameObject Owner;

    //setting up class for weapon data
    [System.Serializable] // serializefield but for this class (More info on WaveSpawner script)
    public class WeaponData
    {
        public Transform Muzzle = null; // the muzzle (Gun) from the WeaponBase
        public float firingRate = 500; // rate of fires per shoot. Higher = more paw paw
        public ProjectileBase Projectile; // the projectile (bullet)
        public int projectileCount = 1; // amounts of bullet in one shot (shotgun will have more than 5)
        public float anglePerRound = 0.5f; // the angle of each projectileCount
        public GameObject Effects = null; // the particle of the gun (like a MuzzleFlash)

        [HideInInspector]
        public float AdjustedFiringRate = 0; // this will adjust the fire rate for the game
    }

    [SerializeField] WeaponData weaponData = new WeaponData();
    //(first time will not called since it will shoot automatically)
    float timeTillCanFire = 0; // the second time the player shoot, it will check for how long player can shoot again

    private AudioSource weaponSound; // the sound for the gun

    private void Start()
    {                     //adjusted = 60 / firingRate
        weaponData.AdjustedFiringRate = 60 / weaponData.firingRate;

        if (transform.parent.gameObject) // if there is a parent that takes WeaponBase, then set it to Owner
        {
            Owner = transform.parent.gameObject;
        }

        weaponSound = GetComponent<AudioSource>();
    }

    public void TriggerPulled()
    {
        // checking to make sure we can fire
        if (!CanFire())
        {
            return;
        }

        //Invoke our firing at auto fire (set up Enum for different fire modes like auto, semi, burst etc)
        InvokeRepeating("FireWeapon", 0, weaponData.AdjustedFiringRate); //calling the function from the FireWeapon. same thing for the TriggerReleased
    }

    public void TriggerReleased()
    {
        CancelInvoke("FireWeapon");
    }


    // this should contain data like ammo counter and making sure we can't fire as fast as we can click
    protected bool CanFire() // accessible but not be overriden
    {
        if (Time.time < timeTillCanFire) //ex: 30 < (30 + 60/600)
        {
            return false; // how it will be true since we dont ++ the value to reach there. Thats why we use Time.time since it will keeps going up
        }

        return true;
    }

    protected virtual void FireWeapon() // this is where we shoot
    {
        if (!PauseUI.isPaused) // if not paused then we can shoot
        {
            ProjectileLogic();

            weaponSound.Play(); // play the sound for every projectile spawn

            // gametime since creation, ex: 30 sec last played means 30. (while deltaTime is time since last frame)
            timeTillCanFire = Time.time + weaponData.AdjustedFiringRate; //ex: 30 + 60/600 , Time.time will not go up if we are not shooting ( Will move if FireWeapon is called)
        }
    }

    protected virtual void ProjectileLogic()
    {
        int numProjectiles = weaponData.projectileCount;

        if (numProjectiles <= 1) // if number projectile is 1 or less
        {
            InstantiateBullet(weaponData.Muzzle.transform.rotation); // needs rotation so we called the rotation of the muzzle
        }
        else
        {
            //is the number of projectiles odd or even
            bool even = numProjectiles % 2 == 0;
            //Calculate the spacing per round based on odd / even number of projectiles
            int adjustedProjNum = even ? numProjectiles / 2 - numProjectiles : -(numProjectiles / 2 - 1) - 1;

            for (int currentRound = 0; currentRound < numProjectiles; currentRound++)
            {
                //takes the muzzle rot to this vector
                Vector3 adjustedRot = weaponData.Muzzle.transform.rotation.eulerAngles;
                int bulletMod = adjustedProjNum + currentRound;

                //muzzle rot += spacing per round * each projectile angle
                adjustedRot.y += bulletMod * weaponData.anglePerRound;

                //additional math if the projectile is even, does not work in the adjusted projectile number line
                if (even)
                {
                    adjustedRot.y += weaponData.anglePerRound / 2;
                }

                InstantiateBullet(Quaternion.Euler(adjustedRot));
            }
        }
    }

    //actually spawns projectile, marked as virtual in case we want to use a ray-cast or some other "projectile" type instead, you know uhh.. override duh
    protected virtual void InstantiateBullet(Quaternion rot)
    {
        if (weaponData.Projectile == null)
        {
            return;
        }
        // got it from ProjectileLogic()
        ProjectileBase Bullet = Instantiate(weaponData.Projectile, weaponData.Muzzle.position, rot);

        if (weaponData.Effects)
        {
            Instantiate(weaponData.Effects, weaponData.Muzzle.position, weaponData.Muzzle.rotation);
        }

        Bullet.Owner = Owner;
    }
}
