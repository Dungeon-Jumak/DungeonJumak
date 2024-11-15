using System.Collections.Generic;
using TMPro;
using Unity.Notifications.iOS;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class EncyclopediaRewardHandler : UI_PopUp
{
    enum Sliders { RewardSlider }
    enum Texts { RewardText }
    enum Buttons { RewardBtn }
    enum Category { Monster, Customer }

    [SerializeField] private RectTransform scrollViewContent;

    private DataManager<EncyclopediaData> g_EncyclopediaData;
    private DataManager<GoodsData> g_GoodsData;
    private Category currentCategory = Category.Customer;
    private int unlockCount;

    private void Awake()
    {
        g_EncyclopediaData = DataManager<EncyclopediaData>.Instance;
        g_GoodsData = DataManager<GoodsData>.Instance;
        BaseEncyclopedia.OnCategoryChanged += OnCategoryChanged;
        Init();

    }

    private void Start()
    {
        SetCategory();
    }

    private void Update()
    {
        UpdateUI();
    }
    public override void Init()
    {
        base.Init();
        Bind<Slider>(typeof(Sliders));
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Button>(typeof(Buttons));
        GetButton((int)Buttons.RewardBtn).gameObject.BindEvent(GetReward);
    }

    private void UpdateUI()
    {
        UpdateSliderUI();
        UpdateTextUI();
    }


    private void UpdateSliderUI()
    {
        var slider = GetSlider((int)Sliders.RewardSlider);
        var sliderCanvas = gameObject.GetComponentInChildren<Canvas>();

        slider.maxValue = scrollViewContent.childCount;
        slider.value = unlockCount;
        sliderCanvas.overrideSorting = false;
    }

    private void UpdateTextUI()
    {
        var rewardText = GetTMP((int)Texts.RewardText);
        rewardText.text = $"{unlockCount} / {scrollViewContent.childCount}";
    }

    private void OnCategoryChanged(BaseEncyclopedia.Category _newCategory)
    {
        currentCategory = (Category)System.Enum.Parse(typeof(Category), _newCategory.ToString());
        SetCategory();;
    }

    private void SetCategory()
    {
        switch (currentCategory)
        {
            case Category.Monster:
                SetUnlockCount(g_EncyclopediaData.Data.Monsters);
                break;
            case Category.Customer:
                SetUnlockCount(g_EncyclopediaData.Data.Customers);
                break;
        }
    }

    private void SetUnlockCount(Dictionary<string, bool> _categoryData)
    {
        unlockCount = 0;
        foreach (var entry in _categoryData)
        {
            if (entry.Value) unlockCount++;
        }
    }

    private void GetReward(PointerEventData _data)
    {
        //ToDo : 시간 제한 추가
        SetCategory();
        g_GoodsData.Data.Yeouiju += (unlockCount * 10);
    }
}
