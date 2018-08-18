using UnityEngine;
using ItemSystem;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    Item item;
    Image itemImg;

    void Start()
    {
        itemImg = transform.GetComponentInChildren<Image>(true);
    }

    /// <summary>
    /// Puts the item if there is no item or if the item already exists and can be stacked. Returns whether the item was added or not
    /// </summary>
    /// <param name="i"></param>
    public bool PutItem(Item i)
    {
        if (item == null)
        {
            item = i;
            itemImg.sprite = i?.itemIcon;
            return true;
        }

        else if (item.itemID == i.itemID && item.stackable && item.stackCount < item.maxStackAmount)
        {
            Mathf.Clamp(item.stackCount + i.stackCount, 0, item.maxStackAmount);
            return true;
        }

        return false;
    }

    public Item GetItem()
    {
        return item;
    }

    /// <summary>
    /// Returns the item, decrements the stackCount and removes the item if the stackCount reaches zero.
    /// </summary>
    /// <returns></returns>
    public Item UseItem()
    {
        Item i = item;

        item.stackCount--;
        if (item.stackCount <= 0)
            RemoveItem();

        return i;
    }

    public void RemoveItem()
    {
        item = null;
        itemImg.sprite = null;
    }
}
