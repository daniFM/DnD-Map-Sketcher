using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolMenu : MonoBehaviour
{
    public GameObject brushes;
    public GameObject slider;
    public Text toolText;
    public Text sizeText;

    private string selectionStr = "Selection";
    private string brushStr = "Brushes";
    private string sizeStr = "Size: ";

    void Start()
    {
        UpdateSize(1);
    }

    public void SwitchTool()
    {
        ToolType newTool = GameController.instance.SwitchTool();

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

    public void UpdateSize(float newSize)
    {
        sizeText.text = sizeStr + newSize;
    }
}
