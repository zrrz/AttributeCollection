using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomPropertyDrawer(typeof(TestProperty))]
public class TestPropertyDrawer : PropertyDrawer
{
    ReorderableList reList;
    bool initialized = false;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (!initialized)
        {
            Debug.Log("Initialize");
            TestProperty testProp = property.boxedValue as TestProperty;
            reList = new ReorderableList(testProp.myList, typeof(int), true, true, true, true);
            initialized = true;
        }
        bool was = GUI.enabled;
        GUI.enabled = true;
        EditorGUI.BeginChangeCheck();
        reList.DoList(position);
        GUI.enabled = was;

        if (EditorGUI.EndChangeCheck())
        {
            property.serializedObject.ApplyModifiedProperties();
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        // It is probably better to do initializations before calculating height as it is done before OnGUI.
        // This way we avoid null checks in GetPropertyHeight that would otherwise would be required.
        if (!initialized)
        {
            Debug.Log("init");
            TestProperty testProp = property.boxedValue as TestProperty;
            reList = new ReorderableList(testProp.myList, typeof(int), true, true, true, true);
            initialized = true;
        }
        
        return reList.GetHeight();
    }
}
