using TMPro;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Diagnostics;

public class InventorySlotData : UI_PopUp
{
    enum Texts { I_Amount }
    enum Images { I_Icon }

    private InventoryItem inventoryItem;

    private void Awake()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Image>(typeof(Images));
    }

    public void Initialize(InventoryItem _item)
    {
        inventoryItem = _item;
        UpdateSlotUI();
    }

    private void UpdateSlotUI()
    {
        GetTMP((int)Texts.I_Amount).text = $"x {InventoryHandler.Instance.GetItemQuantity(inventoryItem.itemData.ItemType.ToString())}";
        GetImage((int)Images.I_Icon).sprite = inventoryItem.itemData.ItemIcon;
    }
}
