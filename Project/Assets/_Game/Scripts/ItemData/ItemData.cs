using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "ItemData", menuName = "ItemData")]
public class ItemData : ScriptableObject
{
    [SerializeField] private ItemAction[] itemActions;
    [SerializeField] private AttributeCollection attributes;

    public AttributeCollection Attributes {
        get
        {
            if(attributes == null)
            {
                attributes = new AttributeCollection();
            }
            return attributes;
        }
    }

    private void OnValidate()
    {
        //TODO move this to OnItemActionsCount changed event
        var actionValueFields = ItemActionUtility.GetAllActionValues(itemActions);
        attributes.UpdateAttributesToMatchList(actionValueFields, true);
    }
}
