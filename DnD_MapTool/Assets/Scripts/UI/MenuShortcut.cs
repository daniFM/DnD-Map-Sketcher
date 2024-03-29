// Copyright (c) 2022 Daniel Fernández Marqués
// Licensed under the GNU General Public License (GPL) version 3. See the LICENSE file for more details.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuShortcut : MonoBehaviour
{
    public KeyCode key;
    public GameObject menu;
    public GameObject parentMenu;

    //public bool active = false; 

    void Update()
    {
        if(GameController.instance.controls.GetKeyDown(0) && !parentMenu.activeSelf)
        {
            ToggleSubMenu();
        }
    }

    private void ToggleSubMenu()
    {
        //active = !active;
        menu.SetActive(!menu.activeSelf);
    }
}
