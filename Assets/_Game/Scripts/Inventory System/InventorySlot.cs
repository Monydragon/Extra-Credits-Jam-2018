using UnityEngine;
using ItemSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerExitHandler
{
    public delegate void MouseEnter(int slotNum);
    public delegate void MouseClick(int slotNum);
    public delegate void MouseExit(int slotNum);

    public event MouseEnter MouseEnterEvent;
    public event MouseClick MouseClickEvent;
    public event MouseExit MouseExitEvent;

    int num;
    Item item;
    Image itemImg;

    void Awake()
    {
        itemImg = transform.GetChild(0).GetComponent<Image>();
    }

    public void SetNumber(int slotNum)
    {
        num = slotNum;
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

    public bool HasItem(int id)
    {
        return item != null && item.itemID == id;
    }

    /// <summary>
    /// Returns the item, decrements the stackCount and removes the item if the stackCount reaches zero.
    /// </summary>
    /// <returns></returns>
    public Item UseItem()
    {
        if (item == null)
            return null;

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

    public void OnPointerEnter(PointerEventData eventData)
    {
        MouseEnterEvent.Invoke(num);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        MouseClickEvent.Invoke(num);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        MouseExitEvent.Invoke(num);
    }
}
