using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemActionValueField : ISerializationCallbackReceiver
{
    public string FieldName;

    public System.Type Type = null;

    //public System.Type Type => type;

    //public System.Type Type {
    //    get
    //    {
    //        if(type != null)
    //        {
    //            return type;
    //        }

    //        //else parse it from Typename
    //        if(string.IsNullOrEmpty(TypeName))
    //        {
    //            return typeof(int);
    //        }
    //        return System.Type.GetType(TypeName);
    //    }
    //    set 
    //    {
    //        typeName = value.Name;
    //        type = value;
    //    }
    //}

    /// <summary>
    /// Used for de/serialization
    /// </summary>
    [SerializeField] private string typeName;
    public string TypeName => typeName;

    //public ItemActionValueField()
    //{
    //    FieldName = "";
    //    //if(!string.IsNullOrEmpty(typeName))
    //    //{
    //    //    type = 
    //    //}
    //    //else
    //    //{
    //    //    Type = typeof(int);
    //    //}
    //    type = Type;
    //}

    public void OnBeforeSerialize()
    {
        if(Type == null)
        {
            Type = typeof(int);
        }
        typeName = Type.AssemblyQualifiedName;
    }

    public void OnAfterDeserialize()
    {
        Type = System.Type.GetType(TypeName);
    }
}
