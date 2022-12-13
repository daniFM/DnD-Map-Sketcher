// Copyright (c) Daniel Fernández 2022


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineController : MonoBehaviour
{
    [SerializeField] private Material outlineMaterial;
    [SerializeField] private float cameraCorrection;

    [SerializeField] private SliderInputPair normalThickness;
    [SerializeField] private SliderInputPair depthThickness;
    [SerializeField] private SliderInputPair depthSensitivity;
    [SerializeField] private SliderInputPair normalsSensitivity;
    [SerializeField] private SliderInputPair colorSensitivity;

    private Material initMaterial;
    private float cameraInitSize;

    private void Start()
    {
        initMaterial = new Material(outlineMaterial);
    }

    private void OnDestroy()
    {
        if(initMaterial != null)
            outlineMaterial.CopyPropertiesFromMaterial(initMaterial);
    }

    private void OnEnable()
    {
        cameraInitSize = Camera.main.orthographicSize;
        SliderInputPair.OnOutlineValueChanged += UpdateMaterial;
    }

    private void OnDisable()
    {
        SliderInputPair.OnOutlineValueChanged -= UpdateMaterial;
    }

    private void Awake()
    {
        normalThickness.value = outlineMaterial.GetFloat(normalThickness.key);
        depthThickness.value = outlineMaterial.GetFloat(depthThickness.key);
        depthSensitivity.value = outlineMaterial.GetFloat(depthSensitivity.key);
        normalsSensitivity.value = outlineMaterial.GetFloat(normalsSensitivity.key);
        colorSensitivity.value = outlineMaterial.GetFloat(colorSensitivity.key);
    }

    public void CameraCorrection(float cameraSize)
    {
        UpdateMaterial(depthSensitivity.key, depthSensitivity.value + (cameraInitSize - cameraSize) * cameraCorrection);
    }

    private void UpdateMaterial(string key, float value)
    {
        outlineMaterial.SetFloat(key, value);
    }
}
