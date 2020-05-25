using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public void ExitRoom()
    {
        NetworkManager.instance.ExitRoom();
    }
}
