using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolMenu : MonoBehaviour
{
    [SerializeField] private GameObject brushes;
    [SerializeField] private GameObject slider;
    [SerializeField] private Button button;
    [SerializeField] private Button[] brushButtons;
    [SerializeField] private Text toolText;
    [SerializeField] private Text sizeText;

    private string selectionStr = "Selection";
    private string brushStr = "Brushes";
    private string sizeStr = "Size: ";

    void Start()
    {
        UpdateSize(1);
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
                    slider.SetActive(false);
                    toolText.text = selectionStr;
                    break;
                }
            case ToolType.brush:
                {
                    brushes.SetActive(true);
                    slider.SetActive(true);
                    toolText.text = brushStr;
                    break;
                }
        }
    }
}
