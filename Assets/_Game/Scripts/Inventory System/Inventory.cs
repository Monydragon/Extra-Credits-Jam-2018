using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemSystem;

public class Inventory : MonoBehaviour
{
    /// <summary>Current inventory</summary>
    public static Inventory Inv { get; private set; }
    readonly InventorySlot[] slots = new InventorySlot[20];

    void Awake()
    {
        if (!Inv)
            Inv = this;
    }

    void Start()
    {
        var comps = transform.GetChild(0).GetComponentsInChildren(typeof(InventorySlot), true);
        for (int i = 0; i < slots.Length; i++)
            slots[i] = (InventorySlot)comps[i];
    }

    /// <summary>
    /// Returns whether the item could be added or not
    /// </summary>
    /// <param name="it"></param>
    /// <returns></returns>
    public bool PutItem(Item it)
    {
        foreach (var s in slots)
        {
            var i = s.GetItem();
            if (i == null || i.itemID == it.itemID)
                if (s.PutItem(it))
                    return true;
        }

        return false;
    }

    /// <summary>
    /// Returns the item and decrements the item stackCount. Returns null if the item doesn't exist
    /// </summary>
    /// <param name="it"></param>
    /// <returns></returns>
    public Item UseItem(ItemItems it)
    {
        int id = (int)it;
        foreach (var i in slots)
            if (i.GetItem().itemID == id)
                return i.UseItem();

        return null;
    }

    void CompactInv()
    {
        List<Item> items = new List<Item>(10);
        for (int i = 0; i < slots.Length; i++)
        {
            items.Add(slots[i].GetItem());
            slots[i].RemoveItem();
        }

        for (int i = 0; i < slots.Length; i++)
            slots[i].PutItem(items[i]);
    }

    /// <summary>
    /// Returns whether an instance of the passed item exists in the inventory
    /// </summary>
    /// <param name="it"></param>
    /// <returns></returns>
    public bool HasItem(ItemItems it)
    {
        int id = (int)it;
        foreach (var i in slots)
            if (i.GetItem().itemID == id)
                return true;

        return false;
    }
}
