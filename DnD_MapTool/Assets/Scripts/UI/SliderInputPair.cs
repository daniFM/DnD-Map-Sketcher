// Copyright (c) 2022 Daniel Fernández Marqués
// Licensed under the GNU General Public License (GPL) version 3. See the LICENSE file for more details.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderInputPair : MonoBehaviour
{
    public string key;
    [ReadOnly] public float value;

    private Slider slider;
    private InputField inputField;

    public static Action<string, float> OnOutlineValueChanged;
    public static Action<string, float> OnSettingValueChanged;

    void Start()
    {
        slider = GetComponentInChildren<Slider>();
        inputField = GetComponentInChildren<InputField>();

        slider.value = value;
        inputField.text = value.ToString();
        GetComponentInChildren<Text>().text = key;
    }

    public void ChangeOutlineValue(float value)
    {
        ChangeFloat(value);
        OnOutlineValueChanged?.Invoke(key, value);
    }

    public void ChangeOutlineValue(string value)
    {
        ChangeString(value);
        OnOutlineValueChanged?.Invoke(key, this.value);
    }

    public void ChangeSettingValue(float value)
    {
        ChangeFloat(value);
        OnSettingValueChanged?.Invoke(key, value);
    }

    public void ChangeSettingValue(string value)
    {
        ChangeString(value);
        OnSettingValueChanged?.Invoke(key, this.value);
    }

    private void ChangeFloat(float value)
    {
        this.value = value;
        inputField.text = value.ToString();
    }

    private void ChangeString(string value)
    {
        if(value == "")
            this.value = 0;
        else
            this.value = float.Parse(value);
        slider.value = this.value;
    }
}
