using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effects : MonoBehaviour
{
    [SerializeField] float lifeTime = 3.0f;

    [SerializeField] GameObject[] VFX; // that takes the particles

    List<GameObject> spawnedEffects = new List<GameObject>(); // once a particle spawned, add it to here

    void Start()
    {
        //basically each GameObject (called effect) in the VFX
        foreach(GameObject effect in VFX)
        {
            //on this condition, it will spawn 3 gameObject. Effects, FX, FX_Smoke. Eventhough you list 2 stuff, i believe this gameObject (Effects) will join in
            GameObject _neweffect = Instantiate(effect, transform.position, transform.rotation); // summon the effect on the VFX array

            //store the particle (effect) on this list
            spawnedEffects.Add(_neweffect);
        }

        Invoke("KillObjects", lifeTime);
    }

    void KillObjects()
    {
        foreach (GameObject effect in spawnedEffects)
        {
            Destroy(effect); // Destroy each gameobject that came from the spawnedEffects
        }
        Destroy(gameObject); // Destroy this gameobject which will be Effects in the inspector
    }
}
