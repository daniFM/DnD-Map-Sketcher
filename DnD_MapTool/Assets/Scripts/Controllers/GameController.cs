using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ToolType { selection, brush }

public class GameController : MonoBehaviour
{
    public Player player;
    [SerializeField] private ToolType tool;
    public ToolType Tool { get { return tool; } }

    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject MasterPrefab;

    [HideInInspector] public NetworkInfo netInfo;
    public static Action OnToolChanged;
    public static Action OnTilesUpdated;
    public static GameController instance;

    void Awake()
    {
        if(instance == null)
            instance = this;
        else if(instance != this)
            Destroy(gameObject);
    }

    void Start()
    {
        netInfo = GetComponent<NetworkInfo>();

        if(GameManager.instance.isDM)
        {
            player = Instantiate(MasterPrefab, this.transform).GetComponent<Player>();
            SetTool(ToolType.brush);
        }
        else
        {
            player = Instantiate(playerPrefab, this.transform).GetComponent<Player>();
            SetTool(ToolType.selection);
        }
    }

    public void SetTool(ToolType tool)
    {
        this.tool = tool;
        OnToolChanged?.Invoke();
    }

    public ToolType SwitchTool()
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
        return tool;
    }
}
