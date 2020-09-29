using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// an enum that starts with 0

enum EAIState
{
    Patrol,
    Attack,
    Stunned
}

[RequireComponent(typeof(NavMeshAgent))] // basically any gameobject that attached to this script will auto getcomponent NavMeshAgent
public class AIBase : MonoBehaviour
{
    EAIState currentState = EAIState.Patrol;

    NavMeshAgent navAgent = null;

    GameObject Target = null;

    float UpdateRate = 0.05f;

    [SerializeField] float StunDelay = 2.0f; // the delay once it get hit (DIDN'T USE, MAYBE FOR FUTURE CHAR)

    [SerializeField] float attackRange = 1.5f; // the range of the attack enemy could do near the player

    [SerializeField] int impactDamage = 1; // damage given

    HealthComponent health;

    void Awake()
    {
        //starting fresh with null on the top of the script. this will get the component. Much safer 
        navAgent = GetComponent<NavMeshAgent>();

        health = GetComponent<HealthComponent>();

        if (health)
        {                   // from this script Death, not the HealthComponent.
            health.OnDeath += Death; // this will subscribe to HealthComponent OnDeath
        }

        StartExecution();
    }

    private void Start()
    {
        GetPlayer();

        //on spawn, increase ai count
        GameManager.Instance.ModifyEnemyCount(true); // True then the enemyCounter will go up
    }

    void GetPlayer()
    {
        GameObject _Player = PlayerController.playerController.gameObject; // get it from the PlayerController gameobject which is the player
        if (_Player)
        {
            Target = _Player;
            return;
        }

        Debug.LogError(name + " PlayerController reference is INVALID");
    }

    void StartExecution()
    {
        InvokeRepeating("ExecuteState", 0, UpdateRate);
        //Starts from time (0)
        //Repeat every repeatRate (UpdateRate)
    }

    void ExecuteState()
    {
        switch (currentState) // check if the currentState is one of the case
        {
            case EAIState.Patrol: //if Patrol, then call the PatrolState
                PatrolState();
                break;

            case EAIState.Attack: // if Attack, then call the AttackState
                AttackState();
                break;

            case EAIState.Stunned: // if Stunned, then call the StunState
                StunState();
                break;
         // theres else which is 'default' but I will not put it since there will be no need
        }
    }

    void PatrolState()
    {
        if (Target == null) return; // only one line so just do like this, line below will be a false statement

        navAgent.SetDestination(Target.transform.position); // using AI system to move the target

        //Deal Damage
        float _distance = 0;

        _distance = Vector3.Distance(gameObject.transform.position, Target.transform.position); // returns the distance from point A to B

        if(_distance < attackRange)         // if the distance is lesser than the attackRange, initiate AttackState. P.S not using if check collider tag anymore
        {
            currentState = EAIState.Attack;
        }
    }

    void AttackState()
    {
        GameplayStatics.DealDamage(Target, impactDamage); // damage to player (player, 1)
        GameplayStatics.DealDamage(gameObject, 100);       // damage to self/enemy (enemy, 100)
        Destroy(gameObject);
    }

    void StunState()
    {
        CancelInvoke();

        Invoke("UnStun", StunDelay);
        //Pause execution, resume after delay
    }

    void UnStun()
    {
        currentState = EAIState.Patrol;
        StartExecution();
    }

    // death function is called via delegate binding
    void Death()
    {
        //on death, reduce AI count
        GameManager.Instance.ModifyEnemyCount(false); // false then enemyCounter go down but NumberEnemiesKilled go up
    }
}
