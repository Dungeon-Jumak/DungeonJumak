using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DungeonRetryPopup : UI_PopUp
{
    enum Buttons { Close, WatchAD, ConsumeYeouiju }

    private DataManager<DungeonPopupData> g_DungeonPopupData;
    private DataManager<GoodsData> g_GoodsData;

    private void Awake()
    {
        g_DungeonPopupData = DataManager<DungeonPopupData>.Instance;
        g_GoodsData = DataManager<GoodsData>.Instance;
        Init();
    }


    public override void Init()
    {
        base.Init();
        Bind<Button>(typeof(Buttons));

        GetButton((int)Buttons.Close).gameObject.BindEvent(ClosePopUp);
        GetButton((int)Buttons.ConsumeYeouiju).gameObject.BindEvent(RetryConsumeYeouiju);
        GetButton((int)Buttons.WatchAD).gameObject.BindEvent(RetryWatchAD);
    }

    private void ClosePopUp(PointerEventData _data)
    {
        GameManager.UI.ClosePopUpUI();
        GameManager.UI.ShowPopupUI<UI_PopUp>("DungeonGameOverPopUp");
    }

    private void RetryConsumeYeouiju(PointerEventData _data)
    {
        if (g_GoodsData.Data.Yeouiju > 30)
        {
            GameManager.UI.ClosePopUpUI();
            g_GoodsData.Data.Yeouiju -= 30;
            // To DO : 던전 다시하기
        }
    }

    private void RetryWatchAD(PointerEventData _data)
    {
        GameManager.UI.ClosePopUpUI();
        // To DO : 광고 재생 및 던전 다시하기
    }


}
