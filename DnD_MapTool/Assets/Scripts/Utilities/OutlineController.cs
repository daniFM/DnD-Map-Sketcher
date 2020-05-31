using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineController : MonoBehaviour
{
    [SerializeField] private Material outlineMaterial;

    [SerializeField] private SliderInputPair normalThickness;
    [SerializeField] private SliderInputPair depthThickness;
    [SerializeField] private SliderInputPair depthSensitivity;
    [SerializeField] private SliderInputPair normalsSensitivity;
    [SerializeField] private SliderInputPair colorSensitivity;

    private void OnEnable()
    {
        SliderInputPair.OnValueChanged += UpdateMaterial;
    }

    private void OnDisable()
    {
        SliderInputPair.OnValueChanged += UpdateMaterial;
    }

    private void UpdateMaterial(string key, float value)
    {
        outlineMaterial.SetFloat(key, value);
    }
}
