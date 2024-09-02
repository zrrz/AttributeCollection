using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Assertions;

[CustomPropertyDrawer(typeof(ItemActionValueField))]
public class ItemActionValueFieldDrawer : PropertyDrawer
{
    private ItemActionValueField itemActionValueField;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //TODO cache this?
        itemActionValueField = property.boxedValue as ItemActionValueField;
        if (itemActionValueField.Type == null)
        {
            Debug.LogError("Zack fix this");
            //SetFieldType(property, TypeLoader.AllTypes[0]);
        }

        Rect nameRect = position;
        nameRect.width /= 2f;

        Rect typeRect = nameRect;
        typeRect.x += typeRect.width;

        EditorGUI.PropertyField(nameRect, property.FindPropertyRelative("FieldName"));
        EditorGUI.BeginChangeCheck();
        int newTypeIndex = TypeLoaderEditorUtility.DrawTypePopup(typeRect, itemActionValueField.Type.Name);
        if(EditorGUI.EndChangeCheck())
        {
            SetFieldType(property, TypeLoader.AllTypes[newTypeIndex]);
        }
    }

    private void SetFieldType(SerializedProperty property, Type newType)
    {
        itemActionValueField.Type = newType;
        property.boxedValue = itemActionValueField;
        (property.boxedValue as ItemActionValueField).Type = newType;
        //property.FindPropertyRelative("typeName").stringValue = newType.Name;
        property.serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(property.serializedObject.targetObject);
    }
}
