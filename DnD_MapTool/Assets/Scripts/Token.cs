using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Token : MonoBehaviour
{
    public Player controlledBy;
    public bool selected;
    public LayerMask tokenLayer;
    public LayerMask tileLayer;

    private bool active;

    void Start()
    {
        
    }

    void OnEnable()
    {
        GameController.OnToolChanged += ToolChanged;
        ToolChanged();
    }

    void OnDisable()
    {
        GameController.OnToolChanged += ToolChanged;
    }

    void Update()
    {
        if(active)
        {
            if(Input.GetMouseButtonDown(0) && controlledBy == GameController.instance.player)
            {
                LayerMask finalLayer = tokenLayer;
                Debug.Log("Trying to click token " + gameObject.name);
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if(Physics.Raycast(ray, out hit, Mathf.Infinity, finalLayer))
                {
                    selected = true;
                    Debug.Log("Clicked token " + gameObject.name);
                }
                else
                {
                    selected = false;
                }
            }
        }
    }

    private void ToolChanged()
    {
        if(GameController.instance.tool == ToolType.selection)
        {
            active = true;
        }
        else
        {
            active = false;
        }
    }
}
