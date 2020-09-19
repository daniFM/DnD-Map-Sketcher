using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlsMenu : MonoBehaviour
{
    [SerializeField]
    private ControlButton[] toolButtons;

    [SerializeField]
    private GameObject configureButtonTooltip;

    private ControlButton currentButton;
    private readonly Array keyCodes = Enum.GetValues(typeof(KeyCode));

    // Start is called before the first frame update
    void Start()
    {
        toolButtons = GetComponentsInChildren<ControlButton>();
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
                        GameController.instance.controls.ChangeToolKey(currentButton.controlAction, keyCode);
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
        string[] actions = GameController.instance.controls.GetKeyNames();

        foreach(ControlButton button in toolButtons)
        {
            if(button.name != "ButtonBack")
            {
                if(keyCount == 10)
                {
                    keyCount++;
                    keyName = "CTRL + " + actions[keyCount];
                }
                else
                {
                    keyName = actions[keyCount];
                    keyCount++;
                }
                button.GetComponentInChildren<Text>().text = keyName;
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
