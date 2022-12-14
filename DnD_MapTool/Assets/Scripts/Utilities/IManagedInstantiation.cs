// Copyright (c) 2022 Daniel Fernández Marqués

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IManagedInstantiation : MonoBehaviour
{
    protected bool instantiated;

    public abstract void Instantiate();
}
