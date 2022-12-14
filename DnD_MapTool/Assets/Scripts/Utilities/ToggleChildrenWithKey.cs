// Copyright (c) 2022 Daniel Fernández Marqués
// Licensed under the GNU General Public License (GPL) version 3. See the LICENSE file for more details.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleChildrenWithKey : MonoBehaviour
{
    public ControlAction key;
    public bool startActive = true;

    private bool active;

    void Start()
    {
        active = startActive;
        ToggleChildren();
    }

    void Update()
    {
        if(GameController.instance.controls.GetKeyDown(key))
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
