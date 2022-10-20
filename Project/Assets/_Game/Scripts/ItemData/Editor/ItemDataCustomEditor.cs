using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(ItemData), true)]
//[CustomEditor(typeof(GenericItemData))]
public class ItemDataCustomEditor : Editor
{
    ReorderableList actionDataList;

    AttributeCollection attributeCollection;

    Dictionary<AttributeType, System.Type> attributeTypeToSystemTypeMap = new Dictionary<AttributeType, System.Type>();
    Dictionary<System.Type, AttributeType> systemTypeToAttributeTypeMap = new Dictionary<System.Type, AttributeType>();

    string[] typeNames;

    void OnEnable()
    {
        AddToTypeConversionMap(AttributeType.Integer, typeof(Attribute<int>));
        AddToTypeConversionMap(AttributeType.Float, typeof(Attribute<float>));
        AddToTypeConversionMap(AttributeType.Color, typeof(Attribute<Color>));
        AddToTypeConversionMap(AttributeType.String, typeof(Attribute<string>));
        AddToTypeConversionMap(AttributeType.Boolean, typeof(Attribute<bool>));
        AddToTypeConversionMap(AttributeType.Vector2, typeof(Attribute<Vector2>));
        AddToTypeConversionMap(AttributeType.Vector3, typeof(Attribute<Vector3>));
        AddToTypeConversionMap(AttributeType.ObjectReference, typeof(Attribute<Object>));

        attributeCollection = (target as ItemData).Attributes;
        attributeCollection.Deserialize();

        serializedObject.Update();
        //serializedObject.ApplyModifiedProperties();

        actionDataList = new ReorderableList(attributeCollection, typeof(AttributeBase), true, true, true, true);
        actionDataList.drawHeaderCallback += rect => EditorGUI.LabelField(rect, "Attributes", EditorStyles.boldLabel);
        actionDataList.onAddCallback += AddNewAttribute;
        actionDataList.drawElementCallback += DrawAttribute;
        actionDataList.elementHeight *= 2f;
    }

    private void AddToTypeConversionMap(AttributeType attributeType, System.Type systemType)
    {
        attributeTypeToSystemTypeMap.Add(attributeType, systemType);
        systemTypeToAttributeTypeMap.Add(systemType, attributeType);
    }

    private void DrawAttribute(Rect rect, int index, bool isActive, bool isFocused)
    {
        var attributeBase = attributeCollection[index];

        Rect nameRect = new Rect(rect);
        nameRect.width /= 4f;
        nameRect.height /= 2f;

        Rect valueRect = new Rect(nameRect);
        valueRect.x += valueRect.width;
        valueRect.width /= 2f;

        Rect typeRect = new Rect(rect);
        typeRect.height /= 2f;
        typeRect.y += typeRect.height;

        EditorGUI.BeginChangeCheck();
        attributeBase.Name = EditorGUI.TextField(nameRect, attributeBase.Name);

        //Case for UnityEngine.Objects
        Object prevValue = null;
        if(attributeBase.IsObject())
        {
            prevValue = ((Attribute<UnityEngine.Object>)attributeBase).Value;

            EditorGUI.BeginChangeCheck();

            var objAttribute = ((Attribute<Object>)attributeBase);
            objAttribute.Value = EditorGUI.ObjectField(valueRect, objAttribute.Value, typeof(Object), false);

            if (EditorGUI.EndChangeCheck())
            {
                var newObj = ((Attribute<UnityEngine.Object>)attributeBase).Value;
                if (!ReferenceEquals(prevValue, newObj))
                {
                    if (prevValue != null)
                    {
                        attributeCollection.unityObjects.Remove(newObj);
                    }
                    attributeCollection.unityObjects.Add(newObj);
                }
                
                attributeCollection.Serialize();
                serializedObject.Update();
                EditorUtility.SetDirty(target);
            }
        }
        else
        {
            EditorGUI.BeginChangeCheck();
            switch (attributeBase)
            {
                case Attribute<int>:
                    var intAttribute = ((Attribute<int>)attributeBase);
                    intAttribute.Value = EditorGUI.IntField(valueRect, intAttribute.Value);
                    break;
                case Attribute<bool>:
                    var boolAttribute = ((Attribute<bool>)attributeBase);
                    boolAttribute.Value = EditorGUI.Toggle(valueRect, boolAttribute.Value);
                    break;
                case Attribute<float>:
                    var floatAttribute = ((Attribute<float>)attributeBase);
                    floatAttribute.Value = EditorGUI.FloatField(valueRect, floatAttribute.Value);
                    break;
                case Attribute<string>:
                    var stringAttribute = ((Attribute<string>)attributeBase);
                    stringAttribute.Value = EditorGUI.TextField(valueRect, stringAttribute.Value);
                    break;
                case Attribute<Color>:
                    var colorAttribute = ((Attribute<Color>)attributeBase);
                    colorAttribute.Value = EditorGUI.ColorField(valueRect, colorAttribute.Value);
                    break;
                //case AttributeType.LayerMask:
                //break;
                case Attribute<System.Enum>:
                    var enumAttribute = ((Attribute<System.Enum>)attributeBase);
                    enumAttribute.Value = EditorGUI.EnumPopup(typeRect, enumAttribute.Value);
                    break;
                case Attribute<Vector2>:
                    var vec2Attribute = ((Attribute<Vector2>)attributeBase);
                    vec2Attribute.Value = EditorGUI.Vector2Field(valueRect, "", vec2Attribute.Value);
                    break;
                case Attribute<Vector3>:
                    var vec3Attribute = ((Attribute<Vector3>)attributeBase);
                    vec3Attribute.Value = EditorGUI.Vector3Field(valueRect, "", vec3Attribute.Value);
                    break;
                case Attribute<Vector4>:
                    var vec4Attribute = ((Attribute<Vector4>)attributeBase);
                    vec4Attribute.Value = EditorGUI.Vector4Field(valueRect, "", vec4Attribute.Value);
                    break;
                case Attribute<Rect>:
                    var rectAttribute = ((Attribute<Rect>)attributeBase);
                    rectAttribute.Value = EditorGUI.RectField(valueRect, "", rectAttribute.Value);
                    break;
                case Attribute<AnimationCurve>:
                    var curveAttribute = ((Attribute<AnimationCurve>)attributeBase);
                    curveAttribute.Value = EditorGUI.CurveField(valueRect, "", curveAttribute.Value);
                    break;
                case Attribute<Bounds>:
                    var boundsAttribute = ((Attribute<Bounds>)attributeBase);
                    boundsAttribute.Value = EditorGUI.BoundsField(valueRect, "", boundsAttribute.Value);
                    break;
                case Attribute<Gradient>:
                    var gradientAttribute = ((Attribute<Gradient>)attributeBase);
                    gradientAttribute.Value = EditorGUI.GradientField(valueRect, "", gradientAttribute.Value);
                    break;
                //case Attribute<Quaternion>:
                //    var quaternionAttribute = ((Attribute<Quaternion>)attributeBase);
                //    quaternionAttribute.Value = EditorGUI.Vector4Field(valueRect, "", quaternionAttribute.Value);
                //    break;
                //case AttributeType.Vector2Int:
                //break;
                //case AttributeType.Vector3Int:
                //break;
                //case AttributeType.RectInt:
                //    break;
                //case AttributeType.BoundsInt:
                //    break;
                //case AttributeType.ManagedReference:
                //    break;
                //case AttributeType.Hash128:
                //    break;
                default:
                    break;
            }
            if (EditorGUI.EndChangeCheck())
            {
                attributeCollection.Serialize();
                serializedObject.Update();
                EditorUtility.SetDirty(target);
            }
        }

        EditorGUI.BeginChangeCheck();

        //string[] typeNames = System.Array.ConvertAll(UnitOptions.s_AllTypes, x => x.Name);
        if (typeNames == null)
        {
            typeNames = UnitOptions.AvailableTypeNames.ToArray();
        }
        var newTypeIndex = EditorGUI.Popup(typeRect, 0, typeNames);
        //attributeBase.Type = (AttributeType)EditorGUI.EnumPopup(typeRect, "Type", attributeBase.Type);

        if (EditorGUI.EndChangeCheck())
        {
            System.Type attributeType = TypeUtility.GetType(typeNames[newTypeIndex]);

            var templateType = typeof(Attribute<>);
            System.Type[] typeArgs = { attributeType };
            System.Type typedAttributeType = templateType.MakeGenericType(typeArgs);


            var attribute = AttributeBase.CreateInstance(typedAttributeType, attributeBase.Name);
            attribute.Type = attributeType;

            //var type = typeof(Attribute<>).MakeGenericType(attributeType);
            //var attribute = System.Activator.CreateInstance(type);

            attributeCollection[index] = attribute;

            attributeCollection.Serialize();
            serializedObject.Update();
            EditorUtility.SetDirty(target);
        }

        serializedObject.ApplyModifiedProperties();
    }

    private System.Type GetSystemTypeFromAttributeType(AttributeType attributeType)
    {
        return attributeTypeToSystemTypeMap[attributeType];
    }

    private AttributeType GetAttributeTypeFromSystemType(System.Type systemType)
    {
        return systemTypeToAttributeTypeMap[systemType];
    }

    private void AddNewAttribute(ReorderableList list)
    {
        int index = list.count - 1;
        var newAttribute = AttributeBase.CreateInstance(typeof(Attribute<bool>), "MyInt");
        newAttribute.Type = typeof(bool);// GetAttributeTypeFromSystemType(typeof(Attribute<bool>));
        attributeCollection.Add(newAttribute);
        serializedObject.Update();
        EditorUtility.SetDirty(target);
        serializedObject.ApplyModifiedProperties();
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        using (new EditorGUI.DisabledScope(true))
            EditorGUILayout.ObjectField("Script", MonoScript.FromScriptableObject((ScriptableObject)target), GetType(), false);

        EditorGUILayout.PropertyField(serializedObject.FindProperty("attributes"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("attributes.serializationData"));

        actionDataList.DoLayoutList();

        serializedObject.ApplyModifiedProperties();
    }
}
