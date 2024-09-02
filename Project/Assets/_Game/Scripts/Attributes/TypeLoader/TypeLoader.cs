using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class TypeLoader
{
    private static string s_DefaultAvailableAssemblies = "Assembly-CSharp;Assembly-CSharp-firstpass;";

    private static Type[] defaultTypes = new Type[] {typeof(int), typeof(float), typeof(bool), typeof(string), typeof(Vector2), typeof(Vector3),
        typeof(Vector4), typeof(GameObject), typeof(Transform), typeof(UnityEngine.AI.NavMeshAgent), typeof(Sprite), typeof(Color),
        typeof(LayerMask), typeof(AnimationCurve), typeof(Material), typeof(UnityEngine.Object)
    };

    private static Type[] allTypes;
    public static Type[] AllTypes => allTypes;

    private static string[] allTypesNames;
    public static string[] AllTypesNames => allTypesNames;

    private static HashSet<string> s_AvailableAssemblyNames = new HashSet<string>();

    [InitializeOnLoadMethod]
    public static void LoadTypes()
    {
        List<Type> types = new List<Type>();

        types.AddRange(defaultTypes);

        string[] defaultAvailableAssemblies = s_DefaultAvailableAssemblies.Split(new char[]
                {
                    ';'
                }, StringSplitOptions.RemoveEmptyEntries);

        for (int j = 0; j < defaultAvailableAssemblies.Length; j++)
        {
            if (!string.IsNullOrEmpty(defaultAvailableAssemblies[j]))
            {
                if (defaultAvailableAssemblies[j].EndsWith("%"))
                {
                    string wildcardAssemblyName = defaultAvailableAssemblies[j].Substring(0, defaultAvailableAssemblies[j].Length - 1);
                    if (defaultAvailableAssemblies[j].EndsWith("."))
                    {
                        s_AvailableAssemblyNames.Add(wildcardAssemblyName.Substring(0, wildcardAssemblyName.Length - 1));
                    }
                }
                else
                {
                    s_AvailableAssemblyNames.Add(defaultAvailableAssemblies[j]);
                }
            }
        }

        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
        if (assemblies == null)
        {
            return;
        }

        List<Assembly> allAssemblies = new List<Assembly>();
        List<Assembly> availableAssemblies = new List<Assembly>();
        for (int i = 0; i < assemblies.Length; i++)
        {
            Assembly assembly = assemblies[i];
            allAssemblies.Add(assembly);
            string assemblyName = assembly.GetName().Name;
            bool shouldCache = TypeLoader.s_AvailableAssemblyNames.Contains(assemblyName);
            if (shouldCache)
            {
                availableAssemblies.Add(assembly);
                TypeLoader.CacheTypes(assembly, ref types);
            }
        }

        allTypes = types.ToArray();

        allTypesNames = new string[allTypes.Length];
        for (int i = 0; i < allTypes.Length; i++)
        {
            string typeName = allTypes[i].Name;
            allTypesNames[i] = typeName;
        }
    }

    private static void CacheTypes(Assembly assembly, ref List<Type> allTypesList)
    {
        var types = assembly.GetTypes();
        foreach (Type type in types)
        {
            if (!type.IsAbstract && !type.IsNotPublic && !type.IsGenericType && type.MemberType == MemberTypes.TypeInfo && type.GetCustomAttribute<ObsoleteAttribute>() == null)
            {
                if (typeof(UnityEngine.Object).IsAssignableFrom(type))
                {
                    allTypesList.Add(type);
                }
            }
        }
    }
}
