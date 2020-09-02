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

    private void Start()
    {
        input = GetComponentInChildren<InputField>();
        sb = new System.Text.StringBuilder();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            SendChatMessage(input.text);
        }
    }

    //public void EndEdit(string message)
    //{
    //    //Debug.Log("Message: " + message);
    //}

    public void SendChatMessage(string message)
    {
        input.text = string.Empty;
        input.Select();
        input.ActivateInputField();

        Text messageText = Instantiate(chatMessageTextPrefab, chatMessagesContainer).GetComponent<Text>();

        //messageText.text = "<color=" + GameController.instance.GetPlayerColor() + ">" + GameManager.instance.playerName + "</color>: " + message;

        //messageText.text = string.Format("<color={0}>{1}</color>: {2}", GameController.instance.GetPlayerColor(), GameManager.instance.playerName, message);

        sb.Length = 0;
        sb.Append("<color=#")
            .Append(UnityEngine.ColorUtility.ToHtmlStringRGB(GameController.instance.GetPlayerColor()))
            .Append(">")
            .Append(GameManager.instance.playerName)
            .Append("</color>: ")
            .Append(message);

        messageText.text = sb.ToString();

        Debug.Log(sb.ToString());
    }
}
