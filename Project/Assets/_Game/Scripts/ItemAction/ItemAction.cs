using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemAction")]
public class ItemAction : ScriptableObject
{
    //public bool ConsumeOnUse = false;

    //public abstract bool CanUse { get; }

    //public abstract void OnUse(ItemData itemData, PlayerData characterData);

    //public virtual void OnHotbarSelected() { }
    //public virtual void OnHotbarDeselected() { }

    public List<ItemActionValueField> ActionKeyValues;
}
