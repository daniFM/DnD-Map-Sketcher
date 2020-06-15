using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolMenu : MonoBehaviour
{
    [SerializeField] private GameObject brushes;
    [SerializeField] private GameObject sliderSize;
    [SerializeField] private GameObject sliderHeight;
    [SerializeField] private Button button;
    [SerializeField] private Button[] brushButtons;
    [SerializeField] private Text toolText;
    [SerializeField] private Text sizeText;
    [SerializeField] private Text heightText;

    private string selectionStr = "Selection";
    private string brushStr = "Brushes";
    private string sizeStr = "Size: ";
    private string heightStr = "Height: ";

    void Start()
    {
        UpdateSize(1);
        UpdateHeight(1);
        button.interactable = GameManager.instance.isDM;
    }

    void OnEnable()
    {
        GameController.OnToolChanged += UpdateTool;
    }

    void OnDisable()
    {
        GameController.OnToolChanged -= UpdateTool;
    }

    public void SwitchTool()
    {
        ToolType newTool = GameController.instance.SwitchTool();
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

    private void UpdateTool()
    {
        ToolType newTool = GameController.instance.Tool;

        switch(newTool)
        {
            case ToolType.selection:
                {
                    brushes.SetActive(false);
                    sliderSize.SetActive(false);
                    sliderHeight.SetActive(false);
                    toolText.text = selectionStr;
                    break;
                }
            case ToolType.brush:
                {
                    brushes.SetActive(true);
                    sliderSize.SetActive(true);
                    sliderHeight.SetActive(true);
                    toolText.text = brushStr;
                    break;
                }
        }
    }
}
