// Copyright (c) 2022 Daniel Fernández Marqués

using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ControlsScriptableObject))]
public class ControlsScriptableObjectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ControlsScriptableObject controls = (ControlsScriptableObject)target;

        base.OnInspectorGUI();

        GUILayout.Space(30);

        GUILayout.BeginHorizontal();

        if(GUILayout.Button("Generate empty config"))
        {
            controls.GenerateEmptyControls();
        }

        if(GUILayout.Button("Update config"))
        {
            controls.UpdateConfigEditor();
        }

        GUILayout.EndHorizontal();
    }
}
