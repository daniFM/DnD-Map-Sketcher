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

    // Start is called before the first frame update
    void Start()
    {
        toolButtons = GetComponentsInChildren<Button>();
        foreach (Button button in toolButtons)
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

    // Update is called once per frame
    void Update()
    {
        string inputString;
        if (configureButtonTooltip.activeInHierarchy)
        {
            inputString = Input.inputString;
            if(inputString != "")
            {
                configureButtonTooltip.SetActive(false);
            }
        }
    }
}
