using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonFX : MonoBehaviour
{
    public AudioSource buttonAudio;
    public AudioClip hoverFX;
    public AudioClip clickFX;

    public void HoverSound()
    {
        buttonAudio.PlayOneShot(hoverFX);
    }

    public void ClickSound()
    {
        // if i click with left mouse, if i don't do this, it will play sound when i click with right or middle mouse
        if (Input.GetMouseButtonDown(0))
        {
            buttonAudio.PlayOneShot(clickFX);
        } 
    }
}
