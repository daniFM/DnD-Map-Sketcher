using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager: MonoBehaviourPunCallbacks, IPunOwnershipCallbacks
{
    [SerializeField] private int maxPlayersPerRoom;
    public int MaxPlayersPerRoom { get { return maxPlayersPerRoom; } }
    [SerializeField] private bool isConnecting = false;
    public bool IsConnecting { get { return isConnecting; } }

    public static Action<string> OnStatusChanged;
    public static Action<List<RoomInfo>> OnRoomsUpdated;
    public static Action<int, string> OnPlayerJoined;
    public static Action<int, string> OnPlayerLeft;

    public static NetworkManager instance = null;

    void Awake()
    {
        if(instance == null)
            instance = this;
        else if(instance != this)
            Destroy(gameObject);

        // So if the master changes scene, all players do as well;
        //PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void SetPlayerName(string name)
    {
        PhotonNetwork.NickName = name;
    }

    public void FindRooms()
    {
        isConnecting = true;
        OnStatusChanged?.Invoke("Searching for rooms...");

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

    public void JoinRoom(string name, bool isDM)
    {
        OnStatusChanged?.Invoke("Joining " + name);
        PhotonNetwork.JoinRoom(name);
        GameManager.instance.isDM = isDM;
    }

    public void CreateRoom(string name, bool isDM)
    {
        Debug.Log("Creating new room");
        PhotonNetwork.CreateRoom(name, new RoomOptions
            {
                MaxPlayers = (byte)NetworkManager.instance.MaxPlayersPerRoom,
                CleanupCacheOnLeave = false
            });

        GameManager.instance.isDM = isDM;
    }

    public void ExitRoom()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel("MainMenu");
    }

    #region PUN CALLBACKS

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

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("Successfully connected to lobby");
        OnStatusChanged?.Invoke("Found " + roomList.Count + " rooms. Searching for more...");
        OnRoomsUpdated?.Invoke(roomList);
        //mainMenu.ClearRooms();
        //foreach(RoomInfo room in roomList)
        //{
        //    Debug.Log("Found room: " + room.Name);
        //    mainMenu.AddRoom(room.Name, room.PlayerCount, room.MaxPlayers, room.IsOpen);
        //}
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        GameManager.instance.isDM = false;
        Debug.LogError("Room connection failed: " + message);
        OnStatusChanged?.Invoke("Failed to join room");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("No clients are waiting");

        //PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = (byte)NetworkManager.instance.MaxPlayersPerRoom });
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Client successfully joined a room");
        //OnStatusChanged?.Invoke("Loading room");

        isConnecting = false;

        //bool setmaster = false;
        //if(GameManager.instance.isDM)
        //    setmaster = PhotonNetwork.SetMasterClient(PhotonNetwork.LocalPlayer);
        //if(!setmaster)
        //    Debug.Log("Failed to set master");

        PhotonNetwork.LoadLevel("AutoTiles_mode");
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        if(PhotonNetwork.CurrentRoom.PlayerCount == maxPlayersPerRoom)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            Debug.Log("Room is full, closing room");
        }

        OnPlayerJoined?.Invoke(newPlayer.ActorNumber, newPlayer.NickName);
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        if(PhotonNetwork.CurrentRoom.PlayerCount == maxPlayersPerRoom - 1)
        {
            PhotonNetwork.CurrentRoom.IsOpen = true;
            Debug.Log("Room now open for new players");
        }

        OnPlayerLeft?.Invoke(otherPlayer.ActorNumber, otherPlayer.NickName);
    }

    //public override void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
    //{
    //    Debug.Log("Master changed");
    //    if(PhotonNetwork.LocalPlayer.IsMasterClient)
    //        Debug.Log("I'm the master");
    //}

    public void OnOwnershipRequest(PhotonView targetView, Photon.Realtime.Player requestingPlayer)
    {
        throw new NotImplementedException();
    }

    public void OnOwnershipTransfered(PhotonView targetView, Photon.Realtime.Player previousOwner)
    {
        //Debug.Log("Transfering " + targetView.name);

        Token token = targetView.GetComponent<Token>();

        if(token != null)
            token.SetPhysics();
    }

    #endregion

    public string GetCurrentRoomName()
    {
        return PhotonNetwork.CurrentRoom.Name;
    }
}
