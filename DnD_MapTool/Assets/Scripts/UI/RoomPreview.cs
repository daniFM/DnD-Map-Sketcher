// Copyright (c) 2022 Daniel Fernández Marqués

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomPreview : MonoBehaviour
{
    [SerializeField] private Text nameText;
    [SerializeField] private InputField passwordText;
    [SerializeField] private Image image;
    [SerializeField] private Text playersText;
    [SerializeField] private Toggle openToggle;
    [SerializeField] private RectTransform joinPanel;
    [SerializeField] private Toggle isDMToggle;

    private string password;
    private Vector2 localPos;

    public void SetRoom(string name, string password, int currentPlayers, int maxPlayers, bool isOpen)
    {
        nameText.text = name;
        playersText.text = currentPlayers + "/" + maxPlayers;
        openToggle.isOn = isOpen;

        this.password = password;
        if(string.IsNullOrEmpty(password))
        {
            //passwordText.gameObject.SetActive(false);
            passwordText.interactable = false;
        }
        localPos = joinPanel.anchoredPosition;
    }

    public void ToggleJoinPanel(bool activate)
    {
        joinPanel.gameObject.SetActive(activate);
        //joinPanel.position = joinPanel.parent.parent.position;// + joinPanel.localPosition;
        if(activate)
        {
            joinPanel.parent = this.transform.parent.parent.parent.parent;
            joinPanel.anchoredPosition = localPos;
        }
        else
        {
            joinPanel.parent = this.transform;
        }
    }

    public void JoinRoom()
    {
        if(password == passwordText.text)
        {
            NetworkManager.instance.JoinRoom(nameText.text, passwordText.text, isDMToggle.isOn);
        }
        else
        {
            Debug.Log("Password is not correct");
            NetworkManager.OnStatusChanged?.Invoke("Password is not correct");
        }
        //menu.ActivateJoinRoom();
    }
}
