using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ToolType { selection, brush }

public class GameController : MonoBehaviour
{
    public Player player;
    public ToolType tool;

    public static Action OnToolChanged;
    public static GameController instance;

    void Awake()
    {
        if(instance == null)
            instance = this;
        else if(instance != this)
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetTool(ToolType tool)
    {
        this.tool = tool;
    }

    public void SwitchTool()
    {
        switch(tool)
        {
            case ToolType.selection:
                {
                    tool = ToolType.brush;
                    break;
                }
            case ToolType.brush:
                {
                    tool = ToolType.selection;
                    break;
                }
        }
        //tool.Next();
        OnToolChanged?.Invoke();
    }
}
