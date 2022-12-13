// Copyright (c) Daniel Fernández 2022


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolSettingsController : MonoBehaviour
{
    [SerializeField] private CameraMovement cameraMovement;

    [SerializeField] private SliderInputPair sensitivityXSlider;
    [SerializeField] private SliderInputPair sensitivityYSlider;

    private void OnEnable()
    {
        SliderInputPair.OnSettingValueChanged += UpdateSetting;
    }

    private void OnDisable()
    {
        SliderInputPair.OnSettingValueChanged -= UpdateSetting;
    }

    private void Awake()
    {
        sensitivityXSlider.value = cameraMovement.sensitivityX;
        sensitivityYSlider.value = cameraMovement.sensitivityY;
    }

    private void UpdateSetting(string key, float value)
    {
        if(key == sensitivityXSlider.key)
        {
            cameraMovement.sensitivityX = value;
        }
        else if(key == sensitivityYSlider.key)
        {
            cameraMovement.sensitivityY = value;
        }
        else
        {
            Debug.LogError("Mismatch key received from SliderInputPair", this);
        }
    }
}
