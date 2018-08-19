using System.Collections.Generic;
using UnityEngine;
using ItemSystem;

public class Inventory : MonoBehaviour
{
    /// <summary>Current inventory</summary>
    public static Inventory Inv { get; private set; }

    InfoPanel infoPanel;
    readonly InventorySlot[] slots = new InventorySlot[10];

    void Awake()
    {
        if (!Inv)
            Inv = this;
    }

    void Start()
    {
        infoPanel = transform.GetChild(1).GetComponent<InfoPanel>();
        HideInfoPanel(0);

        var comps = transform.GetChild(0).GetComponentsInChildren(typeof(InventorySlot), true);
        for (int i = 0; i < slots.Length; i++)
        {
            var s = (InventorySlot)comps[i];
            s.SetNumber(i);
            s.MouseEnterEvent += ShowInfoPanel;
            s.MouseClickEvent += MouseClick;
            s.MouseExitEvent += HideInfoPanel;
            slots[i] = s;
        }

        Inventory.Inv.PutItem(ItemSystemUtility.GetItemCopy<Item>((int)ItemItems.Pudding, ItemType.Item));
        Inventory.Inv.PutItem(ItemSystemUtility.GetItemCopy<Item>((int)ItemItems.GoodBread, ItemType.Item));
    }

    /// <summary>
    /// Returns whether the item could be added or not
    /// </summary>
    /// <param name="it"></param>
    /// <returns></returns>
    public bool PutItem(Item it)
    {
        foreach (var s in slots)
            if (s.GetItem() == null || s.HasItem(it.itemID))
                return s.PutItem(it);

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
            if (i.HasItem(id))
                return i.UseItem();

        return null;
    }

    public Item UseItem(int slotNum)
    {
        return slotNum >= slots.Length ? null : slots[slotNum].UseItem();
    }

    void CompactInv()
    {
        List<Item> items = new List<Item>(10);
        for (int i = 0; i < slots.Length; i++)
        {
            Item it = slots[i].GetItem();
            if (it == null)
                continue;

            items.Add(it);
            slots[i].RemoveItem();
        }

        for (int i = 0; i < slots.Length; i++)
            slots[i].PutItem(items[i]);
    }

    public void RemoveItem(ItemItems it)
    {
        int id = (int)it;
        foreach (var s in slots)
        {
            if (!s.HasItem(id))
                continue;

            s.RemoveItem();
            return;
        }
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
            if (i.HasItem(id))
                return true;

        return false;
    }

    void ShowInfoPanel(int slotNum)
    {
        var i = slots[slotNum].GetItem();
        if (i == null)
            return;

        infoPanel.SetInfo(i, slotNum);
        infoPanel.gameObject.SetActive(true);
    }

    void MouseClick(int slotNum)
    {
        Debug.Log($"Click Slot {slotNum}");
    }

    void HideInfoPanel(int slotNum)
    {
        infoPanel.gameObject.SetActive(false);
    }
}
