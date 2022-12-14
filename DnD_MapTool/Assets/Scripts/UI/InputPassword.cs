// Copyright (c) 2022 Daniel Fernández Marqués
// Licensed under the GNU General Public License (GPL) version 3. See the LICENSE file for more details.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputPassword : MonoBehaviour
{
    private InputField field;

    void Start()
    {
        field = GetComponent<InputField>();
    }

    public void ActivateView(bool active)
    {
        if(active)
        {
            field.contentType = InputField.ContentType.Standard;
        }    
        else
        {
            field.contentType = InputField.ContentType.Password;
        }

        field.ForceLabelUpdate();
    }
}
