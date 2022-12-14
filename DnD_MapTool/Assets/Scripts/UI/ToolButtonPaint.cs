// Copyright (c) 2022 Daniel Fernández Marqués
// Licensed under the GNU General Public License (GPL) version 3. See the LICENSE file for more details.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolButtonPaint : ToolButton
{
    protected override void Start()
    {
        base.Start();

        if(!GameManager.instance.isDM)
        {
            menu.SetActive(false);
            gameObject.SetActive(false);
        }
        else
        {
            button.interactable = false;    // False because it's selected
        }
    }

    public override void Deactivate()
    {
        if(GameManager.instance.isDM)
        {
            base.Deactivate();
        }
    }
}
