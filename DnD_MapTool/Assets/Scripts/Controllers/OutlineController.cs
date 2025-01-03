// Copyright (c) 2022 Daniel Fernández Marqués
// Licensed under the GNU General Public License (GPL) version 3. See the LICENSE file for more details.

using System.Collections;
using System.Collections.Generic;
using Linework.EdgeDetection;
using UnityEngine;

public class OutlineController : MonoBehaviour
{
    public enum Keys
    {
        _NormalThickness,
        _NormalsSensitivity,
        _DepthThickness,
        _DepthSensitivity,
        _ColorSensitivity
    }

    // [SerializeField] private Material outlineMaterial;
    [SerializeField] private float cameraCorrection;

    [SerializeField] private EdgeDetectionSettings edgeDetectionSettingsDepth;
    [SerializeField] private EdgeDetectionSettings edgeDetectionSettingsNormals;

    [SerializeField] private SliderInputPair normalThickness;
    [SerializeField] private SliderInputPair depthThickness;
    [SerializeField] private SliderInputPair depthSensitivity;
    [SerializeField] private SliderInputPair normalsSensitivity;
    [SerializeField] private SliderInputPair colorSensitivity;

    // private Material initMaterial;
    private float cameraInitSize;

    // private void Start()
    // {
    //     initMaterial = new Material(outlineMaterial);
    // }

    // private void OnDestroy()
    // {
    //     if(initMaterial != null)
    //         outlineMaterial.CopyPropertiesFromMaterial(initMaterial);
    // }

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
        // normalThickness.value = outlineMaterial.GetFloat(normalThickness.key);
        // depthThickness.value = outlineMaterial.GetFloat(depthThickness.key);
        // depthSensitivity.value = outlineMaterial.GetFloat(depthSensitivity.key);
        // normalsSensitivity.value = outlineMaterial.GetFloat(normalsSensitivity.key);
        // colorSensitivity.value = outlineMaterial.GetFloat(colorSensitivity.key);

        normalThickness.value = edgeDetectionSettingsNormals.outlineWidth;
        normalsSensitivity.value = edgeDetectionSettingsNormals.normalSensitivity;
        depthThickness.value = edgeDetectionSettingsDepth.outlineWidth;
        depthSensitivity.value = edgeDetectionSettingsDepth.depthSensitivity;
    }

    public void CameraCorrection(float cameraSize)
    {
        UpdateMaterial(depthSensitivity.key, depthSensitivity.value + (cameraInitSize - cameraSize) * cameraCorrection);
    }

    private void UpdateMaterial(string key, float value)
    {
        Keys keyEnum = (Keys)System.Enum.Parse(typeof(Keys), key);

        switch (keyEnum)
        {
            case Keys._NormalThickness:
                edgeDetectionSettingsNormals.outlineWidth = value;
                break;
            case Keys._DepthThickness:
                edgeDetectionSettingsDepth.outlineWidth = value;
                break;
            case Keys._DepthSensitivity:
                edgeDetectionSettingsDepth.depthSensitivity = value;
                break;
            case Keys._NormalsSensitivity:
                edgeDetectionSettingsNormals.normalSensitivity = value;
                break;
            case Keys._ColorSensitivity:
                break;
        }
    }
}
