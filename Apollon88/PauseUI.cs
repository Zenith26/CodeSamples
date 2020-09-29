using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PauseUI : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject settingsMenu;

    public AudioMixerSnapshot paused;
    public AudioMixerSnapshot unpaused;

    public static bool isPaused = false;

    PlayerHealth playerHealth;
    WeaponComponent weaponComp;

    private void Start()
    {
        unpaused.TransitionTo(0.01f);
        if (isPaused)
        {
            isPaused = false;
        }
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(false);
        playerHealth = GameObject.FindObjectOfType<PlayerHealth>();
        weaponComp = GameObject.FindObjectOfType<WeaponComponent>();
    }

    private void Update()
    {
        if(playerHealth.health > 0) // if player health is not zero, then we can pause
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!settingsMenu.activeInHierarchy) // if settings is not in hierachy then you could resume or pause when press escape
                {
                    if (isPaused)
                    {
                        Resume();
                    }
                    else
                    {
                        Pause();
                        weaponComp.StopShooting(); // Fix bugs where if you hold mouse and esc to pause and esc to unpause, it will shoot infinitely
                    }
                }
                else // if not then once press escape it will move back to Pause()
                {
                    settingsMenu.SetActive(false);
                    pauseMenu.SetActive(true);
                }
            }
        }
    }
    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        //Lowpass();
        isPaused = false;
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        //Lowpass();
        isPaused = true;
    }

    //public void Lowpass() // unpaused lowpass value are not consistent, fix it later (WILL NOT USE THIS IN GAME)
    //{
    //    if(Time.timeScale == 0)
    //    {
    //        paused.TransitionTo(.01f); // transition to paused snapshot made in AudioMixer
    //    }
    //    else
    //    {
    //        unpaused.TransitionTo(.01f); // transition to unpaused snapshot made in AudioMixer
    //    }
    //}
}
