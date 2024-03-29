// Copyright (c) 2022 Daniel Fernández Marqués
// Licensed under the GNU General Public License (GPL) version 3. See the LICENSE file for more details.

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
    public Dictionary<int, string> players { get; private set; }
    [SerializeField] private bool isConnecting = false;
    public bool IsConnecting { get { return isConnecting; } }

    public string region { get; private set; }
    public List<string> regions { get; private set; }

    public const string pwKey = "p";

    private List<RoomInfo> lobby;
    private List<RoomInfo> lobbySearch;
    private bool searching;

    public static Action<string> OnStatusChanged;
    public static Action<string> OnConnectedToServer;
    public static Action<List<string>> OnRegionsUpdated;
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
        GameManager.instance.playerName = name;
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

    public void JoinRoom(string name, string password, bool isDM)
    {
        //if(lobby.Find(i => i.Name == name).CustomProperties["p"].ToString() == password)
        //{
            OnStatusChanged?.Invoke("Joining " + name);
            PhotonNetwork.JoinRoom(name);
            GameManager.instance.isDM = isDM;
        //}
        //else
        //{
        //    Debug.Log("Password is not correct");
        //    OnStatusChanged?.Invoke("Password is not correct");
        //}
    }

    public void CreateRoom(string name, string password, bool isDM)
    {
        Debug.Log("Creating new room");

        RoomOptions options = new RoomOptions
        {
            MaxPlayers = (byte)MaxPlayersPerRoom,
            CleanupCacheOnLeave = false,
            CustomRoomPropertiesForLobby = new string[] { pwKey },
            CustomRoomProperties = new ExitGames.Client.Photon.Hashtable()
        };
        options.CustomRoomProperties.Add(pwKey, password);

        PhotonNetwork.CreateRoom(name, options);

        GameManager.instance.isDM = isDM;
    }

    public void ExitRoom()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel("MainMenu");
    }

    public void FilterLobby(string search)
    {
        if(search != string.Empty)
        {
            searching = true;
            lobbySearch.Clear();
            foreach(RoomInfo room in lobby)
            {
                // Like "if(room.Name.Contains(search))" but case insensitive
                if(room.Name?.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0)
                    lobbySearch.Add(room);
            }
            OnRoomsUpdated?.Invoke(new List<RoomInfo>(lobbySearch));
        }
        else
        {
            searching = false;
            // Update to all rooms when finished searching
            OnRoomsUpdated?.Invoke(new List<RoomInfo>(lobby));
        }
    }

    public void ChangeRegion(string region)
    {
        OnStatusChanged?.Invoke("Changing region...");
        PhotonNetwork.Disconnect();
        PhotonNetwork.ConnectToRegion(region);
    }

    #region PUN CALLBACKS

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");

        lobby = new List<RoomInfo>();
        lobbySearch = new List<RoomInfo>();

        region = PhotonNetwork.CloudRegion;
        OnConnectedToServer?.Invoke(region);

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

    public override void OnRegionListReceived(RegionHandler regionHandler)
    {
        List<string> regions = new List<string>();

        foreach(Region region in regionHandler.EnabledRegions)
        {
            regions.Add(region.Code);
        }

        this.regions = regions;
        OnRegionsUpdated?.Invoke(this.regions);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("Successfully connected to lobby");
        OnStatusChanged?.Invoke("Found " + roomList.Count + " new or updated rooms. Searching for more...");

        // Update room cache, knowing room names are unique
        foreach(RoomInfo uproom in roomList)
        {
            RoomInfo room = lobby.Find(x => x.Name == uproom.Name);
            // a room has been updated
            if(room != null)
            {
                // room has been removed
                if(uproom.RemovedFromList)
                {
                    lobby.Remove(room);
                }
                // room has been updated (a player joined)
                else
                {   // didn't find a better way to do this
                    lobby.Remove(room);
                    lobby.Add(uproom);
                }
            }
            // a new room has been added
            else
            {
                lobby.Add(uproom);
            }
        }

        if(!searching)
            OnRoomsUpdated?.Invoke(new List<RoomInfo>(lobby));
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Room creation failed: " + message);
        OnStatusChanged?.Invoke("Failed to create room. A room with the same name already exists.");
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

        players = new Dictionary<int, string>();
        players[PhotonNetwork.LocalPlayer.ActorNumber] = PhotonNetwork.LocalPlayer.NickName;
        foreach(Photon.Realtime.Player otherPlayer in PhotonNetwork.PlayerListOthers)
        {
            players[otherPlayer.ActorNumber] = otherPlayer.NickName;
        }

        PhotonNetwork.LoadLevel("AutoTiles_mode");
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        if(PhotonNetwork.CurrentRoom.PlayerCount == maxPlayersPerRoom)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            Debug.Log("Room is full, closing room");
        }

        players[newPlayer.ActorNumber] = newPlayer.NickName;
        OnPlayerJoined?.Invoke(newPlayer.ActorNumber, newPlayer.NickName);
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        if(PhotonNetwork.CurrentRoom.PlayerCount == maxPlayersPerRoom - 1)
        {
            PhotonNetwork.CurrentRoom.IsOpen = true;
            Debug.Log("Room now open for new players");
        }

        players.Remove(otherPlayer.ActorNumber);
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
