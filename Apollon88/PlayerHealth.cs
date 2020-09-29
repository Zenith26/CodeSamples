using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : HealthComponent
{
    // didn't override ApplyDamage as I don't need to change / add stuff
    //public override void ApplyDamage(int Damage)
    //{
    //    base.ApplyDamage(Damage);
    //    OnDamaged?.Invoke(health);
    //}


    protected override void Death()
    {
        Destroy(gameObject); // destroy player, then call Death function
        
        GameManager.Instance.Death(); // call death
        base.Death();
    }
}
