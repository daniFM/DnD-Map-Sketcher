using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomPreview : MonoBehaviour
{
    [SerializeField] private Text nameText;
    [SerializeField] private Image image;
    [SerializeField] private Text playersText;
    [SerializeField] private Toggle toggle;

    //private MainMenu menu;

    public void SetRoom(string name, int currentPlayers, int maxPlayers, bool isOpen)
    {
        nameText.text = name;
        playersText.text = currentPlayers + "/" + maxPlayers;
        toggle.isOn = isOpen;
    }
}
