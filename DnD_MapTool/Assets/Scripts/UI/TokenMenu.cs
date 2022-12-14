// Copyright (c) 2022 Daniel Fernández Marqués
// Licensed under the GNU General Public License (GPL) version 3. See the LICENSE file for more details.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TokenMenu : MonoBehaviour
{
    public TokenController tokenController;
    public Slider sliderSize;

    private Color color;

    private void Start()
    {
        color = GameController.instance.GetPlayerColor();
    }

    public void CreateToken()
    {
        tokenController.CreateToken(color, (int)sliderSize.value);
        GameController.instance.Tooltip("A token has been created.");
    }

    public void OnColorChanged(Color color)
    {
        this.color = color;
    }
}
