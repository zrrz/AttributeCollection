using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

//CustomEditor(typeof(ItemData), true)]
public class ItemDataCustomEditor : Editor
{
    //private AttributeCollectionDrawer attributeCollectionDrawer;

    void OnEnable()
    {
        serializedObject.Update();
        //attributeCollectionDrawer = new AttributeCollectionDrawer(serializedObject);
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        using (new EditorGUI.DisabledScope(true))
            EditorGUILayout.ObjectField("Script", MonoScript.FromScriptableObject((ScriptableObject)target), GetType(), false);

        EditorGUILayout.PropertyField(serializedObject.FindProperty("attributes"));
        //EditorGUILayout.PropertyField(serializedObject.FindProperty("attributes.serializationData"));

        //attributeCollectionDrawer.DoLayoutList();

        serializedObject.ApplyModifiedProperties();
    }
}
