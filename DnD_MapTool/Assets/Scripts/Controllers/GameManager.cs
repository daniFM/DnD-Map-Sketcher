using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager: MonoBehaviour
{
    //public string gameVersion = "0.0";
    public bool isDM = false;

    public static GameManager instance = null;

    void Awake()
    {
        if(instance == null)
            instance = this;
        else if(instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
