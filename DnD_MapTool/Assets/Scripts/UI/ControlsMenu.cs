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
    private ControlsScriptableObject controlsScriptableObject;
    private KeyCode currentKeyPressed;
    private readonly Array keyCodes = Enum.GetValues(typeof(KeyCode));

    // Start is called before the first frame update
    void Start()
    {
        currentKeyPressed = KeyCode.None;
        toolButtons = GetComponentsInChildren<Button>();
        foreach (Button button in toolButtons)
        {
            if(button.name != "ButtonBack")
            {
                button.onClick.AddListener(delegate ()
                    {
                        configureButtonTooltip.SetActive(true);
                        configureButtonTooltip.GetComponentInChildren<Text>().text = button.name + ": ";
                        currentButton = button.name;
                    }
                );
            }
        }
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
                        Debug.Log("KeyCode down: " + keyCode);
                        break;
                    }
                }
            }
        }
    }
}
