using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealthUI : MonoBehaviour
{
    PlayerHealth pHealth;
    public TextMeshProUGUI healthText;

    public GameObject ComboKill; // reference the Combo Kill

    private void Start()
    {
        pHealth = GameObject.FindObjectOfType<PlayerHealth>(); // get the PlayerHealth from the gameobject that has player health
        pHealth.OnDamaged += ModifyHealth;  // subscribe it
        ModifyHealth(pHealth.health); // start the game as max health

        ComboKill.GetComponent<ComboKill>().combo = 0; // set it to 0 when start
    }

    void ModifyHealth(float playerHealth) // this is where it will call the health number everytime it get hits
    {
        healthText.text = pHealth.health.ToString();
        ResetCombo();
    }

    void ResetCombo()
    {
        ComboKill.GetComponent<ComboKill>().currentCombo = 1; // set the current combo back to 1

        ComboKill.GetComponent<ComboKill>().combo = -1; // had to set to -1, since If i put 0 then it will do++ everytime enemy die from attacking player. you know...  DELEGATE called

        ComboKill.GetComponent<ComboKill>().cooldown = 0; // set cooldown back to 0

        ComboKill.GetComponent<ComboKill>().comboMultiplier = 0; // set multiplier to 0, that way it will not get point when enemy attack the player
    }

    //private void Start()
    //{
    //    //pHealth.GetComponent<PlayerHealth>();
    //    GameManager.manager.GetPlayerHealth();
    //}

    //private void Update()
    //{
    //    healthText.text = GameManager.manager.GetPlayerHealth().ToString;
    //}
}
