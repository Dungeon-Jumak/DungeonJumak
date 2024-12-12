using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryDescPopup : UI_PopUp
{
    enum Texts { ItemName, ItemDesc }
    enum Buttons { Dim }

    private InventoryItem inventoryItem;
    private void Awake()
    {
        InventorySlotTouchHandler.OnItemClicked += ItemInit;;
    }

    private void Start()
    {
        Init();
    }

    private void OnDisable()
    {
        InventorySlotTouchHandler.OnItemClicked -= ItemInit;;
    }

    public override void Init()
    {
        base.Init();
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Button>(typeof(Buttons));
        GetButton((int)Buttons.Dim).gameObject.BindEvent(ClosePopUp);
        UpdateUI();
    }

    public void ItemInit(InventoryItem _item)
    {
        inventoryItem = _item;
    }

    private void UpdateUI()
    {
        GetTMP((int)Texts.ItemName).text = inventoryItem.itemData.Name;
        GetTMP((int)Texts.ItemDesc).text = inventoryItem.itemData.Desc;
    }

    public void ClosePopUp(PointerEventData _data)
    {
        GameManager.UI.ClosePopUpUI();
    }
}
