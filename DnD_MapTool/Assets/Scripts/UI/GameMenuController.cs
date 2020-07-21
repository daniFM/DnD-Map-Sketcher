using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMenuController : MonoBehaviour
{
    [SerializeField] private ToolMenu toolMenu;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject playersMenu;
    [SerializeField] private Text tooltip;

    void Start()
    {
        tooltip.gameObject.SetActive(false);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu.SetActive(!pauseMenu.activeSelf);
        }
        else if(Input.GetKeyDown(GameController.instance.controls.keySwitchTool))
        {
            toolMenu.SwitchTool();
        }
        else if(Input.GetKeyDown(GameController.instance.controls.keyHeight) && GameController.instance.Tool == ToolType.brush)
        {
            toolMenu.ToggleHeightPlane();
        }
    }

    public void ShowTooltip(string message, float time = 5)
    {
        StartCoroutine(IShowTooltip(message, time));
    }

    private IEnumerator IShowTooltip(string message, float time)
    {
        while(tooltip.gameObject.activeSelf)
            yield return null;

        tooltip.gameObject.SetActive(true);
        tooltip.text = message;

        yield return new WaitForSeconds(time);

        tooltip.gameObject.SetActive(false);
    }

    #region Save/Load

    public void SaveLoadMenu()
    {
        
    }

    public void Save()
    {
        JSONSaver.Save(TileController.instance.GetLastSnapshot(), "test1");
    }

    public void Load()
    {
        TileController.instance.LoadSnapshot(JSONSaver.Load<TileData>("test1"), false); // works with false
    }

    #endregion
}
