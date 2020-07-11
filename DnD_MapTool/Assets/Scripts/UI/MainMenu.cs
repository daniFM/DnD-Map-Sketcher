﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject namePanel;
    [SerializeField] private InputField nameField;
    [SerializeField] private Button continueButton;

    private const string playerPrefsNameKey = "Player Name";

    [SerializeField] private GameObject lobbyPanel;
    [SerializeField] private RectTransform roomsContainer;
    [SerializeField] private GameObject roomButtonPrefab;
    //[SerializeField] private GameObject waitingStatusPanel;
    [SerializeField] private Text waitingStatusText;
    [SerializeField] private Text regionText;
    [SerializeField] private InputField roomNameField;
    [SerializeField] private InputField roomPasswordField;
    [SerializeField] private Button createRoomButton;
    [SerializeField] private Toggle isDMCreate;

    private List<RoomPreview> rooms;

    private void OnEnable()
    {
        NetworkManager.OnStatusChanged += SetStatus;
        NetworkManager.OnConnectedToServer += SetRegion;
        NetworkManager.OnRoomsUpdated += UpdateRooms;
    }

    private void OnDisable()
    {
        NetworkManager.OnStatusChanged -= SetStatus;
        NetworkManager.OnConnectedToServer -= SetRegion;
        NetworkManager.OnRoomsUpdated -= UpdateRooms;
    }

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

    #region Lobby Setup

    public void StartLobby()
    {
        namePanel.SetActive(false);
        lobbyPanel.SetActive(true);

        rooms = new List<RoomPreview>();
        NetworkManager.instance.FindRooms();
    }

    public void SetStatus(string status)
    {
        waitingStatusText.text = status;
    }

    public void ClearRooms()
    {
        //foreach(GameObject room in rooms)
        //{
        //    Destroy(room);
        //}
        for(int i = 0; i < roomsContainer.childCount; ++i)
        {
            Destroy(roomsContainer.GetChild(i).gameObject);
        }
        rooms.Clear();
    }

    public void AddRoom(string name, string password, int playerCount, byte maxPlayers, bool isOpen)
    {
        RoomPreview room = Instantiate(roomButtonPrefab, roomsContainer).GetComponent<RoomPreview>();
        room.SetRoom(name, password, playerCount, maxPlayers, isOpen);
        rooms.Add(room);
    }

    //public void RemoveRoom(int id)
    //{
    //    for(int i = 0; i < rooms.Count; ++i)
    //    {
    //        if(rooms[i].id == id)
    //        {
    //            Destroy(rooms[i].gameObject);
    //            rooms.RemoveAt(i);
    //        }
    //    }
    //}

    public void UpdateRooms(List<Photon.Realtime.RoomInfo> roomList)
    {
        Debug.Log("Number of rooms: " + roomList.Count);
        ClearRooms();
        roomList.Reverse();
        foreach(Photon.Realtime.RoomInfo room in roomList)
        {
            Debug.Log("Found room: " + room.Name);
            AddRoom(room.Name, room.CustomProperties[NetworkManager.pwKey]?.ToString(), room.PlayerCount, room.MaxPlayers, room.IsOpen);
        }
    }

    public void SetRegion(string region)
    {
        regionText.text += region.ToUpper();
    }

    public void CheckRoomNameValid(string name)
    {
        createRoomButton.interactable = !string.IsNullOrEmpty(name);
    }

    public void CreateRoom()
    {
        NetworkManager.instance.CreateRoom(roomNameField.text, roomPasswordField.text, isDMCreate.isOn);
    }

    public void ActivateJoinRoom()
    {

    }

    #endregion
}
