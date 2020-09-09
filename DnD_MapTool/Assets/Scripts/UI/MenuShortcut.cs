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
        if(Input.GetKeyDown(GameController.instance.controls.keyShortcut) && !parentMenu.activeSelf)
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
