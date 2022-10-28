using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using UnityEngine;

[System.Serializable]
public abstract class AttributeBase
{
    [SerializeField, HideInInspector] protected byte[] serializationData;

    public System.Type Type;
    public string FieldName;
    public string TypeName;

    /// <summary>
    /// Returns the type of the value of the attribute.
    /// </summary>
    /// <returns>The value Type.</returns>
    public abstract System.Type GetValueType();

    public static AttributeBase CreateInstance(System.Type type, string name, System.Type attributeType, string typeName)
    {
        //Use reflection to create the instance.
        //Parameters example: string name, int OverrideValue = 0, VariantType variantType = VariantType.Override, string modifyExpression = "" .

        if (type.IsSubclassOf(typeof(AttributeBase)) == false)
        {
            Debug.LogError("Must be a subclass of AttributeBase! Did you maybe use a T instead of an Attribute<T>?");
            return null;
        }

        //if (value == null || valueType != value.GetType())
        //{
        //    value = System.Type.Missing;
        //}

        var newAttribute = System.Activator.CreateInstance(type,
            BindingFlags.CreateInstance | BindingFlags.Public | BindingFlags.Instance | BindingFlags.OptionalParamBinding, null,
            new object[] { name/*, value, modifyExpression */},
            CultureInfo.CurrentCulture) as AttributeBase;

        newAttribute.Type = attributeType;
        newAttribute.TypeName = typeName;
        return newAttribute;
    }

    public abstract UnityEngine.Object GetUnityObject();
    public abstract void SetUnityObject(UnityEngine.Object unityObject);

    /// <summary>
    /// Default Constructor used by the Editor to create an empty attributeBase.
    /// It is protected so that the subClasses can take advantage of it.
    /// Can be used through Reflection.
    /// </summary>
    protected AttributeBase()
    {
        FieldName = "NewAttribute";
    }

    /// <summary>
    /// Constructor base should be used by subclass for convenience.
    /// </summary>
    /// <param name="name">The attribute name.</param>
    /// <param name="variantType">The variant Type.</param>
    /// <param name="modifyExpression">The modify expression.</param>
    protected AttributeBase(string name)
    {
        FieldName = name;
    }

    public bool IsUnityObject()
    {
        return Type.IsSubclassOf(typeof(UnityEngine.Object));
    }
}
