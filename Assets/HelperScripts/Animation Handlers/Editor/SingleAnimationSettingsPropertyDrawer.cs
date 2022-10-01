using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//[CustomPropertyDrawer(typeof(SingleAnimationSettings))]
public class SingleAnimationSettingsPropertyDrawer : PropertyDrawer
{
    bool showPosition = false;
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        bool isUsed = property.FindPropertyRelative("isUsed").boolValue;

        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        Rect rect = new Rect(position.position, Vector2.one * 20);

        isUsed = EditorGUI.Toggle(rect, isUsed);
        SetIsUsedProperty(property, isUsed);
        position.x += 30;
        if (isUsed)
        {
            showPosition = EditorGUI.BeginFoldoutHeaderGroup(position, showPosition, new GUIContent());
            if (showPosition)
            {
                position.y += 20;
                EditorGUI.PropertyField(position, property.FindPropertyRelative("sprites"), GUIContent.none, true);
                EditorGUI.EndFoldoutHeaderGroup();
            }
        }
        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return showPosition?30:20;
    }

    private void SetIsUsedProperty(SerializedProperty prop, bool value)
    {
        var propRel = prop.FindPropertyRelative("isUsed");
        propRel.boolValue = value;
        prop.serializedObject.ApplyModifiedProperties();
    }
}
