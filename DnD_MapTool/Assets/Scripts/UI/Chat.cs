// Copyright (c) 2022 Daniel Fernández Marqués
// Licensed under the GNU General Public License (GPL) version 3. See the LICENSE file for more details.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Chat : MonoBehaviourPun
{
    [SerializeField] private Text chatMessageTextPrefab;
    [SerializeField] private Transform chatMessagesContainer;
    [SerializeField] private Dropdown diceNumber;
    [SerializeField] private Dropdown diceType;

    private InputField input;
    private System.Text.StringBuilder sb;
    private string lastMessage;

    private readonly string[] rollCommands = { "roll ", "roll" };

    private const string errorRollMsg = "Error parsing dice command";

    private void Start()
    {
        input = GetComponentInChildren<InputField>();
        sb = new System.Text.StringBuilder();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            lastMessage = input.text;
            //SendChatMessage(input.text, false, true, PhotonNetwork.LocalPlayer.ActorNumber);
            photonView.RPC("SendChatMessage", RpcTarget.All, input.text, false, PhotonNetwork.LocalPlayer.ActorNumber);
        }

        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            input.text = lastMessage;
        }
    }

    [PunRPC]
    public void SendChatMessage(string message, bool systemMessage, int senderID)
    {
        input.text = string.Empty;
        input.Select();
        input.ActivateInputField();

        if(!string.IsNullOrEmpty(message))
        {
            Text messageText = Instantiate(chatMessageTextPrefab, chatMessagesContainer).GetComponent<Text>();

            //messageText.text = "<color=" + GameController.instance.GetPlayerColor() + ">" + GameManager.instance.playerName + "</color>: " + message;

            //messageText.text = string.Format("<color={0}>{1}</color>: {2}", GameController.instance.GetPlayerColor(), GameManager.instance.playerName, message);

            sb.Clear();

            if(systemMessage)
            {
                sb.Append("System: ");
            }
            else
            {
                sb.Append("<color=#")
                    .Append(UnityEngine.ColorUtility.ToHtmlStringRGB(GameController.instance.GetPlayerColor(senderID)))
                    .Append(">")
                    .Append(NetworkManager.instance.players[senderID])
                    .Append("</color>: ");
            }
            sb.Append(message);

            messageText.text = sb.ToString();

            if(PhotonNetwork.LocalPlayer.ActorNumber == senderID)
            {
                CheckCommand(message);
            }
        }
    }

    public void SendChatMessageToAll(string message, bool systemMessage)
    {
        photonView.RPC("SendChatMessage", RpcTarget.All, message, systemMessage, PhotonNetwork.LocalPlayer.ActorNumber);
    }

    //public void SendChatMessageToOthers(string message, bool systemMessage)
    //{
    //    photonView.RPC("SendChatMessage", RpcTarget.Others, message, systemMessage, PhotonNetwork.LocalPlayer.ActorNumber);
    //}

    private void CheckCommand(string message)
    {
        foreach(string command in rollCommands)
        {
            if(message.Contains(command))
            {
                try
                {
                    int a = message.IndexOf(command);
                    int b = a + command.Length;
                    int c = message.IndexOf('d', b);
                    int d = c - b;
                    int n = int.Parse(message.Substring(b, d));
                    int e = message.IndexOf(' ', c);
                    if(e == -1)
                        e = message.Length;
                    e--;
                    int f = e - c;
                    int m = int.Parse(message.Substring(c + 1, f));

                    if(Enum.IsDefined(typeof(DiceType), m))
                    {
                        GameController.instance.diceController.Roll((DiceType)m, n);
                    }
                    else
                    {
                        Debug.Log("Could not parse dice roll");
                    }
                }
                catch(Exception e)
                {
                    SendChatMessage(errorRollMsg, true, PhotonNetwork.LocalPlayer.ActorNumber);
                    Debug.LogError(errorRollMsg + "\n" + e.Message + "\n\n" + message);
                }

                break;
            }
        }
    }

    public void RollWithButton()
    {
        sb.Clear();
        sb.Append("roll ").Append(diceNumber.options[diceNumber.value].text).Append("d").Append(diceType.options[diceType.value].text);
        //SendChatMessage(sb.ToString(), false, PhotonNetwork.LocalPlayer.ActorNumber);
        photonView.RPC("SendChatMessage", RpcTarget.All, sb.ToString(), false, PhotonNetwork.LocalPlayer.ActorNumber);
    }

    public void BlockShortcuts(bool block)
    {
        GameController.instance.controls.keysDisabled = block;
    }
}
