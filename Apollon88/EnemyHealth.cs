using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : HealthComponent
{
    // takes it from the CameraShake struct
    [SerializeField] ShakeData shake = new ShakeData();

    // override: takes the default function + add stuff
    public override void ApplyDamage (float Damage) // without override, it still give damage. Its just for the line 13 scenario if I want more damage.
    {
        // int newDamage = Damage * 2;

        base.ApplyDamage(Damage);
    }

    protected override void Death()  // add destroy
    {                                   // amount           duration
        CameraShake.shake.ShakeCamera(shake.amount, shake.duration);     // takes from the camera shake static at CameraShake script.
        
        base.Death();
        Destroy(gameObject);
    }

    protected override void Initialized()
    {
        // SEE IF I WILL USE THIS FOR FUTURE STUFF

        //base.Initialized(); No need since there are NOTHING on the Initialized function
    }
}
