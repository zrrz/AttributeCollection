using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class TypeLoaderEditorUtility
{
    public static int DrawTypePopup(Rect typeRect, string currentTypeName)
    {
        string[] names = TypeLoader.AllTypesNames;
        int newTypeIndex = 0;
        int currentIndex = System.Array.IndexOf(names, currentTypeName);
        if (currentIndex == -1)
        {
            if (string.IsNullOrEmpty(currentTypeName))
            {
                currentTypeName = "<Empty>";
            }
            List<string> namesList = new List<string>(names);
            namesList.Insert(0, $"<Missing: {currentTypeName}>");
            newTypeIndex = EditorGUI.Popup(typeRect, 0, namesList.ToArray());
            if (newTypeIndex != 0)
            {
                newTypeIndex--;
            }
        }
        else
        {
            newTypeIndex = EditorGUI.Popup(typeRect, currentIndex, names);
        }
        return newTypeIndex;
    }
}
