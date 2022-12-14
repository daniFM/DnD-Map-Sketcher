// Copyright (c) 2022 Daniel Fernández Marqués
// Licensed under the GNU General Public License (GPL) version 3. See the LICENSE file for more details.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IManagedInstantiation : MonoBehaviour
{
    protected bool instantiated;

    public abstract void Instantiate();
}
