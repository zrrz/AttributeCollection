using OdinSerializer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttributeCollection :IList<AttributeBase>, IList
{
    [SerializeField, HideInInspector]
    public List<Object> unityObjects;

    [SerializeField]
    private byte[] serializationData;

    [Tooltip("An array struct created in initialize that stores the attributes in the collection.")]
    public List<AttributeBase> attributes;

    #region IList<T> Implementation

    public bool IsReadOnly => true;

    public AttributeBase this[int index]
    {
        get
        {
            return attributes?[index];
        }
        set
        {
            attributes[index] = value;
            if (value.IsObject())
            {
                unityObjects.Add(((Attribute<Object>)value).Value);
            }
            Serialize();
        }
    }

    public int Count => attributes.Count;

    public int IndexOf(AttributeBase item)
    {
        return attributes.IndexOf(item);
    }

    public void Insert(int index, AttributeBase item)
    {
        if (item.IsObject())
        {
            unityObjects.Add(((Attribute<Object>)item).Value);
        }
        attributes.Insert(index, item);
    }

    public void RemoveAt(int index)
    {
        var attribute = attributes[index];

        attributes.RemoveAt(index);
        if (attribute.IsObject())
        {
            unityObjects.Remove(((Attribute<Object>)attribute).Value);
        }
        Serialize();
    }

    public void Add(AttributeBase item)
    {
        attributes.Add(item);
        if (item.IsObject())
        {
            unityObjects.Add(((Attribute<Object>)item).Value);
        }
        Serialize();
    }

    public void Clear()
    {
        foreach (var attribute in attributes)
        {
            if (attribute.IsObject())
            {
                unityObjects.Remove(((Attribute<Object>)attribute).Value);
            }
        }

        attributes.Clear();
        Serialize();
    }

    public bool Contains(AttributeBase item)
    {
        return attributes.Contains(item);
    }

    public void CopyTo(AttributeBase[] array, int arrayIndex)
    {
        attributes.CopyTo(array, arrayIndex);
    }

    public bool Remove(AttributeBase item)
    {
        var attribute = attributes.Find(ex => ex == item);

        if (attribute.IsObject())
        {
            unityObjects.Remove(((Attribute<Object>)attribute).Value);
        }

        bool remove = attributes.Remove(item);
        if (remove)
        {
            Serialize();
        }
        return remove;
    }

    public IEnumerator<AttributeBase> GetEnumerator()
    {
        if (attributes == null) { yield break; }

        for (int i = 0; i < attributes.Count; i++) { yield return attributes[i]; }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    #endregion

    #region IList implementation

    public bool IsFixedSize => false;

    public bool IsSynchronized => false;

    public object SyncRoot => null;

    object IList.this[int index]
    {
        get
        {
            return attributes?[index];
        }
        set
        {
            attributes[index] = (AttributeBase)value;
            Serialize();
        }
    }

    public int Add(object value)
    {
        Add((AttributeBase)value);
        return attributes.Count;
    }

    public bool Contains(object value)
    {
        return Contains((AttributeBase)value);
    }

    public int IndexOf(object value)
    {
        return IndexOf((AttributeBase)value);
    }

    public void Insert(int index, object value)
    {
        Insert(index, (AttributeBase)value);
    }

    public void Remove(object value)
    {
        Remove((AttributeBase)value);
    }

    public void CopyTo(System.Array array, int index)
    {
        CopyTo(array, index);
    }
    #endregion

    /// <summary>
    /// Returns the attribute.
    /// </summary>
    /// <typeparam name="T">The attribute type.</typeparam>
    /// <param name="attributeName">The name of the attribute.</param>
    /// <returns>Returns the attribute.</returns>
    public T GetAttribute<T>(string attributeName) where T : AttributeBase
    {
        TryGetAttribute(attributeName, out var attribute);
        return attribute as T;
    }

    public void Serialize()
    {
        Debug.Log("Serializing");

        // Unity should be allowed to handle serialization and deserialization of its own weird objects.
        // So if your data-graph contains UnityEngine.Object types, you will need to provide Odin with
        // a list of UnityEngine.Object which it will then use as an external reference resolver.
        //List<UnityEngine.Object> unityObjectReferences = new List<UnityEngine.Object>();
        foreach (var obj in attributes)
        {
            if (obj.IsObject())
            {
                unityObjects.Add(((Attribute<UnityEngine.Object>)obj).Value);
            }
        }


        DataFormat dataFormat = DataFormat.Binary;
        //DataFormat dataFormat = DataFormat.JSON;

        serializationData = SerializationUtility.SerializeValue(attributes, dataFormat, out unityObjects);
    }

    public List<AttributeBase> Deserialize()
    {
        //List<UnityEngine.Object> unityObjectReferences = new List<UnityEngine.Object>();
        DataFormat dataFormat = DataFormat.Binary;
        //DataFormat dataFormat = DataFormat.JSON;

        attributes = SerializationUtility.DeserializeValue<List<AttributeBase>>(serializationData, dataFormat, unityObjects);
        if (attributes == null)
        {
            attributes = new List<AttributeBase>();
        }
        return attributes;
    }

    public bool UpdateAttributesToMatchList(IReadOnlyList<AttributeBase> requiredAttributes, bool removeAdditionalAttributes)
    {
        if (removeAdditionalAttributes)
        {
            RemoveAdditionalAttributes(requiredAttributes);
        }
        return CreateNewAttributesIfMissing(requiredAttributes);
    }

    public bool RemoveAdditionalAttributes(IReadOnlyList<AttributeBase> requiredAttributes)
    {
        //if (m_Attributes == null)
        //{
        //    Debug.LogWarning($"The AttributeCollection '{this}' was not initialized yet you are trying to use it.");
        //}
        //for (int i = 0; i < m_Attributes.Count; i++)
        //{
        //    var attribute = m_Attributes[i];
        //    var match = false;
        //    for (int j = 0; j < requiredAttributes.Count; j++)
        //    {
        //        var requiredAttribute = requiredAttributes[j];

        //        if ((attribute.Name == requiredAttribute.Name) && attribute.GetType() == requiredAttribute.GetType())
        //        {
        //            if (!match)
        //            {
        //                match = true;
        //            }
        //            else
        //            {
        //                Debug.LogWarning("The attributes matched twice, some attribute had been duplicated.");
        //            }
        //        }
        //        else if (attribute.ConnectionID == requiredAttribute.ConnectionID && attribute.GetType() == requiredAttribute.GetType())
        //        {
        //            attribute.Rename(requiredAttribute.Name);
        //            if (match == true)
        //            {
        //                Debug.LogWarning("The attributes matched twice, some attribute had been duplicated.");
        //            }
        //            match = true;
        //        }
        //    }

        //    if (match == false)
        //    {
        //        RemoveAttribute(attribute);
        //        i--;
        //    }
        //}

        return true;
    }

    /// <summary>
    /// Creates any attribute in the list that is not yet part of the collection.
    /// </summary>
    /// <param name="requiredAttributes">The attributes that are required to exist in the collection.</param>
    /// <returns>Returns false if something fails while adding the new attributes.</returns>
    public bool CreateNewAttributesIfMissing(IReadOnlyList<AttributeBase> requiredAttributes)
    {
        //for (int i = 0; i < requiredAttributes.Count; i++)
        //{
        //    var requiredAttribute = requiredAttributes[i];
        //    if (TryGetAttribute(requiredAttribute.Name, out var variant))
        //    {
        //        if (variant.GetType() != requiredAttribute.GetType())
        //        {
        //            Debug.LogError("Unable to create new Attribute. There are two different Attributes with the same name.");
        //            return false;
        //        }
        //    }
        //    else
        //    {
        //        bool foundConnectionMatch = false;
        //        for (int j = 0; j < attributes.Count; j++)
        //        {
        //            var existingAttribute = attributes[j];
        //            if (requiredAttribute.ConnectionID == existingAttribute.ConnectionID
        //                && existingAttribute.GetType() == requiredAttribute.GetType())
        //            {
        //                existingAttribute.Rename(requiredAttribute.Name);
        //                foundConnectionMatch = true;
        //            }
        //        }

        //        if (foundConnectionMatch) { continue; }
        //        var attribute = AttributeBase.CreateInstance(requiredAttribute.GetType(), requiredAttribute.Name,
        //            Type.Missing, VariantType.Inherit);

        //        //Items that are note Default Items should not preevaluate attributes by default. Everything else should
        //        if (AttachedItem == null || AttachedItem == AttachedItem.ItemDefinition.DefaultItem)
        //        {
        //            attribute?.ReevaluateValue(true);
        //        }

        //        AddAttribute(attribute);
        //    }
        //}

        //ReevaluateAll(false);

        return true;
    }

    /// <summary>
    /// Try get the attribute.
    /// Use this if you do not know the type of the attribute.
    /// </summary>
    /// <param name="attributeName">The name of the attribute.</param>
    /// <param name="attribute">Output of the attribute.</param>
    /// <returns>Returns true if the attribute exists in the collection.</returns>
    public bool TryGetAttribute(string attributeName, out AttributeBase attribute)
    {
        if (attributes == null)
        {
            attribute = null;
            return false;
        }

        for (int i = 0; i < attributes.Count; i++)
        {
            if (attributes[i].Name != attributeName) { continue; }

            attribute = attributes[i];
            return true;
        }

        attribute = null;
        return false;
    } 
}
