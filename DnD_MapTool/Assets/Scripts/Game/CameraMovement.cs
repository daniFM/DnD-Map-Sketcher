using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float sensitivityX = 0.3f;
    public float sensitivityY = 0.5f;

    private Vector3 accumulator;

    //void Start()
    //{
    //}

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        float mouseWheel = Input.GetAxis("Mouse ScrollWheel");

        // Grid movement - MOVE TO OTHER SCRIPT
        if(Input.GetMouseButton(1))
        {
            accumulator += new Vector3(mouseX * sensitivityX, 0, mouseY * sensitivityY);

            if(accumulator.magnitude > 1)
            {
                transform.position -= (Quaternion.AngleAxis(45, Vector3.up) * accumulator).Round();
                accumulator = Vector3.zero;
            }
        }

        if(mouseWheel > 0)
        {
            transform.Translate(0, 1, 0);
        }
        else if(mouseWheel < 0)
        {
            transform.Translate(0, -1, 0);
        }
    }
}
