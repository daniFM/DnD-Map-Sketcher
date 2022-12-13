// Copyright (c) Daniel Fernández 2022


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolButtonSelection: ToolButton
{
    protected override void Start()
    {
        base.Start();

        if(!GameManager.instance.isDM)
        {
            //button.interactable = false;
            gameObject.SetActive(false);
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
