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
    public string playerColor { get; private set; }

    private readonly string[] rollCommands = { "roll ", "roll" };

    private const string errorRollMsg = "Error parsing dice command";

    private void Start()
    {
        input = GetComponentInChildren<InputField>();
        sb = new System.Text.StringBuilder();
        playerColor = UnityEngine.ColorUtility.ToHtmlStringRGB(GameController.instance.GetPlayerColor());
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            lastMessage = input.text;
            SendChatMessage(input.text, false, true, playerColor);
            string msg = input.text;
            photonView.RPC("SendChatMessage", RpcTarget.Others, msg, false, false, playerColor);
        }

        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            input.text = lastMessage;
        }
    }

    [PunRPC]
    public void SendChatMessage(string message, bool systemMessage, bool checkForCommands, string senderColor)
    {
        input.text = string.Empty;
        input.Select();
        input.ActivateInputField();

        Text messageText = Instantiate(chatMessageTextPrefab, chatMessagesContainer).GetComponent<Text>();

        //messageText.text = "<color=" + GameController.instance.GetPlayerColor() + ">" + GameManager.instance.playerName + "</color>: " + message;

        //messageText.text = string.Format("<color={0}>{1}</color>: {2}", GameController.instance.GetPlayerColor(), GameManager.instance.playerName, message);

        sb.Length = 0;

        if(systemMessage)
        {
            sb.Append("System: ");
        }
        else
        {
            sb.Append("<color=#")
                .Append(senderColor)
                .Append(">")
                .Append(GameManager.instance.playerName)
                .Append("</color>: ");
        }
        sb.Append(message);

        messageText.text = sb.ToString();

        if(checkForCommands)
            CheckCommand(message);
    }

    public void SendChatMessageToAll(string message, bool systemMessage, bool checkForCommands, string senderColor)
    {
        photonView.RPC("SendChatMessage", RpcTarget.All, message, systemMessage, checkForCommands, senderColor);
    }

    public void SendChatMessageToOthers(string message, bool systemMessage, bool checkForCommands, string senderColor)
    {
        photonView.RPC("SendChatMessage", RpcTarget.Others, message, systemMessage, checkForCommands, senderColor);
    }

    private void CheckCommand(string message)
    {
        foreach(string command in rollCommands)
        {
            if(message.Contains(command))
            {
                //try
                //{
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
                //}
                //catch(Exception e)
                //{
                //    SendChatMessage(errorRollMsg);
                //    Debug.LogError(errorRollMsg + "\n" + e.Message + "\n\n" + message);
                //}

                break;
            }
        }
    }

    public void RollWithButton()
    {
        sb.Length = 0;
        sb.Append("roll ").Append(diceNumber.options[diceNumber.value].text).Append("d").Append(diceType.options[diceType.value].text);
        SendChatMessage(sb.ToString(), false, true, playerColor);
        photonView.RPC("SendChatMessage", RpcTarget.Others, sb.ToString(), false, false, playerColor);
    }
}
