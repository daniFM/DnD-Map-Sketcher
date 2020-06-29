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

    public TokenController tokenController;

    [SerializeField] private ControlsScriptableObject editorControls;
    [SerializeField] private ControlsScriptableObject webControls;
    [HideInInspector] public ControlsScriptableObject controls;

    [SerializeField] private GameMenuController gameMenuController;
    [HideInInspector] public OutlineController outlineController;

    //public NetworkInfo netInfo;
    public static Action OnToolChanged;
    //public static Action OnTilesUpdated;
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
        outlineController = GetComponent<OutlineController>();

        if(GameManager.instance.isDM)
        {
            //Token newToken = PhotonNetwork.Instantiate(tokenNPCPrefab.name, Vector3.zero, Quaternion.identity).GetComponent<Token>(); // NO DEBERÍA DE HACERSE ASÍ NORMALMENTE
            //newToken.Init(GetPlayerColor(PhotonNetwork.LocalPlayer.ActorNumber - 1));
            SetTool(ToolType.brush);
        }
        else
        {
            //Token newToken = PhotonNetwork.Instantiate(tokenPlayerPrefab.name, Vector3.zero, Quaternion.identity).GetComponent<Token>();
            //newToken.Init(GetPlayerColor(PhotonNetwork.LocalPlayer.ActorNumber - 1));
            SetTool(ToolType.selection);
        }

        tokenController.CreateToken();

        if(Application.platform == RuntimePlatform.WindowsEditor)
        {
            controls = editorControls;
        }
        else if(Application.platform == RuntimePlatform.WebGLPlayer)
        {
            controls = webControls;
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

    public Color GetPlayerColor()
    {
        return GetPlayerColor(PhotonNetwork.LocalPlayer.ActorNumber);
    }

    public Color GetPlayerColor(int index)
    {
        index--;
        while(index > playerColors.Count - 1)
            playerColors.Add(new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)));

        return playerColors[index];
    }

    public void Tooltip(string message, float time = 5)
    {
        gameMenuController.ShowTooltip(message, time);
    }
}
