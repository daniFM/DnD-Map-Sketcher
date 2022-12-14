// Copyright (c) 2022 Daniel Fernández Marqués

using UnityEngine;
using UnityEditor;

public class RangeDrawer: PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        float slashWidth = 15;
        float width = position.width / 2 - slashWidth / 2;
        var amountRect = new Rect(position.x, position.y, width, position.height);
        var slashRect = new Rect(position.x + width, position.y, slashWidth, position.height);
        var unitRect = new Rect(position.x + width + slashWidth, position.y, width, position.height);

        EditorGUI.PropertyField(amountRect, property.FindPropertyRelative("max"), GUIContent.none);
        EditorGUI.LabelField(slashRect, " -");
        EditorGUI.PropertyField(unitRect, property.FindPropertyRelative("min"), GUIContent.none);

        EditorGUI.EndProperty();
    }
}

[CustomPropertyDrawer(typeof(RangeInt))]
public class RangeIntDrawer: RangeDrawer { }

[CustomPropertyDrawer(typeof(RangeFloat))]
public class RangeFloatDrawer: RangeDrawer { }
