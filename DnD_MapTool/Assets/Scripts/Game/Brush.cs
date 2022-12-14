// Copyright (c) 2022 Daniel Fernández Marqués

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brush : MonoBehaviour
{
    private void OnEnable()
    {
        if(GameController.instance?.Tool == ToolType.selection)
            gameObject.SetActive(false);
    }
}
