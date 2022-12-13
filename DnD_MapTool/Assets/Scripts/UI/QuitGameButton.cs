// Copyright (c) Daniel Fernández 2022


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuitGameButton : MonoBehaviour
{
    private void OnEnable()
    {
        GetComponent<Button>().onClick.AddListener(QuitGame);
    }

    private void OnDisable()
    {
        GetComponent<Button>().onClick.RemoveListener(QuitGame);
    }

    public void QuitGame()
    {
        GameManager.instance.QuitGame();
    }
}
