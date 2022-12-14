// Copyright (c) 2022 Daniel Fernández Marqués

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlsMenu : IManagedInstantiation
{
    [SerializeField]
    private ControlButton[] toolButtons;
    [SerializeField]
    private GameObject configureButtonTooltip;

    private ControlButton currentButton;
    private readonly Array keyCodes = Enum.GetValues(typeof(KeyCode));

    public override void Instantiate()
    {
        if(!instantiated)
        {
            instantiated = true;
            toolButtons = GetComponentsInChildren<ControlButton>();
            WriteNewKeysToMenu();
        }
    }

    void Start()
    {
        if(!instantiated)
        {
            instantiated = true;
            toolButtons = GetComponentsInChildren<ControlButton>();
            WriteNewKeysToMenu();
        }
    }

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
                        GameController.instance.controls.RemapControl(currentButton.controlAction, keyCode);
                        WriteNewKeysToMenu();
                        break;
                    }
                }
            }
        }
    }

    void WriteNewKeysToMenu()
    {
        int keyCount = 0;
        string keyName = "";

        foreach(ControlButton button in toolButtons)
        {
            if(button.name != "ButtonBack")
            {
                keyName = GameController.instance.controls.GetKeyName(button.controlAction);
                keyCount++;

                button.text.text = keyName;
                button.button.onClick.AddListener(delegate ()
                {
                    configureButtonTooltip.SetActive(true);
                    configureButtonTooltip.GetComponentInChildren<Text>().text = button.name + ": ";
                    currentButton = button;
                }
                );
            }
        }
    }
}
