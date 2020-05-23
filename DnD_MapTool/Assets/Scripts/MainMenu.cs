using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.Net.NetworkInformation;

public class MainMenu : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject namePanel;
    [SerializeField] private InputField nameField;
    [SerializeField] private Button continueButton;

    private const string playerPrefsNameKey = "Player Name";

    [SerializeField] private GameObject lobbyPanel;
    [SerializeField] private GameObject waitingStatusPanel;
    [SerializeField] private Text waitingStatusText;

    private bool isConnecting = false;

    #region Player Logging

    private void Start()
    {
        //SetUpInputField();
        if(PlayerPrefs.HasKey(playerPrefsNameKey))
        {
            string defaultName = PlayerPrefs.GetString(playerPrefsNameKey);
            nameField.text = defaultName;
        }
    }

    public void CheckNameValid(string name)
    {
        continueButton.interactable = !string.IsNullOrEmpty(name);
    }

    public void SavePlayerName()
    {
        string playerName = nameField.text;
        NetworkManager.instance.SetPlayerName(playerName);
        PlayerPrefs.SetString(playerPrefsNameKey, playerName);
        Debug.Log("Saved player name:" + playerName);
    }

    #endregion

    #region Room Setup

    //public void FindRooms()
    //{
    //    isConnecting = true;
    //    lobbyPanel.SetActive(true);
    //    waitingStatusPanel.SetActive(true);
    //    waitingStatusText.text = "Searching for rooms...";

    //    if(PhotonNetwork.IsConnected)
    //    {
    //        PhotonNetwork.JoinRandomRoom();
    //    }
    //    else
    //    {
    //        PhotonNetwork.GameVersion = Application.version;
    //        PhotonNetwork.ConnectUsingSettings();
    //    }
    //}

    //public override void OnConnectedToMaster()
    //{
    //    //base.OnConnectedToMaster();

    //    Debug.Log("Connected to Master");

    //    if(isConnecting)
    //    {
    //        PhotonNetwork.JoinRandomRoom();
    //    }
    //}

    //public override void OnDisconnected(DisconnectCause cause)
    //{
    //    //base.OnDisconnected(cause);

    //    waitingStatusPanel.SetActive(false);
    //    lobbyPanel.SetActive(true);

    //    Debug.LogError($"Disconnected due to: {cause}");
    //}

    //public override void OnJoinRandomFailed(short returnCode, string message)
    //{
    //    //base.OnJoinRandomFailed(returnCode, message);

    //    Debug.Log("No clients are waiting, creating new room");

    //    PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = (byte)NetworkManager.instance.MaxPlayersPerRoom });
    //}

    //public override void OnJoinedRoom()
    //{
    //    //base.OnJoinedRoom();

    //    Debug.Log("Client successfully joined a room");

    //    int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
    //    if(playerCount != NetworkManager.instance.MaxPlayersPerRoom)
    //    {
    //        waitingStatusText.text = "Waiting for more players";
    //        Debug.Log("Client waiting for more players");
    //    }
    //    else
    //    {
    //        waitingStatusText.text = "All players are ready";
    //        Debug.Log("Room is ready");
    //    }
    //}

    //public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    //{
    //    //base.OnPlayerEnteredRoom(newPlayer);

    //    if(PhotonNetwork.CurrentRoom.PlayerCount == NetworkManager.instance.MaxPlayersPerRoom)
    //    {
    //        PhotonNetwork.CurrentRoom.IsOpen = false;
    //        Debug.Log("Room is full");

    //        waitingStatusText.text = "Player found";

    //        PhotonNetwork.LoadLevel("Scene_main");
    //    }
    //}

    #endregion

    public void SetStatus(string status)
    {
        waitingStatusText.text = status;
    }

    public void ClearRooms()
    {

    }

    public void AddRoom(string name, int playerCount, byte maxPlayers, bool isOpen)
    {

    }
}
