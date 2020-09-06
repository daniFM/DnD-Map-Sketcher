using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chat : MonoBehaviour
{
    [SerializeField] private Text chatMessageTextPrefab;
    [SerializeField] private Transform chatMessagesContainer;

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
            SendChatMessage(input.text, false);
        }

        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            input.text = lastMessage;
        }
    }

    //public void EndEdit(string message)
    //{
    //    //Debug.Log("Message: " + message);
    //}

    public void SendChatMessage(string message, bool systemMessage = true)
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
                .Append(UnityEngine.ColorUtility.ToHtmlStringRGB(GameController.instance.GetPlayerColor()))
                .Append(">")
                .Append(GameManager.instance.playerName)
                .Append("</color>: ");
        }
        sb.Append(message);

        messageText.text = sb.ToString();

        CheckCommand(message);
    }

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
                    SendChatMessage(errorRollMsg);
                    Debug.LogError(errorRollMsg + "\n" + e.Message);
                }

                break;
            }
        }
    }
}
