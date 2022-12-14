// Copyright (c) 2022 Daniel Fernández Marqués
// Licensed under the GNU General Public License (GPL) version 3. See the LICENSE file for more details.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private Text roomName;
    [SerializeField] private PopupYesNo popup;

    void Start()
    {
        roomName.text = NetworkManager.instance.GetCurrentRoomName();
    }

    public void ExitRoom()
    {
        //NetworkManager.instance.ExitRoom();
        popup.PopupQuestion("Are you sure you want to exit? Unsaved maps will be lost", NetworkManager.instance.ExitRoom, null);
    }
}
