// Copyright (c) 2022 Daniel Fernández Marqués
// Licensed under the GNU General Public License (GPL) version 3. See the LICENSE file for more details.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager: MonoBehaviour
{
    //public string gameVersion = "0.0";
    public bool isDM = false;
    public string playerName;

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
        #if UNITY_STANDALONE_WIN && !UNITY_EDITOR
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        #else
            Application.Quit();
        #endif
    }
}
