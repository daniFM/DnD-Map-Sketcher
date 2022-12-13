// Copyright (c) Daniel Fernández 2022


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

    private int index;

    public void Init(string name, int index, bool isDM, Color color)
    {
        playerName.text = "";
        this.index = index;

        if(isDM)
            playerName.text += "(DM) ";
        playerName.text += name;

        if(photo.sprite.name == "T_PlayerIcon")
            photo.color = color;

        colorIcon.color = color;

        this.color = color;
    }

    public void CreateToken()
    {
        if(GameController.instance.Tool == ToolType.selection)
            GameController.instance.tokenController.CreateToken(index);
    }
}
