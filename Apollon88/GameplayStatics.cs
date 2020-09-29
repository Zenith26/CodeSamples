using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// basically create a static class that will be access to everything.
public static class GameplayStatics
{
    public static void DealDamage(GameObject DamagedObject, float Damage)
    {
        HealthComponent health = DamagedObject.GetComponent<HealthComponent>();
        if (health)
        {
            health.ApplyDamage(Damage);
        }
    }

    public static void DealRadialDamage(float radius, Vector3 position, int Damage) // grenade damage
    {
        Collider[] hits = null;

        hits = Physics.OverlapSphere(position, radius); // create a sphere in the position with radius
    
        foreach(Collider hit in hits)
        {
            DealDamage(hit.gameObject, Damage); // call DealDamage function for any gameObject who's in the area
        }
    }
}
