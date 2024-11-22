using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEncl : MonoBehaviour
{
    private DataManager<EncyclopediaData> g_EData;
    [SerializeField] private InventoryData[] inventoryData;

    private void Awake()
    {
        g_EData = DataManager<EncyclopediaData>.Instance;
    }
    public void OnClickBtn()
    {
        g_EData.Data.Monsters["0"] = true;
    }
    public void OnClickBtn1()
    {
        g_EData.Data.Customers["11"] = true;
        for (int i = 0; i < inventoryData.Length; i++) 
        {
            InventoryHandler.Instance.AddItem(inventoryData[i], 103);
            
        }
    }
}
