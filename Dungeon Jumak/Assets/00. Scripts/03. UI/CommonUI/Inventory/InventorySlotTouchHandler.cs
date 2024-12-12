using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlotTouchHandler : MonoBehaviour, IPointerClickHandler
{
    private InventoryItem inventoryItem;
    public static event System.Action<InventoryItem> OnItemClicked;

    public void Initialize(InventoryItem _item)
    {
        inventoryItem = _item;
    }

    public void OnPointerClick(PointerEventData _eventData)
    {
        GameManager.UI.ShowPopupUI<UI_PopUp>("InventoryDesc_PopUp");
        OnItemClicked?.Invoke(inventoryItem);
    }
}
