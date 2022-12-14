// Copyright (c) 2022 Daniel Fernández Marqués
// Licensed under the GNU General Public License (GPL) version 3. See the LICENSE file for more details.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToolButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public ToolType type;
    public bool toggle;
    //public UnityEvent eventActivate;
    //public UnityEvent eventDeactivate;
    public KeyCode shortcutKey;
    public GameObject menu;

    [HideInInspector] public Button button;
    [HideInInspector] public Text text;
    [SerializeField] private ToolButton[] incompatibleTools;

    private bool toggleActive;

    protected virtual void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(Execute);

        text = GetComponentInChildren<Text>();
        text.gameObject.SetActive(false);
    }

    void Update()
    {
        if(Input.GetKeyDown(shortcutKey))
        {
            //eventActivate.Invoke();
            Execute();
        }
    }

    public virtual void Deactivate()
    {
        button.interactable = true;
        if(menu != null) menu.SetActive(false);
    }

    public void Execute()
    {
        if(!toggle)
        {
            button.interactable = false;
            GameController.instance.SetTool(type);
            if(menu != null) menu.SetActive(true);
        }
        else
        {
            toggleActive = !toggleActive;
            GameController.instance.SetTool(type, toggleActive);
            if(menu != null) menu.SetActive(!menu.activeSelf);
        }

        foreach(ToolButton incompatibleTool in incompatibleTools)
        {
            //eventDeactivate.Invoke();
            incompatibleTool.Deactivate();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        text.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        text.gameObject.SetActive(false);
    }
}
