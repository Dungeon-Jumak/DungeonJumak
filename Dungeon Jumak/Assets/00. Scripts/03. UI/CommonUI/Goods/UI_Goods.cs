//System
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;

//Unity
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Goods : UI_PopUp
{
    enum Texts
    {
        MoneyCount,
        YeouijuCount,
    }

    private DataManager<GoodsData> g_goodsData;

    private void Awake()
    {
        g_goodsData = DataManager<GoodsData>.Instance;
    }

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        DisplayCoin();
        DisplayYeouiju();
    }

    public override void Init()
    {
        base.Init();

        Bind<TextMeshProUGUI>(typeof(Texts));
   }

    private void DisplayCoin()
    {
        GetTMP((int)Texts.MoneyCount).text = $"{g_goodsData.Data.Money} 전";
    }

    private void DisplayYeouiju()
    {
        GetTMP((int)Texts.YeouijuCount).text = $"{g_goodsData.Data.Yeouiju} 개";
    }

}
