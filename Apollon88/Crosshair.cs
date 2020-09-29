using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    public GameObject crosshairs;
    private Vector3 target;

    private void Start()
    {
        // mouse cursor disable
        Cursor.visible = false;

        // find crosshair gameobject in the inspector by name
        crosshairs = GameObject.Find("Crosshair");
    }
    void Update()
    {
        //Debug.Log(Input.mousePosition); // Check mouse position
                                                                                // mouse position x         mouse position y   just default transform for z since 2d mouse doesn't use z
        target = transform.GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z));
        // since my crosshair moves forward by z so instead of target.y, i switch to z making y takes zero
        crosshairs.transform.position = new Vector3(target.x, 0, target.z);
    }
}
