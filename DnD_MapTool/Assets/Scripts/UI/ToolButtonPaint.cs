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
            gameObject.SetActive(false);
        }
        else
        {
            button.interactable = false;    // False because it's selected
        }
    }

    public override void Activate()
    {
        if(GameManager.instance.isDM)
        {
            base.Activate();
        }
    }
}
