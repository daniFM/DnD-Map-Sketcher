// Copyright (c) 2022 Daniel Fernández Marqués

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolMenu : MonoBehaviour
{
    [SerializeField] private GameObject brushes;
    [SerializeField] private GameObject sliderSize;
    [SerializeField] private GameObject sliderHeight;
    [SerializeField] private GameObject tokenPanel;
    //[SerializeField] private ToolButton[] toolButtons;
    [SerializeField] private Button[] brushButtons;
    [SerializeField] private Text toolText;
    [SerializeField] private Text sizeText;
    [SerializeField] private Text heightText;
    [SerializeField] private GameObject heightPlane;

    private const string selectionStr = "Selection";
    private const string brushStr = "Brushes";
    private const string sizeStr = "Size: ";
    private const string heightStr = "Height: ";

    //void OnEnable()
    //{
    //    GameController.OnToolChanged += UpdateTool;
    //}

    //void OnDisable()
    //{
    //    GameController.OnToolChanged -= UpdateTool;
    //}

    void Start()
    {
        UpdateSize(1);
        UpdateHeight(1);
    }

    public void SetTool(ToolType newTool)
    {
        GameController.instance.SetTool(newTool);
    }

    public void SwitchTool()
    {
        ToolType newTool = GameController.instance.SwitchTool();

        if(newTool == ToolType.selection)
            heightPlane.SetActive(false);
    }

    public void ToggleHeightPlane()
    {
        heightPlane.SetActive(!heightPlane.activeSelf);
    }

    public void UpdateSize(float newSize)
    {
        sizeText.text = sizeStr + newSize;
    }

    public void UpdateHeight(float newHeight)
    {
        heightText.text = heightStr + newHeight;
    }

    public void ReActivateBrushBruttons()
    {
        foreach(Button b in brushButtons)
        {
            b.interactable = true;
        }
    }

    //private void UpdateTool()
    //{
    //    ToolType newTool = GameController.instance.Tool;

    //    switch(newTool)
    //    {
    //        case ToolType.selection:
    //            {
    //                brushes.SetActive(false);
    //                sliderSize.SetActive(false);
    //                sliderHeight.SetActive(false);
    //                toolText.text = selectionStr;
    //                break;
    //            }
    //        case ToolType.brush:
    //            {
    //                brushes.SetActive(true);
    //                sliderSize.SetActive(true);
    //                sliderHeight.SetActive(true);
    //                toolText.text = brushStr;
    //                break;
    //            }
    //        case ToolType.token:
    //            {
    //                tokenPanel.SetActive(!tokenPanel.activeSelf);
    //                break;
    //            }
    //    }
    //}
}
