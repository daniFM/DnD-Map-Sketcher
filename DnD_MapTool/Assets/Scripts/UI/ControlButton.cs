// Copyright (c) 2022 Daniel Fernández Marqués
// Licensed under the GNU General Public License (GPL) version 3. See the LICENSE file for more details.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlButton : MonoBehaviour
{
    [HideInInspector] public Button button;
    [HideInInspector] public Text text;

    public ControlAction controlAction;

    void Awake()
    {
        button = GetComponent<Button>();
        text = GetComponentInChildren<Text>();
    }
}
