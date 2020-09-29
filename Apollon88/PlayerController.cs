using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Movement movement;
    [SerializeField] CameraFollow camFollow = null; // This one is to call and modify the camera follow
    [SerializeField] GameObject playerCamera = null; // This one is to drag the CameraContainer out of the parent which is the player

    public static PlayerController playerController;
    
    public float Speed = 5f; // technically this one is for the camFollow

    WeaponComponent weaponComp;

    public GameObject leftBooster;
    public GameObject rightBooster;
    private AudioSource boost;


    private void Awake()
    {
        playerController = this; // this playerController is the static, just safety check
    }

    void Start()
    {
        movement = GetComponent<Movement>();
        weaponComp = GetComponent<WeaponComponent>();
        boost = GetComponent<AudioSource>();

        if (playerCamera)
        {
            // by making it null, CameraContainer is out of the parent (the parent is the Player)
            playerCamera.transform.parent = null;
        }
    }

    void Update()
    {
        CheckInput();

        if (PauseUI.isPaused) // if paused, then it will stop the sound. Before that it was bug at that time where if you move and pause, the sound is still playing
        {
            boost.Stop();
        }
    }

    void CheckInput()
    {
        if (!PauseUI.isPaused) // if pause then no look direction and no scroll weapon
        {
            // this symbol is to check if is it null or not
            movement?.MousePoint();

            // ------ SCROLL WEAPON ------
            if (weaponComp)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    weaponComp.StartFire();
                }

                if (Input.GetMouseButtonUp(0))
                {
                    weaponComp.StopFire();
                }

                //creating local var that detects input, just shortening the GetAxis bit
                float mouseWheel = Input.GetAxis("Mouse ScrollWheel");

                if (mouseWheel > 0)
                {
                    weaponComp.SwitchWeapon(true);
                }

                if (mouseWheel < 0)
                {
                    weaponComp.SwitchWeapon(false);
                }
                // --------------------------
            }
        }


        
    }

    void FixedUpdate()
    {
        if (movement)
        {
            movement.MovePlayer(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            //this is where the ship sound and booster on
            float moveSpeed = 0;
            moveSpeed = movement.moveVelocity.magnitude / Speed;

            if(moveSpeed > 0)
            {
                leftBooster.SetActive(true);
                rightBooster.SetActive(true);
                if (!boost.isPlaying) // This one will saves the audio from BUZZING
                {
                    boost.Play();
                }
            }
            else
            {
                leftBooster.SetActive(false);
                rightBooster.SetActive(false);
                boost.Stop();
            }
        }

        if (camFollow)
        {
            float _normalizedSpeed = 0;
            // get the player velocity with magnitude and divide with speed
            _normalizedSpeed = movement.moveVelocity.magnitude / Speed;
            // now alpha speed will be depends on the movement of the player (move then camera increase, stop then camera decrease)
            camFollow.ResizeCamera(_normalizedSpeed);
            camFollow.FollowPlayer();
        }
        else
        {
            //Good for safety check
            Debug.LogError(name + "camFollow reference is NULL");
        }
    }
}
