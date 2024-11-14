using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System;
using UnityEngine.EventSystems;

public class BaseEncyclopedia : UI_PopUp
{
    enum Buttons { monsterBtn, customerBtn }
    public enum Category { Monster, Customer }

    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private Transform content;


    public Category currentCategory = Category.Customer;
    public static event Action<Category> OnCategoryChanged;

    public Transform Content => content;

    private void Awake()
    {
        currentCategory = Category.Customer;
        Init();
        LoadEntries();
    }

    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));

        GetButton((int)Buttons.monsterBtn).gameObject.BindEvent(SetMonster);
        GetButton((int)Buttons.customerBtn).gameObject.BindEvent(SetCustomer);
    }

    private void SetCustomer(PointerEventData _data)
    {
        SwitchCategory(Category.Customer);
    }

    private void SetMonster(PointerEventData _data)
    {
        SwitchCategory(Category.Monster);
    }

    private void SwitchCategory(Category _newCategory)
    {
        if (currentCategory != _newCategory)
        {
            currentCategory = _newCategory;
            LoadEntries();
            OnCategoryChanged?.Invoke(currentCategory);
        }
    }

    private void LoadEntries()
    {
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }

        string label = currentCategory == Category.Monster ? "Encyclopedia_M" : "Encyclopedia_C";
        Addressables.LoadAssetsAsync<EncyclopediaSO>(label, null).Completed += OnEntriesLoaded;
    }

    private void OnEntriesLoaded(AsyncOperationHandle<IList<EncyclopediaSO>> _handle)
    {
        if (_handle.Status == AsyncOperationStatus.Succeeded)
        {
            var entries = _handle.Result;
            foreach (var entry in entries)
            {
                CreateSlot(entry);
            }
        }
        else
        {
            Debug.LogError("항목 로드 실패.");
        }
    }

    private void CreateSlot(EncyclopediaSO _entry)
    {
        GameObject slot = Instantiate(slotPrefab, content);

        slot.GetComponent<SlotData>().SetEntry(_entry);

        Canvas slotCanvas = slot.GetComponentInChildren<Canvas>();
        if (slotCanvas != null)
        {
            slotCanvas.overrideSorting = false;
        }
    }
}
