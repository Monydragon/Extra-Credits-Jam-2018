using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using ItemSystem;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerExitHandler
{
    public delegate void MouseEnter(int slotNum);
    public delegate void MouseClick(int slotNum, PointerEventData.InputButton mouseBtn);
    public delegate void MouseExit(int slotNum);
    public delegate void CompactInv();

    public event MouseEnter MouseEnterEvent;
    public event MouseClick MouseClickEvent;
    public event MouseExit MouseExitEvent;
    public event CompactInv CompactInvEvent;

    int num;
    Item item;
    Image itemImg;
    TextMeshProUGUI countTxt;

    void Awake()
    {
        itemImg = transform.GetChild(0).GetComponent<Image>();
        countTxt = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
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
            itemImg.sprite = i.itemIcon;
            UpdateCount();
            return true;
        }

        else if (item.itemID == i.itemID && item.stackable && item.stackCount < item.maxStackAmount)
        {
            item.stackCount = Mathf.Clamp(item.stackCount + i.stackCount, 0, item.maxStackAmount);
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

        UpdateCount();
        return i;
    }

    public void RemoveItem(bool compact = true)
    {
        item = null;
        itemImg.sprite = null;
        UpdateCount();

        if (compact)
            CompactInvEvent.Invoke();
    }

    public void UpdateCount()
    {
        countTxt.text = item == null ? "" : item.stackCount.ToString();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        MouseEnterEvent.Invoke(num);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        MouseClickEvent.Invoke(num, eventData.button);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        MouseExitEvent.Invoke(num);
    }
}
