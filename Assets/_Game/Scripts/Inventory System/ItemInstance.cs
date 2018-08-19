using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemSystem;

public class ItemInstance : MonoBehaviour
{
    public ItemItems itemToMake;
    [HideInInspector]
    public Item item;
    bool canPick;

    public static GameObject CreateItemInstance(ItemItems i)
    {
        var go = new GameObject(i.ToString());
        go.SetActive(false);
        go.layer = 8;
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

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            canPick = true;
    }

    void Update()
    {
        if (!canPick)
            return;

        if (Input.GetKeyDown(KeyCode.E) && Inventory.Inv.PutItem(item))
            Destroy(gameObject);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
            canPick = false;
    }
}