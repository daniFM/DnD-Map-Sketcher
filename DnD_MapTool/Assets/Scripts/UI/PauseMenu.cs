using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private Text roomName;

    void Start()
    {
        roomName.text = NetworkManager.instance.GetCurrentRoomName();
    }

    public void ExitRoom()
    {
        NetworkManager.instance.ExitRoom();
    }
}
