using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager: MonoBehaviourPunCallbacks
{
    [SerializeField] private int maxPlayersPerRoom;
    public int MaxPlayersPerRoom { get { return maxPlayersPerRoom; } }
    [SerializeField] private bool isConnecting = false;
    public bool IsConnecting { get { return isConnecting; } }

    private MainMenu mainMenu;

    public static NetworkManager instance = null;

    void Awake()
    {
        if(instance == null)
            instance = this;
        else if(instance != this)
            Destroy(gameObject);

        // So if the master changes scene, all players do as well;
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    void Start()
    {
        mainMenu = FindObjectOfType<MainMenu>();
    }

    public void SetPlayerName(string name)
    {
        PhotonNetwork.NickName = name;
    }

    public void FindRooms()
    {
        isConnecting = true;
        //lobbyPanel.SetActive(true);
        //waitingStatusPanel.SetActive(true);
        //waitingStatusText.text = "Searching for rooms...";
        mainMenu.SetStatus("Searching for rooms...");

        PhotonNetwork.JoinLobby();

        if(PhotonNetwork.IsConnected) // In case we were connected from earlier
        {
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            PhotonNetwork.GameVersion = Application.version;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        //base.OnConnectedToMaster();

        Debug.Log("Connected to Master");

        if(isConnecting)
        {
            PhotonNetwork.JoinRandomRoom();
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        //base.OnDisconnected(cause);

        //waitingStatusPanel.SetActive(false);
        //lobbyPanel.SetActive(true);

        Debug.LogError($"Disconnected due to: {cause}");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        mainMenu.ClearRooms();
        foreach(RoomInfo room in roomList)
        {
            mainMenu.AddRoom(room.Name, room.PlayerCount, room.MaxPlayers, room.IsOpen);
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        //base.OnJoinRandomFailed(returnCode, message);

        Debug.Log("No clients are waiting, creating new room");

        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = (byte)NetworkManager.instance.MaxPlayersPerRoom });
    }

    public override void OnJoinedRoom()
    {
        //base.OnJoinedRoom();

        Debug.Log("Client successfully joined a room");

        int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
        if(playerCount != NetworkManager.instance.MaxPlayersPerRoom)
        {
            //waitingStatusText.text = "Waiting for more players";
            mainMenu.SetStatus("Waiting for more players");
            Debug.Log("Client waiting for more players");
        }
        else
        {
            //waitingStatusText.text = "All players are ready";
            mainMenu.SetStatus("All players are ready");
            Debug.Log("Room is ready");
        }
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        //base.OnPlayerEnteredRoom(newPlayer);

        if(PhotonNetwork.CurrentRoom.PlayerCount == NetworkManager.instance.MaxPlayersPerRoom)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            Debug.Log("Room is full");

            //waitingStatusText.text = "Player found";
            mainMenu.SetStatus("Player found");

            PhotonNetwork.LoadLevel("Scene_main");
        }
    }
}
