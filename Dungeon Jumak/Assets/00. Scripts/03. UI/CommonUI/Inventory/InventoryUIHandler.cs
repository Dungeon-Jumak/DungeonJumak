using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryUIHandler : UI_PopUp
{
    enum Buttons { Dim }

    public Transform itemSlotParent;
    public GameObject itemSlotPrefab;

    private void OnEnable()
    {
        UpdateInventoryUI();
    }
    private void Start()
    {
        Init();
    }

    public override void Init()
    {

        base.Init();

        Bind<Button>(typeof(Buttons));
        GetButton((int)Buttons.Dim).gameObject.BindEvent(ClosePopUp);
    }

    private void UpdateInventoryUI()
    {
        foreach (Transform child in itemSlotParent)
        {
            Destroy(child.gameObject);
        }

        List<InventoryItem> sortedItems = InventoryHandler.Instance.GetSortedInventoryItems();

        foreach (var item in sortedItems)
        {
            int remainingQuantity = item.quantity;
            int splitCount = Mathf.CeilToInt(remainingQuantity / 99f);

            for (int i = 0; i < splitCount; i++)
            {
                int quantityToDisplay = Mathf.Min(99, remainingQuantity);
                remainingQuantity -= quantityToDisplay;

                GameObject itemSlot = Instantiate(itemSlotPrefab, itemSlotParent);

                InventorySlotTouchHandler slotTouch = itemSlot.GetComponentInChildren<InventorySlotTouchHandler>();
                slotTouch.Initialize(item);

                itemSlot.GetComponentInChildren<Image>().sprite = item.itemData.ItemIcon;
                itemSlot.GetComponentInChildren<TextMeshProUGUI>().text = $"x{quantityToDisplay}";

                var slotCanvas = itemSlot.GetComponent<Canvas>();
                if (slotCanvas != null)
                {
                    slotCanvas.overrideSorting = false;
                }
            }
        }

        UpdateCanvasHeight();
    }

    public void UpdateCanvasHeight()
    {
        GridLayoutGroup gridLayout = itemSlotParent.GetComponent<GridLayoutGroup>();
        if (gridLayout != null)
        {
            float cellHeight = gridLayout.cellSize.y;
            float paddingVertical = gridLayout.padding.top + gridLayout.padding.bottom;
            float spacingVertical = gridLayout.spacing.y;

            int totalItems = InventoryHandler.Instance.InventoryItems.Count;
            int rows = Mathf.CeilToInt(totalItems / (float)gridLayout.constraintCount); 

            RectTransform contentRect = itemSlotParent.GetComponent<RectTransform>();
            if (contentRect != null)
            {
                float contentHeight = (cellHeight + spacingVertical) * rows - spacingVertical + paddingVertical;
                contentRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, contentHeight);
            }
        }
    }

    public void ClosePopUp(PointerEventData _data)
    {
        GameManager.UI.ClosePopUpUI();
    }
}
