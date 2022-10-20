using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine.Rendering;
using System;

[CreateAssetMenu(fileName = "ItemData", menuName = "ItemData")]
public class ItemData : ScriptableObject
{
    [SerializeField] private AttributeCollection attributes;

    public AttributeCollection Attributes {
        get
        {
            if(attributes == null)
            {
                attributes = new AttributeCollection();
            }
            return attributes;
        }
    }
}
