using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public static class UnitOptions
{
    private static string s_DefaultAvailableAssemblies = "mscorlib;UnityEngine;UnityEngine.%;Assembly-CSharp;Assembly-CSharp-firstpass;";
    private static string s_DefaultAvailableTypes = "System.Int32;System.Single;System.Boolean;System.String;UnityEngine.Vector2;UnityEngine.Vector3;" +
        "UnityEngine.Vector4;UnityEngine.Quaternion;UnityEngine.GameObject;UnityEngine.Transform;UnityEngine.AI.NavMeshAgent;UnityEngine.Physics;" +
        "UnityEngine.Physics2D;UnityEngine.Rect;UnityEngine.Matrix4x4;UnityEngine.Bounds;UnityEngine.Sprite;UnityEngine.Color;UnityEngine.LayerMask;" +
        "UnityEngine.RaycastHit;UnityEngine.RaycastHit2D;UnityEngine.AnimationCurve;UnityEngine.Mathf;UnityEngine.Input;UnityEngine.Random;" +
        "UnityEngine.Time;UnityEngine.AI.NavMeshAgent;UnityEngine.Object;";

    private static string s_TypeNameReplacements = "System.Int32,Int;System.Single,Float;System.Boolean,Bool";

    private static Assembly[] s_AllAssemblies;
    private static Assembly[] s_AvailableAssemblies;
    private static Assembly[] s_IgnoredAssemblies;

    private static Type[] s_AllTypes;
    private static Type[] s_AllEditorTypes;
    private static Type[] s_AvailableTypes;
    //private static Type[] s_FavoriteTypes;
    private static Dictionary<Type, string> s_TypeByName = new Dictionary<Type, string>();

    private static HashSet<string> s_AvailableTypeNames = new HashSet<string>();
    private static HashSet<string> s_AvailableAssemblyNames = new HashSet<string>();
    private static List<string> s_WildcardAssemblyNames = new List<string>();
    private static HashSet<string> s_IgnoredAssemblyNames = new HashSet<string>();

    //private static string[] typeNamesFixed;
    public static HashSet<string> AvailableTypeNames => s_AvailableTypeNames;
    //{ 
    //    get
    //    {
    //        if(typeNamesFixed == null)
    //        {
    //            var typeArray = s_AllTypes;
    //            typeNamesFixed = new string[typeArray.Length];
    //            for (int i = 0; i < typeArray.Length; i++)
    //            {
    //                Type type = typeArray[i];
    //                typeNamesFixed[i] = GetName(type);
    //            }
    //        }
    //        return typeNamesFixed;
    //    }
    //}

    [InitializeOnLoadMethod]
    public static void LoadTypes()
    {
        //string @string = EditorPrefs.GetString(UnitOptions.s_AvailableAssembliesKey, UnitOptions.s_DefaultAvailableAssemblies);
        string[] array2 = s_DefaultAvailableAssemblies.Split(new char[]
                {
                    ';'
                });
        for (int j = 0; j < array2.Length; j++)
        {
            if (!string.IsNullOrEmpty(array2[j]))
            {
                if (array2[j].EndsWith("%"))
                {
                    string text = array2[j].Substring(0, array2[j].Length - 1);
                    s_WildcardAssemblyNames.Add(text);
                    if (array2[j].EndsWith("."))
                    {
                        s_AvailableAssemblyNames.Add(text.Substring(0, text.Length - 1));
                    }
                }
                else
                {
                    s_AvailableAssemblyNames.Add(array2[j]);
                }
            }
        }

        string string2 = string.Empty;
        if (!string.IsNullOrEmpty(string2))
        {
            string[] array3 = string2.Split(new char[]
            {
                    ';'
            });
            for (int k = 0; k < array3.Length; k++)
            {
                if (!string.IsNullOrEmpty(array3[k]))
                {
                    UnitOptions.s_IgnoredAssemblyNames.Add(array3[k]);
                }
            }
        }

        Dictionary<string, string> nameFixMap = new Dictionary<string, string>();
        string[] nameFixTokens = UnitOptions.s_TypeNameReplacements.Split(new char[] { ';' });
        for (int num7 = 0; num7 < nameFixTokens.Length; num7++)
        {
            string[] array8 = nameFixTokens[num7].Split(new char[] { ',' });
            if (array8.Length == 2)
            {
                nameFixMap.Add(array8[0], array8[1]);
            }
        }

        string[] array4 = s_DefaultAvailableTypes.Split(new char[] { ';' });
        for (int l = 0; l < array4.Length; l++)
        {
            if (!string.IsNullOrEmpty(array4[l])) 
            {
                if (TypeUtility.GetType(array4[l]) == null)
                {
                    //generateStubs = true;
                    //UnitOptions.RemoveObjectPreference(array4[l], UnitOptions.s_AvailableTypesKey, UnitOptions.s_DefaultAvailableTypes);
                }
                else
                {
                    if(nameFixMap.TryGetValue(array4[l], out string nameFix))
                    {
                        array4[l] = nameFix;
                    }
                    s_AvailableTypeNames.Add(array4[l]); 
                }
            }
        }
        //if (s_AdditionalUnitOptions != null)
        //{
        //    for (int m = 0; m < s_AdditionalUnitOptions.Length; m++)
        //    {
        //        for (int n = 0; n < UnitOptions.s_AdditionalUnitOptions[m].AvailableTypes.Length; n++)
        //        {
        //            if (!string.IsNullOrEmpty(UnitOptions.s_AdditionalUnitOptions[m].AvailableTypes[n]))
        //            {
        //                UnitOptions.s_AvailableTypeNames.Add(UnitOptions.s_AdditionalUnitOptions[m].AvailableTypes[n]);
        //            }
        //        }
        //    }
        //}
        string value = Application.dataPath.Remove(Application.dataPath.Length - 6, 6).Replace('\\', '/');
        Assembly[] assemblies = System.AppDomain.CurrentDomain.GetAssemblies();
        if (assemblies == null)
        {
            return;
        }
        string[] array6 = "Editor;Unity.Entities;Unity.Transforms;Unity.Jobs".Split(new char[]
        {
                ';'
        });

        var s_IgnoredWords = new string[array6.Length];
        for (int num4 = 0; num4 < s_IgnoredWords.Length; num4++)
        {
            s_IgnoredWords[num4] = array6[num4];
        }
        List<Assembly> list2 = new List<Assembly>();
        List<Assembly> list3 = new List<Assembly>();
        List<Assembly> list4 = new List<Assembly>();
        List<Type> list5 = new List<Type>();
        List<Type> list6 = new List<Type>();
        List<Type> list7 = new List<Type>();
        //List<Type> list8 = new List<Type>();
        foreach (Assembly assembly in assemblies)
        {
            list2.Add(assembly);
            string name = assembly.GetName().Name;
            //if (!UnitOptions.IsNameValid(name)) //This is for ignoredWords
            //{
            //    UnitOptions.CacheTypes(assembly, true, ref list5, ref list6, ref list7, ref list8);
            //}
            //else
            {
                bool flag = UnitOptions.s_AvailableAssemblyNames.Contains(name);
                if (!flag)
                {
                    bool flag2 = UnitOptions.s_IgnoredAssemblyNames.Contains(name);
                    if (flag2)
                    {
                        list4.Add(assembly);
                    }
                    else
                    {
                        for (int num6 = 0; num6 < UnitOptions.s_WildcardAssemblyNames.Count; num6++)
                        {
                            if (name.StartsWith(UnitOptions.s_WildcardAssemblyNames[num6]))
                            {
                                flag = true;
                                break;
                            }
                        }
                        if (!flag && !assembly.IsDynamic)
                        {
                            flag = assembly.Location.Replace('\\', '/').Contains(value);
                        }
                    }
                }
                if (flag)
                {
                    list3.Add(assembly);
                    UnitOptions.CacheTypes(assembly, false, ref list5, ref list6, ref list7/*, ref list8*/);
                }
            }
        }
        UnitOptions.s_AllAssemblies = list2.ToArray();
        UnitOptions.s_AvailableAssemblies = list3.ToArray();
        UnitOptions.s_IgnoredAssemblies = list4.ToArray();
        UnitOptions.s_AllTypes = list5.ToArray();
        UnitOptions.s_AllEditorTypes = list6.ToArray();
        UnitOptions.s_AvailableTypes = list7.ToArray();
        //UnitOptions.s_FavoriteTypes = list8.ToArray();
        //UnitOptions.CacheAvailableMethods(generateStubs);
        if (UnitOptions.s_TypeByName.Count == 0)
        {
            string[] array7 = UnitOptions.s_TypeNameReplacements.Split(new char[]
            {
                    ';'
            });
            for (int num7 = 0; num7 < array7.Length; num7++)
            {
                string[] array8 = array7[num7].Split(new char[]
                {
                        ','
                });
                if (array8.Length == 2)
                {
                    UnitOptions.s_TypeByName.Add(Type.GetType(array8[0]), array8[1]);
                }
            }
        }
    }

    private static void CacheTypes(Assembly assembly, bool editorAssembly, ref List<Type> allTypesList, ref List<Type> allEditorTypesList, ref List<Type> availableTypesList/*, ref List<Type> favoriteTypesList*/)
    {
        foreach (Type type in assembly.GetTypes())
        {
            if (!type.IsAbstract && !type.IsNotPublic && !type.IsGenericType && type.MemberType == MemberTypes.TypeInfo && type.GetCustomAttribute<ObsoleteAttribute>() == null)
            {
                allEditorTypesList.Add(type);
                if (!editorAssembly && (string.IsNullOrEmpty(type.Namespace) /*|| UnitOptions.IsNameValid(type.Namespace)*/))
                {
                    allTypesList.Add(type);
                    if (typeof(System.Object).IsAssignableFrom(type) || UnitOptions.s_AvailableTypeNames.Contains(type.FullName))
                    {
                        availableTypesList.Add(type);
                        //if (UnitOptions.s_FavoriteTypeNames.Contains(type.FullName))
                        //{
                        //    favoriteTypesList.Add(type);
                        //}
                    }
                }
            }
        }
    }

    public static string GetName(Type type, bool fullName = false)
    {
        string text;
        if (UnitOptions.s_TypeByName.TryGetValue(type, out text))
        {
            if (fullName)
            {
                string[] array = type.FullName.Split(new char[]
                {
                        '.'
                });
                return type.FullName.Replace(array[array.Length - 1], text);
            }
            return text;
        }
        else
        {
            if (fullName)
            {
                return type.FullName;
            }
            return type.Name;
        }
    }
}
