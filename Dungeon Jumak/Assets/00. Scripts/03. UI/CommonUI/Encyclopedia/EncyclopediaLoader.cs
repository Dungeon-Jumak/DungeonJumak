using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class EncyclopediaLoader : MonoBehaviour
{
    enum Category { Monster, Customer }

    private DataManager<EncyclopediaData> g_EncyclopediaData;
    private Category currentCategory = Category.Customer;

    private void Awake()
    {
        g_EncyclopediaData = DataManager<EncyclopediaData>.Instance;
        BaseEncyclopedia.OnCategoryChanged += OnCategoryChanged;
    }

    private void Start()
    {
        LoadUnlockData();
    }

    private void OnCategoryChanged(BaseEncyclopedia.Category _newCategory)
    {
        currentCategory = (Category)System.Enum.Parse(typeof(Category), _newCategory.ToString());
        LoadUnlockData();
    }

    private void LoadUnlockData()
    {
        switch (currentCategory)
        {
            case Category.Monster:
                LoadCategoryData("Encyclopedia_M", g_EncyclopediaData.Data.Monsters);
                break;
            case Category.Customer:
                LoadCategoryData("Encyclopedia_C", g_EncyclopediaData.Data.Customers);
                break;
        }
    }

    private void LoadCategoryData(string _addressableKey, Dictionary<string, bool> _categoryData)
    {
        foreach (var entry in _categoryData)
        {
            if (entry.Value) 
            {
                LoadAssetAsync(_addressableKey, entry.Key);
            }
        }
    }

    private void LoadAssetAsync(string _addressableKey, string _entryName)
    {
        Addressables.LoadAssetsAsync<EncyclopediaSO>(_addressableKey, null).Completed += handle =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                var encyclopediaEntries = handle.Result;

                foreach (var entry in encyclopediaEntries)
                {
                    if (entry.name == _entryName)  
                    {
                        UpdateSlot(entry); 
                        break;
                    }
                }
            }
        };
    }

    private void UpdateSlot(EncyclopediaSO _entry)
    {
        var contentTransform = FindObjectOfType<BaseEncyclopedia>()?.Content;

        if (contentTransform != null)
        {
            foreach (Transform slot in contentTransform)
            {
                var slotData = slot.GetComponent<EncyclopediaSlotData>();
                if (slotData != null && slotData.GetEntry() == _entry)
                {
                    slotData.UpdateSlot(_entry.EntryName, _entry.Icon); 
                    break;
                }
            }
        }
    }

}