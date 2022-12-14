// Copyright (c) 2022 Daniel Fernández Marqués
// Licensed under the GNU General Public License (GPL) version 3. See the LICENSE file for more details.

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
        else if(GameController.instance.controls.GetKeyDown(ControlAction.CycleTool))
        {
            toolMenu.SwitchTool();
        }
        else if((GameController.instance.controls.GetKeyDown(ControlAction.HeightAid)) && GameController.instance.Tool == ToolType.brush)
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
}
