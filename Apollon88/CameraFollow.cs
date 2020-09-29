using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform targetObject = null; // get the CameraFollowLocation transform

    [Header("Camera Sizes")] // this will create a header on the inspector

    [SerializeField]
    float minSize = 5;

    [SerializeField]
    float maxSize = 7;

    float lastAlpha = 0; // this will be the one who decide whether the camera increase or decrease

    [Header("Change Size Speed")]

    [SerializeField]
    float decreaseRate = 0.5f;  // differs from the old code since I did change it in the inspector, LEGIT 0,05 is too slow hahaha

    [SerializeField]
    float increaseRate = 0.75f;


    public void ResizeCamera(float alpha)
    {
        float newAlpha = 0;

        if(alpha >= lastAlpha)
        {
            newAlpha = lastAlpha + (Time.deltaTime * increaseRate);
        }
        else
        {
            newAlpha = lastAlpha - (Time.deltaTime * decreaseRate);
        }

        // Clamp = making sure the newAlpha value is between value 1 and 2 (0 , 1)
        newAlpha = Mathf.Clamp(newAlpha, 0, 1);

        lastAlpha = newAlpha; // so that the next time the player don't move it will decrease since the latest size is increased

        // resize the camera
        Camera.main.orthographicSize = Mathf.SmoothStep(minSize, maxSize, lastAlpha);
        //SmoothStep is the same as Lerp but will speed up from start to middle and slows down towards the end. its run more slope-like-curve
    }

    public void FollowPlayer()
    {
        //check if it will the player is null
        if(targetObject == null)
        {
            return;
        }

        Vector3 _Location = targetObject.position;
        //Distance returns the distance between A and B
        float _alpha = Vector3.Distance(transform.position, _Location);
        //Lerp is linear so it will move by the speed of _alpha CONSTANT
        Vector3 _newLocation = Vector3.Lerp(transform.position, _Location, _alpha);

        // set it to the camera transform
        transform.position = _newLocation;
    }
}
