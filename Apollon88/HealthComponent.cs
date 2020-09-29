using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    public float health = 3;

    float maxHealth;

    [SerializeField] GameObject deathFX = null;

    public MulticastNoParams OnDeath;
    public MulticastOneFloatParam OnDamaged;

    void Awake()
    {
        maxHealth = health;
        Initialized(); // For the EnemyHealth to run the function since this will not be called anywhere on this script
    }

    protected virtual void Initialized()
    {
        // Nothing, it will be overriden on EnemyHealth
    }

    // to override, needs virtual
    public virtual void ApplyDamage(float Damage)
    {
        if(Damage <= 0 || health <= 0)
        {
            return; // SAFE CHECK
        }

        health -= Damage;

        Debug.Log("Amount of " + Damage + " Damage dealt, current health is " + health);

        //?. is a null reference check, it will not call the OnDamaged Delegate if there are no bound functions (meaning the delegate == null)
        //OnDamaged?.Invoke(Damage);

        if(health <= 0)
        {
            Death();
        }

        if(OnDamaged != null)
        {
            //?. is a null reference check, it will not call the OnDamaged Delegate if there are no bound functions (meaning the delegate == null)
            OnDamaged?.Invoke(Damage); // will do nothing since there are no function that subscribe OnDamaged
        }
        else
        {
            Debug.LogError(name + " No functions bound to OnDamaged Delegate");
        }
    }

    // summon the particle, taking from the Death function in EH script
    protected virtual void Death()
    {
        if(OnDeath != null)
        {
            // this one takes it from the AIBase
            OnDeath?.Invoke();        // no params, means nothing inside an invoke
        }
        else
        {
            Debug.LogError(name + " No functions bound to OnDeath Delegate");
        }

        // this one takes it from the AIBase
        //OnDeath?.Invoke();        // no params, means nothing inside an invoke
    
        if(deathFX != null)
        {
                      // object       position         rotation. (transform.rotation if you want it to be auto)
            Instantiate(deathFX, transform.position, Quaternion.Euler(-90, 0, 0));
        }

        Debug.Log("Health Component - Death Called");
    }

    public virtual void Kill()
    {
        ApplyDamage(maxHealth);
    }
}
