using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomPreview : MonoBehaviour
{
    [SerializeField] private Text nameText;
    [SerializeField] private Image image;
    [SerializeField] private Text playersText;
    [SerializeField] private Toggle openToggle;
    [SerializeField] private RectTransform joinPanel;
    [SerializeField] private Toggle isDMToggle;

    private Vector2 localPos;

    public void SetRoom(string name, int currentPlayers, int maxPlayers, bool isOpen)
    {
        nameText.text = name;
        playersText.text = currentPlayers + "/" + maxPlayers;
        openToggle.isOn = isOpen;

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
        NetworkManager.instance.JoinRoom(nameText.text, isDMToggle.isOn);
        //menu.ActivateJoinRoom();
    }
}
