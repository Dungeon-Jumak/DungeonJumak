using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryHandler : MonoBehaviour
{
    public static InventoryHandler Instance { get; private set; }

    private Dictionary<string, InventoryItem> inventoryItems = new ();
    public Dictionary<string, InventoryItem> InventoryItems => inventoryItems;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddItem(InventoryData _item, int _amount)
    {
        if (inventoryItems.ContainsKey(_item.ItemType.ToString()))
        {
            inventoryItems[_item.ItemType.ToString()].quantity += _amount;
        }
        else
        {
            InventoryItem newItem = new InventoryItem
            {
                itemData = _item,
                quantity = _amount
            };
            inventoryItems.Add(_item.ItemType.ToString(), newItem);
        }
    }

    //ex) InventoryHandler.Instance.UseItem("아이템이름", 수량);
    public void UseItem(string _itemName, int _amount)
    {
        if (inventoryItems.ContainsKey(_itemName) && inventoryItems[_itemName].quantity >= _amount)
        {
            InventoryItem item = inventoryItems[_itemName];
            item.quantity -= _amount;

            if (item.quantity <= 0)
            {
                inventoryItems.Remove(_itemName);
            }
        }
    }

    public bool HasItem(string _itemName, int _amount)
    {
        return inventoryItems.ContainsKey(_itemName) && inventoryItems[_itemName].quantity >= _amount;
    }

    public int GetItemQuantity(string itemName)
    {
        if (inventoryItems.ContainsKey(itemName))
        {
            return inventoryItems[itemName].quantity;
        }
        return 0;
    }

    public List<InventoryItem> GetSortedInventoryItems()
    {
        List<InventoryItem> sortedItems = new List<InventoryItem>(inventoryItems.Values);
        sortedItems.Sort((item1, item2) =>
        {
            int idComparison = item1.itemData.ID.CompareTo(item2.itemData.ID);
            if (idComparison == 0)
            {
                return item2.quantity.CompareTo(item1.quantity);
            }
            return idComparison;
        });
        return sortedItems;
    }

}

[Serializable]
public class InventoryItem
{
    public InventoryData itemData; 
    public int quantity;
}
