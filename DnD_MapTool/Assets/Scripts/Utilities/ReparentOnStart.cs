﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReparentOnStart : MonoBehaviour
{
    public Transform newParent;

    void Start()
    {
        transform.parent = newParent;
    }
}