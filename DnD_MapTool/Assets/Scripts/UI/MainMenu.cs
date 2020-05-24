using System;
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
    [SerializeField] private InputField roomNameField;
    [SerializeField] private Button createRoomButton;
    [SerializeField] private Toggle isDMCreate;

    private List<GameObject> rooms;

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

        rooms = new List<GameObject>();
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

    public void AddRoom(string name, int playerCount, byte maxPlayers, bool isOpen)
    {
        RoomPreview room = Instantiate(roomButtonPrefab, roomsContainer).GetComponent<RoomPreview>();
        room.SetRoom(name, playerCount, maxPlayers, isOpen);
        rooms.Add(room.gameObject);
    }

    public void CheckRoomNameValid(string name)
    {
        createRoomButton.interactable = !string.IsNullOrEmpty(name);
    }

    public void CreateRoom()
    {
        NetworkManager.instance.CreateRoom(roomNameField.text, isDMCreate.isOn);
    }

    public void ActivateJoinRoom()
    {

    }

    #endregion
}
