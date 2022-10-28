using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Assertions;

[CustomPropertyDrawer(typeof(AttributeCollection))]
public class AttributeCollectionDrawer : PropertyDrawer
{
    private static string[] typeNames;

    private SerializedObject serializedObject;
    private AttributeCollection attributeCollection;
    private ReorderableList attributeList;

    private bool initialized = false;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (!initialized)
        {
            Debug.Log("Initializing");
            Initialize(property.serializedObject);
            initialized = true;
        }
        attributeList.DoList(position);
    }

    private void Initialize(SerializedObject serializedObject)
    {
        var itemData = (serializedObject.targetObject as ItemData);
        Assert.IsNotNull(itemData);

        attributeCollection = itemData.Attributes;
        attributeCollection.Deserialize();

        this.serializedObject = serializedObject;

        attributeList = new ReorderableList(attributeCollection.Attributes, typeof(AttributeBase), true, true, true, true);
        attributeList.drawHeaderCallback += rect => EditorGUI.LabelField(rect, "Attributes", EditorStyles.boldLabel);
        attributeList.onAddCallback += AddNewAttribute;
        attributeList.onRemoveCallback += RemoveAttribute;
        attributeList.drawElementCallback += DrawAttribute;
        attributeList.elementHeight *= 2f;
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
        attributeBase.FieldName = EditorGUI.TextField(nameRect, attributeBase.FieldName);

        //Case for UnityEngine.Objects
        Object prevValue = null;
        if (attributeBase.IsUnityObject())
        {
            prevValue = attributeBase.GetUnityObject();

            EditorGUI.BeginChangeCheck();

            var unityObject = attributeBase.GetUnityObject();
            var newUnityObject = EditorGUI.ObjectField(valueRect, unityObject, attributeBase.Type, false);

            if (EditorGUI.EndChangeCheck())
            {
                attributeBase.SetUnityObject(newUnityObject);
                attributeCollection.Serialize();
                serializedObject.Update();
                EditorUtility.SetDirty(serializedObject.targetObject);
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
                case Attribute<LayerMask>:
                    var layerMaskAttribute = ((Attribute<LayerMask>)attributeBase);
                    string[] layers = Enumerable.Range(0, 31).Select(index => LayerMask.LayerToName(index)).Where(l => !string.IsNullOrEmpty(l)).ToArray();
                    layerMaskAttribute.Value = EditorGUI.MaskField(valueRect, layerMaskAttribute.Value, layers);
                    break;
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
                default:
                    Debug.LogError("No case for: " + attributeBase.TypeName);
                    break;
            }
            if (EditorGUI.EndChangeCheck())
            {
                attributeCollection.Serialize();
                serializedObject.Update();
                EditorUtility.SetDirty(serializedObject.targetObject);
            }
        }

        EditorGUI.BeginChangeCheck();

        if (typeNames == null)
        {
            typeNames = TypeLoader.AllTypesNames;
        }
        string[] names = typeNames;
        int newTypeIndex = 0;
        int currentIndex = System.Array.IndexOf(typeNames, attributeBase.TypeName);
        if (currentIndex == -1)
        {
            string currentTypeName = attributeBase.TypeName;
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

        if (EditorGUI.EndChangeCheck())
        {
            System.Type attributeType = TypeLoader.AllTypes[newTypeIndex];

            var templateType = typeof(Attribute<>);
            System.Type[] typeArgs = { attributeType };
            System.Type typedAttributeType = templateType.MakeGenericType(typeArgs);

            var attribute = AttributeBase.CreateInstance(typedAttributeType, attributeBase.FieldName, attributeType, TypeLoader.AllTypesNames[newTypeIndex]);

            attributeCollection[index] = attribute;

            serializedObject.Update();
            EditorUtility.SetDirty(serializedObject.targetObject);
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void AddNewAttribute(ReorderableList list)
    {
        var newAttribute = AttributeBase.CreateInstance(typeof(Attribute<int>), "MyInt", typeof(int), typeof(int).Name);
        attributeCollection.Add(newAttribute);
        serializedObject.Update();
        EditorUtility.SetDirty(serializedObject.targetObject);
        serializedObject.ApplyModifiedProperties();
    }

    private void RemoveAttribute(ReorderableList list)
    {
        attributeCollection.RemoveAt(list.index);
        serializedObject.Update();
        EditorUtility.SetDirty(serializedObject.targetObject);
        serializedObject.ApplyModifiedProperties();
    }
}
