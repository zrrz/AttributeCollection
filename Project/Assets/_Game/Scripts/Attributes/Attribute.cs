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
        //base.Value = Value;
    }

    //public override void Serialize()
    //{
    //    // Unity should be allowed to handle serialization and deserialization of its own weird objects.
    //    // So if your data-graph contains UnityEngine.Object types, you will need to provide Odin with
    //    // a list of UnityEngine.Object which it will then use as an external reference resolver.
    //    List<UnityEngine.Object> unityObjectReferences = new List<UnityEngine.Object>();

    //    //DataFormat dataFormat = DataFormat.Binary;
    //    DataFormat dataFormat = DataFormat.JSON;

    //    serializationData = SerializationUtility.SerializeValue(Value, dataFormat, out unityObjectReferences);
    //}

    //public override object Deserialize()
    //{
    //    List<UnityEngine.Object> unityObjectReferences = new List<UnityEngine.Object>();
    //    //DataFormat dataFormat = DataFormat.Binary;
    //    DataFormat dataFormat = DataFormat.JSON;

    //    Value = SerializationUtility.DeserializeValue<T>(serializationData, dataFormat, unityObjectReferences);
    //    return Value;
    //}

    //public T DeserializeValue()
    //{
    //    return (T)Deserialize();
    //}

    /// <summary>
    /// Static function to get the attribute value Type.
    /// </summary>
    /// <returns>Returns the type of the attribute value.</returns>
    public static System.Type GetAttributeValueType()
    {
        return typeof(T);
    }

    public override System.Type GetValueType()
    {
        return typeof(T);
    }
}
