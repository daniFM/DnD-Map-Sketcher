// Copyright (c) Daniel Fernández 2022


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
