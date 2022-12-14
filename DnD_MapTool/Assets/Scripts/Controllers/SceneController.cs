// Copyright (c) 2022 Daniel Fernández Marqués
// Licensed under the GNU General Public License (GPL) version 3. See the LICENSE file for more details.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Scenes
{
    MainMenu,
    AutoTiles_mode
}

public class SceneController : MonoBehaviour
{
    public Scenes startScene;
    public bool useStartScene;

    public static SceneController instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(this);
            return;
        }

        if(useStartScene)
        {
            //Destroy(this);
            //Destroy(gameObject);
            Debug.Log("Loading start scene");
            SceneManager.LoadScene(startScene.ToString());
        }
    }
}
