using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public enum ToolType { selection, brush }

public class GameController : MonoBehaviour
{
    //public Player player;
    [SerializeField] private ToolType tool;
    public ToolType Tool { get { return tool; } }

    //[SerializeField] private GameObject playerPrefab;
    //[SerializeField] private GameObject masterPrefab;
    [SerializeField] private GameObject tokenPlayerPrefab;
    [SerializeField] private GameObject tokenNPCPrefab;

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
            //player = Instantiate(MasterPrefab, this.transform).GetComponent<Player>();
            PhotonNetwork.Instantiate(tokenNPCPrefab.name, Vector3.zero, Quaternion.identity); // NO DEBERÍA DE HACERSE ASÍ NORMALMENTE
            SetTool(ToolType.brush);
        }
        else
        {
            //player = Instantiate(playerPrefab, this.transform).GetComponent<Player>();
            PhotonNetwork.Instantiate(tokenPlayerPrefab.name, Vector3.zero, Quaternion.identity);
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
