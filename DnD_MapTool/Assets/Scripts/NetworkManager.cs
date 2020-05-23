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
        mainMenu.SetStatus("Searching for rooms...");

        if(PhotonNetwork.IsConnected) // In case we were connected from earlier
        {
            //PhotonNetwork.JoinRandomRoom();
            PhotonNetwork.JoinLobby();
        }
        else
        {
            PhotonNetwork.GameVersion = Application.version;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public void ConnectToRoom(string name)
    {
        mainMenu.SetStatus("Joining " + name);
        PhotonNetwork.JoinRoom(name);
    }

    public void CreateRoom(string name)
    {
        Debug.Log("Creating new room");
        PhotonNetwork.CreateRoom(name, new RoomOptions
            {
                MaxPlayers = (byte)NetworkManager.instance.MaxPlayersPerRoom
            });
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");

        if(isConnecting)
        {
            //PhotonNetwork.JoinRandomRoom();
            PhotonNetwork.JoinLobby();
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        //base.OnDisconnected(cause);

        //waitingStatusPanel.SetActive(false);
        //lobbyPanel.SetActive(true);

        Debug.LogError($"Disconnected due to: {cause}");
    }

    // Called on entering the Lobby
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("Successfully connected to lobby");
        mainMenu.ClearRooms();
        foreach(RoomInfo room in roomList)
        {
            Debug.Log("Found room: " + room.Name);
            mainMenu.AddRoom(room.Name, room.PlayerCount, room.MaxPlayers, room.IsOpen);
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        //base.OnJoinRandomFailed(returnCode, message);

        Debug.Log("No clients are waiting");

        //PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = (byte)NetworkManager.instance.MaxPlayersPerRoom });
    }

    public override void OnJoinedRoom()
    {
        //base.OnJoinedRoom();

        Debug.Log("Client successfully joined a room");

        //int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
        //if(playerCount != maxPlayersPerRoom)
        //{
        //    //waitingStatusText.text = "Waiting for more players";
        //    mainMenu.SetStatus("Waiting for more players");
        //    Debug.Log("Client waiting for more players");
        //}
        //else
        //{
        //    //waitingStatusText.text = "All players are ready";
        //    mainMenu.SetStatus("All players are ready");
        //    Debug.Log("Room is ready");
        //}

        //TEST
        PhotonNetwork.LeaveRoom(false);
        //Invoke("FindRooms", 3);
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        //base.OnPlayerEnteredRoom(newPlayer);

        if(PhotonNetwork.CurrentRoom.PlayerCount == maxPlayersPerRoom)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            Debug.Log("Room is full");

            //waitingStatusText.text = "Player found";
            mainMenu.SetStatus("Player found");

            PhotonNetwork.LoadLevel("Scene_main");
        }
    }
}
