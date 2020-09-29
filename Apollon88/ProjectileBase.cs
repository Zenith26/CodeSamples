using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBase : MonoBehaviour
{
    Rigidbody rb = null;

    [SerializeField] float Velocity = 20; // higher the Velocity, the faster the projectile is

    [SerializeField] GameObject ImpactFX = null; // both this and the owner will be null for now, unless i want to add particle for the projectile

    public GameObject Owner = null; // look at the if check from OnTriggerEnter to see what Owner really does

    public ComboKill ComboKill; // find object of type combo kill on awake
    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        //adding initial movement force           to make sure it will not keep physics once going other direction (like HTMAVG)
        rb.AddForce(transform.forward * Velocity, ForceMode.VelocityChange);

        ComboKill = GameObject.FindObjectOfType<ComboKill>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //make sure we're not colliding with the player shooting the projectile
        if(other.gameObject == Owner)
        {
            return;
        }
        if (Application.isEditor)// are we running it inside the editor
        {
            //Debug.Log("Projectile collided with " + other.name);
        }



        if (other.gameObject.GetComponent<EnemyHealth>()) // that way it will not call this if it's not enemy, perfect for increasing combo cooldown
        {
            if (gameObject.name == "MinigunBullet(Clone)") // if the projectile is from MinigunBullet(Clone)(Since we spawn it), then change damage
            {
                GameplayStatics.DealDamage(other.gameObject, 0.4f);
                Debug.Log("MACHINE GUN");
            }
            else
            {
                Debug.Log("NOT");
                // damage hit enemy or other object if they have HealthComponent
                GameplayStatics.DealDamage(other.gameObject, 1);
            }
            if(ComboKill.currentCombo != 1) // only increase cooldown if current combo is not 1
            {
                ComboKill.cooldown = ComboKill.cooldown + 0.1f; // increase combo cooldown
            }
        }

        // instantiating effects
        SpawnEffects();

        //if(other.name != "BaseProjectile(Clone)") // note, forgot to use layer zzz
        //{                                 not going to delete as this will show how to check clone prefab
        //    Destroy(gameObject);
        //}
        Destroy(gameObject);
    }

    //death effects
    protected virtual void SpawnEffects()
    {
        if (ImpactFX)
        {
            Instantiate(ImpactFX, transform.position, Quaternion.identity);
        }
    }
}
