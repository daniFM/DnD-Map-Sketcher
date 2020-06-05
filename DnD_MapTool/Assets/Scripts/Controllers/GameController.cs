using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Random = UnityEngine.Random;

public enum ToolType { selection, brush }

public class GameController : MonoBehaviour
{
    //public Player player;
    [SerializeField] private ToolType tool;
    public ToolType Tool { get { return tool; } }

    [ColorUsage(false, false)] public List<Color> playerColors;
    [ColorUsage(false, true)] public Color highlightColor;

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
            Token newToken = PhotonNetwork.Instantiate(tokenNPCPrefab.name, Vector3.zero, Quaternion.identity).GetComponent<Token>(); // NO DEBERÍA DE HACERSE ASÍ NORMALMENTE
            newToken.Init(GetPlayerColor(PhotonNetwork.LocalPlayer.ActorNumber - 1));
            SetTool(ToolType.brush);
        }
        else
        {
            //player = Instantiate(playerPrefab, this.transform).GetComponent<Player>();
            Token newToken = PhotonNetwork.Instantiate(tokenPlayerPrefab.name, Vector3.zero, Quaternion.identity).GetComponent<Token>();
            newToken.Init(GetPlayerColor(PhotonNetwork.LocalPlayer.ActorNumber - 1));
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

    public Color GetPlayerColor(int index)
    {
        while(index > playerColors.Count - 1)
            playerColors.Add(new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)));

        return playerColors[index];
    }
}
