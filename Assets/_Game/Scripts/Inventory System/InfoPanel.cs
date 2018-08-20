using UnityEngine;
using TMPro;
using ItemSystem;

public class InfoPanel : MonoBehaviour
{
    public int slotSeparation, slotSize;

    RectTransform rect;
    TextMeshProUGUI nameTxt, descTxt, infoTxt;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        nameTxt = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        descTxt = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        infoTxt = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
    }

    public void SetInfo(Item item, int slotNum)
    {
        nameTxt.text = item.itemName;
        descTxt.text = item.itemDescription;

        string stackMsg = item.stackable ? $"{item.stackCount}/{item.maxStackAmount}" : "Does not stack";
        //infoTxt.text = $"Healing:\t{item.healing}\nHunger:\t{item.hunger}\nRadiation:\t{item.radiation}\nAmount:\t{stackMsg}";
        infoTxt.text = $"Healing:\t{item.healing}\nRadiation:\t{item.radiation}\nAmount:\t{stackMsg}";

        rect.anchoredPosition = new Vector2((slotSize / 2f) + (slotSize + slotSeparation) * slotNum, rect.anchoredPosition.y);
    }
}
