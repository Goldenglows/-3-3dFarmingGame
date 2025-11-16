using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerClickHandler
{
    [SerializeField] private ItemData itemToDisplay;
    [SerializeField] private Image itemDisplayImage;

    public enum InventoryType
    {
        Item, Tool
    }
    public InventoryType inventoryType;

    int slotIndex; 

    public void Display(ItemData itemToDisplay)
    {
        if (itemDisplayImage == null)
        {
            Debug.LogError($"itemDisplayImage 未赋值！槽位: {gameObject.name}", gameObject);
            return;
        }
        Debug.Log($"槽位: {gameObject.name}, 物品: {(itemToDisplay != null ? itemToDisplay.name : "null")}, Thumbnail: {(itemToDisplay?.thumbnail != null ? itemToDisplay.thumbnail.name : "null")}");
        if (itemToDisplay != null && itemToDisplay.thumbnail != null)
        {
            itemDisplayImage.sprite = itemToDisplay.thumbnail;
            itemDisplayImage.color = new Color(1, 1, 1, 1); // 确保不透明
            this.itemToDisplay = itemToDisplay;
            if (!itemDisplayImage.gameObject.activeSelf)
                itemDisplayImage.gameObject.SetActive(true);
            return;
        }
        if (itemDisplayImage.gameObject.activeSelf)
            itemDisplayImage.gameObject.SetActive(false);

    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {

        InventoryManager.Instance.InventoryToHand(slotIndex,inventoryType);

    }

    public void AssignIndex(int slotIndex)
    {
        this.slotIndex = slotIndex;

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UiManager.Instance.DisplayItemInfo(itemToDisplay);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UiManager.Instance.DisplayItemInfo(null);
    }

}
