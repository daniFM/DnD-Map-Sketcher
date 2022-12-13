// Copyright (c) Daniel Fernández 2022


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleObjectsWithKey : MonoBehaviour
{
    public ControlAction key;
    public GameObject[] objects;

    private bool active = true;

    void Update()
    {
        if(GameController.instance.controls.GetKeyDown(key))
        {
            ToggleObjects();
        }
    }

    void ToggleObjects()
    {
        active = !active;
        foreach(GameObject go in objects)
        {
            go.SetActive(active);
        }
    }
}
