using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//[CustomPropertyDrawer(typeof(AttributeCollection))]
public class AttributeCollectionCustomDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUILayout.LabelField("WHYYYYYYY");
        //base.OnGUI(position, property, label);
    }
}
