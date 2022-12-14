// Copyright (c) 2022 Daniel Fernández Marqués
// Licensed under the GNU General Public License (GPL) version 3. See the LICENSE file for more details.

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
