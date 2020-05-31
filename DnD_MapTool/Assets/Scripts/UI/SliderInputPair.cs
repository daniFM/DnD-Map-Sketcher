using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderInputPair : MonoBehaviour
{
    [SerializeField] private string key;
    public string Key { get { return key; } }
    [SerializeField] private float value;
    public float Value { get { return value; } }

    private Slider slider;
    private InputField inputField;

    public static Action<string, float> OnValueChanged;

    void Start()
    {
        slider = GetComponent<Slider>();
        inputField = GetComponentInChildren<InputField>();

        slider.value = value;
        inputField.text = value.ToString();
        GetComponentInChildren<Text>().text = key;
    }

    public void ChangeValue(float value)
    {
        this.value = value;
        inputField.text = value.ToString();

        OnValueChanged?.Invoke(key, value);
    }

    public void ChangeValue(string value)
    {
        if(value == "")
            this.value = 0;
        else
            this.value = float.Parse(value);
        slider.value = this.value;

        OnValueChanged?.Invoke(key, this.value);
    }
}
