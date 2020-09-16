using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlsMenu : MonoBehaviour
{
    [SerializeField]
    private Button[] toolButtons;

    [SerializeField]
    private GameObject configureButtonTooltip;

    private string currentButton;
    private KeyCode currentKeyPressed;
    private readonly Array keyCodes = Enum.GetValues(typeof(KeyCode));

    // Start is called before the first frame update
    void Start()
    {
        currentKeyPressed = KeyCode.None;
        toolButtons = GetComponentsInChildren<Button>();
        WriteNewKeysToMenu();
    }

    // Update is called once per frame
    void Update()
    {
        if (configureButtonTooltip.activeInHierarchy)
        {
            if (Input.anyKeyDown)
            {
                foreach (KeyCode keyCode in keyCodes)
                {
                    if (Input.GetKey(keyCode))
                    {
                        configureButtonTooltip.SetActive(false);
                        GameController.instance.controls.ChangeToolKey(currentButton, keyCode);
                        WriteNewKeysToMenu();
                        break;
                    }
                }
            }
        }
    }

    // To Do
    void WriteNewKeysToMenu()
    {
        //int keyCount = 0;
        //string keyName = "";
        //foreach (Button button in toolButtons)
        //{
        //    if (button.name != "ButtonBack")
        //    {
        //        if (keyCount == 10)
        //        {
        //            keyCount++;
        //            keyName = "CTRL + " + GameController.instance.controls.controlsConfig[keyCount].GetMainKey().ToString();
        //        }
        //        else
        //        {
        //            keyName = GameController.instance.controls.controlsConfig[keyCount].GetMainKey().ToString();
        //            keyCount++;
        //        }
        //        button.GetComponentInChildren<Text>().text = keyName;
        //        button.onClick.AddListener(delegate ()
        //        {
        //            configureButtonTooltip.SetActive(true);
        //            configureButtonTooltip.GetComponentInChildren<Text>().text = button.name + ": ";
        //            currentButton = button.name;
        //        }
        //        );
        //    }
        //}
    }
}
