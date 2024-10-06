//System
using CartoonFX;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.TerrainTools;

//Unity
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ProfilePageSystem : UI_PopUp
{
    enum Buttons
    {
        PreviousPage,
        NextPage,
    }

    enum Texts
    {
        BonusRevenue,
        MaxOfflineDuration,
    }

    [SerializeField] private GameObject[] ratingIcons;

    private int page = 0;

    private DataManager<RatingData> g_ratingData;

    private void Awake()
    {
        g_ratingData = DataManager<RatingData>.Instance;
    }

    private void Start()
    {
        Init();

        //g_ratingData.Data.Rating = 1;
        //g_ratingData.Data.BonusRevenue = 0.3f;
        //g_ratingData.Data.MaxOfflineDuration = 321;

        Mathf.Clamp(g_ratingData.Data.Rating, 0, ratingIcons.Length - 1);

        AdjustmentPage(g_ratingData.Data.Rating);
        page = g_ratingData.Data.Rating;

        SetBenefitsText(g_ratingData.Data.BonusRevenue, g_ratingData.Data.MaxOfflineDuration);
    }

    private void OnEnable()
    {

    }


    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));
        Bind<Text>(typeof(Texts));

        GetButton((int)Buttons.PreviousPage).gameObject.BindEvent(ToPreviousPage);
        GetButton((int)Buttons.NextPage).gameObject.BindEvent(ToNextPage);
    }

    private void ToPreviousPage(PointerEventData _data)
    {
        if (page > 0)
        {
            page--;
            AdjustmentPage(page);
        }
    }

    private void ToNextPage(PointerEventData _data)
    {
        if (page < ratingIcons.Length - 1)
        {
            page++;
            AdjustmentPage(page);
        }
    }

    private void AdjustmentPage(int CurrentPage)
    {
        CurrentPage = Mathf.Clamp(CurrentPage, 0, ratingIcons.Length - 1);

        foreach (var icon in ratingIcons)
        {
            icon.SetActive(false);
        }

        ratingIcons[CurrentPage].SetActive(true);
    }

    private void SetBenefitsText(float BonusRevenue, float MaxOfflineDuration)
    {
        if (page < g_ratingData.Data.Rating)
        {
            // To do : 업그레이드 완료 텍스트로 표시하기
            GetText((int)Texts.BonusRevenue).text = "";
            GetText((int)Texts.MaxOfflineDuration).text = "";
        }
        else 
        {
            GetText((int)Texts.BonusRevenue).text = $"결제 수익 : +{BonusRevenue * 100}%";

            float maxDuration = MaxOfflineDuration;
            int minutes = Mathf.RoundToInt(maxDuration / 60);

            GetText((int)Texts.MaxOfflineDuration).text = $"오프라인 최대 적용 시간 : {minutes}분";
        }
    }
}
