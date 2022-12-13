// Copyright (c) Daniel Fernández 2022


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IManagedInstantiation : MonoBehaviour
{
    protected bool instantiated;

    public abstract void Instantiate();
}
