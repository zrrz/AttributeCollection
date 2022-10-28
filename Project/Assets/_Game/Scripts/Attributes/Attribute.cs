using OdinSerializer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Attribute<T> : AttributeBase
{
    [SerializeField] public T Value;

    /// <summary>
    /// Default constructor, required by the Editor, the extensions must also have one.
    /// </summary>
    public Attribute() : base("NewAttribute")
    {
        Value = default(T);
    }

    /// <summary>
    /// Constructor with a value for the override value, the extensions must have the same constructor, with the same order of parameter.
    /// The reason is that we use reflection to create attributes at runtime.
    /// </summary>
    /// <param name="name">The attribute name.</param>
    public Attribute(string name, T Value = default(T))
        : base(name)
    {
        this.Value = Value;
    }

    /// <summary>
    /// Static function to get the attribute value Type.
    /// </summary>
    /// <returns>Returns the type of the attribute value.</returns>
    public static System.Type GetAttributeValueType()
    {
        return typeof(T);
    }

    public override Object GetUnityObject()
    {
        if (IsUnityObject())
        {
            //HACK no way to do this cleanly without a type constraint which we can't do because this can be templatized to non UnityEngine Objects
            UnityEngine.Object obj = (UnityEngine.Object)(System.Object)Value;
            return obj;
        }
        return null;
    }

    public override void SetUnityObject(Object unityObject)
    {
        if (IsUnityObject())
        {
            Value = (T)(System.Object)unityObject;
        }
    }

    public override System.Type GetValueType()
    {
        return typeof(T);
    }
}
