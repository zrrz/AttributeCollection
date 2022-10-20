using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

public class TypeUtility
{
    private static Dictionary<string, Type> s_TypeLookup = new Dictionary<string, Type>();

    // Token: 0x0400001E RID: 30
    private static List<Assembly> s_LoadedAssemblies = null;

    public static Type GetType(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return null;
        }
        Type type;
        if (TypeUtility.s_TypeLookup.TryGetValue(name, out type))
        {
            return type;
        }
        type = Type.GetType(name);
        if (type == null)
        {
            if (TypeUtility.s_LoadedAssemblies == null || TypeUtility.s_LoadedAssemblies.Count == 0)
            {
                TypeUtility.s_LoadedAssemblies = new List<Assembly>();
                Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
                for (int i = 0; i < assemblies.Length; i++)
                {
                    TypeUtility.s_LoadedAssemblies.Add(assemblies[i]);
                }
            }
            for (int j = 0; j < TypeUtility.s_LoadedAssemblies.Count; j++)
            {
                type = TypeUtility.s_LoadedAssemblies[j].GetType(name);
                if (type != null)
                {
                    break;
                }
            }
        }
        if (type != null)
        {
            TypeUtility.s_TypeLookup.Add(name, type);
        }
        return type;
    }
}
