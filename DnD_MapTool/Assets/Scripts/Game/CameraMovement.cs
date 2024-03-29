// Copyright (c) 2022 Daniel Fernández Marqués
// Licensed under the GNU General Public License (GPL) version 3. See the LICENSE file for more details.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float sensitivityX = 0.3f;
    public float sensitivityY = 0.5f;
    [SerializeField] private float rotationSpeed = 1;
    [SerializeField] private float zoomSensitivity = 1;
    [SerializeField] private float maxZoom = 10;
    [SerializeField] private float minZoom = 1;
    [SerializeField] private AnimationCurve rotationTween;
    [SerializeField] private Transform tCamera;

    private Camera[] cameras;
    private Vector3 accumulator;
    private bool rotating;
    private int height;

    public static Action<int> HeightChanged;

    void Start()
    {
        cameras = tCamera.GetComponentsInChildren<Camera>();
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        float mouseWheel = Input.GetAxis("Mouse ScrollWheel");

        // Grid movement
        if(GameController.instance.controls.GetKey(ControlAction.Pan) && !rotating)
        {
            // Multiply by rotation to make it local
            accumulator += transform.rotation * new Vector3(mouseX * sensitivityX, 0, mouseY * sensitivityY);

            if(accumulator.magnitude > 1)
            {
                // AngleAxis to take camera rotation into account
                transform.position -= (Quaternion.AngleAxis(45, Vector3.up) * accumulator).Round();
                accumulator = Vector3.zero;
            }
        }

        // Zoom
        if(mouseWheel != 0 && Input.GetKey(KeyCode.LeftControl))
        {
            foreach(Camera camera in cameras)
            {
                camera.orthographicSize = Mathf.Clamp(camera.orthographicSize - mouseWheel * zoomSensitivity, minZoom, maxZoom);
            }
            //Debug.Log("Camera zoom: " + camera.orthographicSize);

            GameController.instance.outlineController.CameraCorrection(cameras[0].orthographicSize);
        }
        // Up-down movement
        else if(mouseWheel > 0 && GameManager.instance.isDM)
        {
            transform.Translate(0, 1, 0);
            HeightChanged?.Invoke(++height);
        }
        else if(mouseWheel < 0 && GameManager.instance.isDM)
        {
            transform.Translate(0, -1, 0);
            HeightChanged?.Invoke(--height);
        }

        // Camera rotation
        if(!rotating)
        {
            if(GameController.instance.controls.GetKeyDown(ControlAction.RotateLeft))
            {
                StartCoroutine(DoRotation(-90));
            }
            else if(GameController.instance.controls.GetKeyDown(ControlAction.RotateRight))
            {
                StartCoroutine(DoRotation(90));
            }
        }
    }

    private IEnumerator DoRotation(float angle)
    {
        rotating = true;

        float sign = Mathf.Sign(angle);
        angle = Mathf.Abs(angle);
        float t_angle = 0;
        float acc_angle = 0;
        Vector3 initialPos = tCamera.position;
        Quaternion initialRot = tCamera.rotation;
        Vector3 direction;
        Quaternion rot;

        while(acc_angle <= angle)
        {
            t_angle = acc_angle / angle;
            acc_angle += Time.deltaTime * rotationSpeed * rotationTween.Evaluate(t_angle);

            direction = initialPos - transform.position;
            rot = Quaternion.AngleAxis(acc_angle * sign, Vector3.up);
            tCamera.transform.position = transform.position + rot * direction;
            tCamera.transform.rotation = initialRot * Quaternion.Inverse(initialRot) * rot * initialRot;

            yield return null;
        }
        // Rotate to exact end rotation
        direction = initialPos - transform.position;
        rot = Quaternion.AngleAxis(angle * sign, Vector3.up);
        tCamera.transform.position = transform.position + rot * direction;
        tCamera.transform.rotation = initialRot * Quaternion.Inverse(initialRot) * rot * initialRot;

        // Set controller rotation without moving the camera
        tCamera.parent = null;
        transform.eulerAngles = new Vector3(0, tCamera.rotation.eulerAngles.y - 45, 0);
        tCamera.parent = transform;

        rotating = false;
    }
}
