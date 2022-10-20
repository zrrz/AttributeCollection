using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using UnityEngine;

[System.Serializable]
public abstract class AttributeBase //: ISerializationCallbackReceiver
{
    [SerializeField, HideInInspector] protected byte[] serializationData;

    public System.Type Type;
    public string Name;
    //public object Value;

    /// <summary>
    /// Returns the type of the value of the attribute.
    /// </summary>
    /// <returns>The value Type.</returns>
    public abstract System.Type GetValueType();

    public static AttributeBase CreateInstance(System.Type type, string name, string modifyExpression = "")
    {
        //Use reflection to create the instance.
        //Parameters example: string name, int OverrideValue = 0, VariantType variantType = VariantType.Override, string modifyExpression = "" .

        if (type.IsSubclassOf(typeof(AttributeBase)) == false)
        {
            Debug.LogError("Must be a subclass of AttributeBase! Did you maybe use a T instead of an Attribute<T>?");
            return null;
        }

        var getTypeMethodInfo = type.GetMethod("GetAttributeValueType");
        System.Type valueType = null;
        if (getTypeMethodInfo != null)
        {
            valueType = (System.Type)getTypeMethodInfo.Invoke(null, null); // (null, null) means calling static method with no parameters
        }

        //if (value == null || valueType != value.GetType())
        //{
        //    value = System.Type.Missing;
        //}

        var newAttribute = System.Activator.CreateInstance(type,
            BindingFlags.CreateInstance | BindingFlags.Public | BindingFlags.Instance | BindingFlags.OptionalParamBinding, null,
            new object[] { name/*, value, modifyExpression */},
            CultureInfo.CurrentCulture) as AttributeBase;

        return newAttribute;
    }

    /// <summary>
    /// Default Constructor used by the Editor to create an empty attributeBase.
    /// It is protected so that the subClasses can take advantage of it.
    /// Can be used through Reflection.
    /// </summary>
    protected AttributeBase()
    {
        Name = "NewAttribute";
        //m_ModifyExpression = "";
    }

    /// <summary>
    /// Constructor base should be used by subclass for convenience.
    /// </summary>
    /// <param name="name">The attribute name.</param>
    /// <param name="variantType">The variant Type.</param>
    /// <param name="modifyExpression">The modify expression.</param>
    protected AttributeBase(string name)
    {
        Name = name;
        //m_ModifyExpression = modifyExpression;
    }

    public bool IsObject()
    {
        return Type.IsSubclassOf(typeof(UnityEngine.Object));
    }

    //public virtual void Serialize() { }

    //public virtual object Deserialize() { return null; }

    //public void OnBeforeSerialize()
    //{
    //    Serialize();
    //}

    //public void OnAfterDeserialize()
    //{
    //    Deserialize();
    //}
}
