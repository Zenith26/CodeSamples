using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    // this is SerializeField for class, struct, enum
    [System.Serializable] // allows us to change the values of instances of this class inside of the unity inspector || Specifically allows it to be added to inspector on SerializeField
    public class Wave   // create custom class (WITH S.Serializable and public Wave[], this will show on inspector)
    {
        public string name; // name of the wave
        public GameObject enemy; // the enemy of the GameObject we will put
        public int count; // amount of enemies
        public float rate; // spawn rate
        public float timeTillNextWave = 20.0f; // time till the next wave
    }
    // S.Serializable is the one who can allow to change value of instance in the Wave class
    public Wave[] waves; // this is just to declare it public (Like every public you make in every script DUH..)

    // will be use for Wave[] waves arrays index number
    private int waveIndex = -1; // that way we increment on the if statement on StartWave();

    public Transform[] spawnPoints; // arrays of spawn position

    [SerializeField] float minDistance = 10; // to check if each spawnPoints are further than minDistance, if yes then add it to list

    private int numberEnemySpawn = 0; // number of enemy that has been spawned in each wave, to check if in SpawnEnemy(), will set back to 0 once go to StartWave()

    // new waves for randomize
    private bool normalWaveFin = false;
    public GameObject normalEnemy;
    public GameObject tankEnemy;
    public GameObject sprintEnemy;

    void Start()
    {
        if (spawnPoints.Length == 0) // Check if there is no spawn point
        {
            Debug.LogError("No spawn points referenced");
        }

        StartWave();
    }

    void SpawnEnemy()
    {
        if (normalWaveFin) // once all normal wave has been spawned, GACHA
        {
            // edit what professional wave gonna be
            waves[waveIndex].count = 10;
            waves[waveIndex].rate = 2f;
            waves[waveIndex].timeTillNextWave = 23.0f;

            int shuffleEnemy = Random.Range(1, 6);
            Debug.Log(shuffleEnemy);
            switch (shuffleEnemy)
            {
                case 1: case 2:
                    waves[waveIndex].enemy = normalEnemy;
                    break;
                case 3: case 4:
                    waves[waveIndex].enemy = sprintEnemy;
                    break;
                case 5:
                    waves[waveIndex].enemy = tankEnemy;
                    break;
            }
        }
        Debug.Log("Spawning Enemy: " + waves[waveIndex].enemy.name);

        if (numberEnemySpawn >= waves[waveIndex].count) // bigger than the amount of enemy in a wave (count)
        {
            return;
        }
        // SPAWNING THE ENEMY
        Instantiate(waves[waveIndex].enemy, GetSpawnPointMinDistance(), Quaternion.identity); // enemy, spawnPos, spawnRot
        numberEnemySpawn++; // then it will increment until the number of enemy in a wave (count) is lower than numberEnemySpawn
    }

    Vector3 GetSpawnPointMinDistance()
    {
        List<Transform> _usableSpawn = new List<Transform>(); // create a list

        // using player position, doesn't change, so created here
        Vector3 _playerLoc = GameManager.Instance.GetPlayer().transform.position;

        foreach (Transform point in spawnPoints) // we call it point for the spawnPoints
        {
            // getting distance between point and current spawn pos
            float newDistance = Vector3.Distance(point.position, _playerLoc);

            if (newDistance > minDistance) // basically if a spawner is further than the minDistance. it will be added to the list.
            {
                _usableSpawn.Add(point);
            }
        }
        // range between 0 and the list of spawn that being added
        int indexToUse = Random.Range(0, _usableSpawn.Count - 1);

        return _usableSpawn[indexToUse].transform.position; // at the end of the function this will pick one number from a list of wave
    }

    void StartWave() // will be called from Start as we be doing InvokeRepeating
    {
        numberEnemySpawn = 0;
        CancelInvoke(); // unless you don't want to specify what function to be stop. This will stop everything. CancelInvoke("funcName") to cancel a function.

        // if waveIndex is not passing through the number of waves
        if (waveIndex < waves.Length - 1) // since waveIndex is (0-3) the length should be 4 in terms of how many. we should minus it by 1 so that it will not run waveIndex 4.
        {                                // you could use <= instead of < as long as you remove -1
            waveIndex++;    // increment the wave
        }
        else
        {
            normalWaveFin = true;
        }
        Debug.Log("Wave: " + waves[waveIndex].name);

        //THE RATE FOR EVERY ENEMY TO BE SPAWN   //call the first delay   //after that it will be this forever until the CancelInvoke();
        InvokeRepeating("SpawnEnemy", waves[waveIndex].rate, waves[waveIndex].rate);

        //Invoke the function by amount of timeTillNextWave. Until it execute, it will still run InvokeRepeating
        Invoke("StartWave", waves[waveIndex].timeTillNextWave); // this one will do it after the timeTillNextWave, since the start of the function is CancelInvoke(); so it start over again.
    }
}
