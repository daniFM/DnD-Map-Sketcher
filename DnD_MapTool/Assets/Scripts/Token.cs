using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Token : MonoBehaviour
{
    public Player controlledBy;
    public bool selected;
    public LayerMask tokenLayer;
    public LayerMask tileLayer;
    public Material highlightedMaterial;

    private bool active;
    private new Renderer renderer;
    private Material mainMaterial;

    void Start()
    {
        renderer = GetComponent<Renderer>();
        mainMaterial = renderer.material;
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

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if(Physics.Raycast(ray, out hit, Mathf.Infinity, finalLayer))
                {
                    selected = true;
                    renderer.material = highlightedMaterial;
                }
                else
                {
                    if(selected)
                        renderer.material = mainMaterial;
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
