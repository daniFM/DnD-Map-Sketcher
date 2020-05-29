using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Text playerName;
    public Image photo;
    public Image colorIcon;
    [HideInInspector] public Color color;

    public void Init(string name, bool isDM, Color color)
    {
        playerName.text = "";
        if(isDM)
            playerName.text += "(DM) ";
        playerName.text += name;

        if(photo.sprite.name == "T_PlayerIcon")
            photo.color = color;

        colorIcon.color = color;

        this.color = color;
    }
}
