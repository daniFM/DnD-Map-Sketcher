﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleChildrenWithKey : MonoBehaviour
{
    public KeyCode key;
    public bool startActive = true;

    private bool active;

    void Start()
    {
        active = startActive;
        ToggleChildren();
    }

    void Update()
    {
        if(Input.GetKeyDown(key))
        {
            ToggleChildren();
        }
    }

    void ToggleChildren()
    {
        for(int i = 0; i < transform.childCount; ++i)
        {
            transform.GetChild(i).gameObject.SetActive(active);
        }
        active = !active;
    }
}