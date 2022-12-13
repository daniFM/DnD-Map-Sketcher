// Copyright (c) Daniel Fernández 2022


using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PopupYesNo : MonoBehaviour
{
    [SerializeField] private Text text;
    [SerializeField] private Button buttonYes;
    [SerializeField] private Button buttonNo;

    private bool active;

    void Start()
    {
        SetActive(false);
    }

    public void PopupQuestion(string question, string yesText, string noText, UnityAction callbackYes, UnityAction callbackNo)
    {
        if(!active)
        {
            SetActive(true);

            text.text = question;
            buttonYes.GetComponentInChildren<Text>().text = yesText;
            buttonNo.GetComponentInChildren<Text>().text = noText;

            if(callbackYes != null)
                buttonYes.onClick.AddListener(callbackYes);
            if(callbackNo != null)
                buttonNo.onClick.AddListener(callbackNo);

            buttonYes.onClick.AddListener(OnQuestionAnswered);
            buttonNo.onClick.AddListener(OnQuestionAnswered);
        }
    }

    /// <summary>
    /// Default answers are "Yes" and "No".
    /// Callbacks can be null.
    /// </summary>
    public void PopupQuestion(string question, UnityAction callbackYes, UnityAction callbackNo)
    {
        PopupQuestion(question, "Yes", "No", callbackYes, callbackNo);
    }

    private void OnQuestionAnswered()
    {
        buttonYes.onClick.RemoveAllListeners();
        buttonNo.onClick.RemoveAllListeners();
        SetActive(false);
    }

    private void SetActive(bool active)
    {
        this.active = active;

        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(active);
        }
    }
}
