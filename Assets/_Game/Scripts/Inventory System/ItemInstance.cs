using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemSystem;

public class ItemInstance : MonoBehaviour
{
    public ItemItems itemToMake;
    [HideInInspector]
    public Item item;

    public static GameObject CreateItemInstance(ItemItems i)
    {
        var go = new GameObject(i.ToString());
        go.SetActive(false);
        go.layer = 8;
        go.AddComponent<Mono_BaseItemObject>();
        go.AddComponent<ItemInstance>().itemToMake = i;
        go.AddComponent<SpriteRenderer>().sortingOrder = -1;
        go.SetActive(true);

        return go;
    }

    public static GameObject CreateItemInstance(ItemItems i, Vector3 pos)
    {
        var go = CreateItemInstance(i);
        go.transform.position = pos;
        return go;
    }

    void Start()
    {
        SetItem();
    }

    void Update()
    {

    }

    public void SetItem()
    {
        if (itemToMake == ItemItems.None)
            return;

        item = ItemSystemUtility.GetItemCopy<Item>((int)itemToMake, ItemType.Item);
        gameObject.name = item.itemName;
        GetComponent<SpriteRenderer>().sprite = item.itemSprite;
        DestroyImmediate(gameObject.GetComponent<BoxCollider2D>());
        gameObject.AddComponent<BoxCollider2D>().isTrigger = true;

        transform.position = new Vector3(Mathf.Floor(transform.position.x) + 0.5f, Mathf.Floor(transform.position.y) + 0.5f, 0);
    }
}