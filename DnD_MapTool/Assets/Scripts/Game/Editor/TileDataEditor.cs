using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(TileData))]
public class TileDataEditor: ReadOnlyDrawer
{
    //private bool show = true;

    //public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    //{
    //    //return EditorGUI.GetPropertyHeight(property, label, true);
    //    return 0; 
    //}

    //public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
    //{
    //    SerializedProperty maxSize = property.FindPropertyRelative("maxSize");
    //    SerializedProperty tileTypes = property.FindPropertyRelative("tileTypes");
    //    SerializedProperty transforms = property.FindPropertyRelative("transforms");

    //    show = EditorGUILayout.Foldout(show, property.displayName);
    //    if(show)
    //    {
    //        EditorGUILayout.BeginHorizontal();
    //        EditorGUILayout.Space(15);
    //        EditorGUILayout.BeginVertical();
    //        EditorGUILayout.PropertyField(maxSize);

    //        if(tileTypes != null && transforms != null)
    //        {
    //            GUI.enabled = false;

    //            //EditorGUI.PropertyField(rect, property, label, false);
    //            EditorGUILayout.PropertyField(tileTypes);
    //            EditorGUILayout.PropertyField(transforms);

    //            GUI.enabled = true;
    //        }

    //        EditorGUILayout.EndVertical();
    //        EditorGUILayout.EndHorizontal();
    //    }
    //}
}