using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class ItemActionUtility
{
    public static List<ItemActionValueField> GetAllActionValues(ItemAction[] itemActions)
    {
        List<ItemActionValueField> allActionValues = new List<ItemActionValueField>();
        foreach(var itemAction in itemActions)
        {
            if (itemAction == null)
            {
                continue;
            }
            foreach(var valueField in itemAction.ActionKeyValues)
            {
                if(valueField == null)
                {
                    continue;
                }
                allActionValues.Add(valueField);
            }
        }

        var filteredList = allActionValues
            .GroupBy(x => x.FieldName)
            .Select(y => y.First())
            .ToList();

        return filteredList;
    }
}
