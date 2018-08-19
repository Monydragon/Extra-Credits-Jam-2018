using System.Collections.Generic;
using UnityEngine;
using ItemSystem;
using UnityEngine.EventSystems;

public class Inventory : MonoBehaviour
{
    /// <summary>Current inventory</summary>
    public static Inventory Inv { get; private set; }


    [SerializeField]
    Transform player;
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
            s.CompactInvEvent += CompactInv;
            slots[i] = s;
        }
        Test();
    }

    void Test()
    {
        var it = ItemSystemUtility.GetItemCopy<Item>((int)ItemItems.Pudding, ItemType.Item);
        it.stackCount = 4;
        Inventory.Inv.PutItem(it);

        it = ItemSystemUtility.GetItemCopy<Item>((int)ItemItems.GoodBread, ItemType.Item);
        it.stackCount = 5;
        Inventory.Inv.PutItem(it);

        it = ItemSystemUtility.GetItemCopy<Item>((int)ItemItems.Carrot, ItemType.Item);
        Inventory.Inv.PutItem(it);

        it = ItemSystemUtility.GetItemCopy<Item>((int)ItemItems.Water, ItemType.Item);
        Inventory.Inv.PutItem(it);
        StartCoroutine(T());
    }

    System.Collections.IEnumerator T()
    {
        yield return new WaitForSeconds(2);
        Inventory.Inv.UseItem(1);
        Inventory.Inv.UseItem(2);
        yield return null;
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
        //Get all items from the slots and empty the slots
        List<Item> items = new List<Item>(10);
        for (int i = 0; i < slots.Length; i++)
        {
            Item it = slots[i].GetItem();
            if (it == null)
                continue;

            items.Add(it);
            slots[i].RemoveItem(false);
        }

        //Fill them in order
        for (int i = 0; i < items.Count; i++)
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

    void MouseClick(int slotNum, PointerEventData.InputButton mouseBtn)
    {
<<<<<<< HEAD
        if (slots[slotNum].GetItem() == null || mouseBtn != PointerEventData.InputButton.Right)
            return;

        ItemInstance.CreateItemInstance((ItemItems)slots[slotNum].UseItem().itemID, player.position);
        ShowInfoPanel(slotNum);
=======
        Debug.Log($"Click Slot {slotNum}");
>>>>>>> 1f4e5aac2825318dad95e3494971053c28ddbe52
    }

    void HideInfoPanel(int slotNum)
    {
        infoPanel.gameObject.SetActive(false);
    }
}
