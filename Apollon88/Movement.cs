using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] float Speed = 0; // player speed
    public Vector3 moveVelocity;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // for MovePlayer, we will use "PlayerController" you will ref this with
    // input.getaxis horizontal and vertical as the constructor
    public void MovePlayer(float x, float y) 
    {
        Vector3 moveInput;
        moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        //Vector3 moveVelocity; // call new Vector to store it with speed

        moveVelocity = moveInput * Speed;

        rb.velocity = moveVelocity; // then put it to the rigidbody velocity
    }
    //public void LookAtMouse()
    //{
    //    rb.angularVelocity = Vector3.zero;

    //    Vector3 _mousePosition = Input.mousePosition; // get the input from the mouse position

    //    //_mousePosition.z = transform.position.z - Camera.main.transform.position.z;
    //    _mousePosition = Camera.main.ScreenToWorldPoint(_mousePosition); // takes the _mousePosition for it to go world
    //    _mousePosition.y = transform.position.y;

    //    if(_mousePosition == transform.position)
    //    {
    //        return;
    //    }
    //                               //create a rotation which rotate from this direction to other direction                         
    //    Quaternion _lookRotation = Quaternion.FromToRotation(Vector3.forward, _mousePosition - transform.position);
    //    _lookRotation.x = 0;
    //    //set the rotation of x to 0
    //    transform.rotation = _lookRotation;
    //}

    public void MousePoint()
    {
        var mouse = Input.mousePosition;
        var screenPoint = Camera.main.WorldToScreenPoint(transform.localPosition);
        var offset = new Vector2(mouse.x - screenPoint.x, mouse.y - screenPoint.y);
        var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;

        //basically the way the player could react with the mouse cursor rotation before it will be quaternion.
        Vector3 LookAngle = new Vector3(transform.rotation.x, -angle + 90, transform.rotation.z); // I had to add 90 since the char is 90 further than the mouse

        transform.rotation = Quaternion.Euler(LookAngle);//transform.rotation.x, transform.rotation.y, angle);
    }
}
