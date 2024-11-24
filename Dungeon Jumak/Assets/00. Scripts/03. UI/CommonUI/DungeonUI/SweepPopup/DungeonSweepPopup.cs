using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DungeonSweepPopup : UI_PopUp
{
    enum Texts { StageTitle, SweepCountText }
    enum Buttons { Sweep }

    private DataManager<DungeonPopupData> g_DungeonPopupData;
    private int stageIndex;

    public static event System.Action<int> OnAdditionalSweepBtnClicked;

    private void Awake()
    {
        g_DungeonPopupData = DataManager<DungeonPopupData>.Instance;
        Init();
        BaseDungeonListPopup.OnSweepBtnClicked += SetUI;

        g_DungeonPopupData.Data.SweepCount = 2;
    }

    public override void Init()
    {
        base.Init();
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Button>(typeof(Buttons));
        GetButton((int)Buttons.Sweep).gameObject.BindEvent(GetSweepReward);
    }

    private void SetUI(int _StageIndex)
    {
        stageIndex = _StageIndex;
        GetTMP((int)Texts.StageTitle).text = $"{_StageIndex}스테이지 소탕";
        UpdateSweepCountText();
    }

    public void GetSweepReward(PointerEventData _data)
    {
        if (g_DungeonPopupData.Data.SweepCount > 0)
        {
            g_DungeonPopupData.Data.SweepCount--;
            UpdateSweepCountText();
            //TO Do : 보상 획득
            //To Do : 시간에 따른 소탕 횟수 회복
        }
        else
        {
            ShowAdditionalSweepPopup();
        }
    }

    private void UpdateSweepCountText()
    {
        GetTMP((int)Texts.SweepCountText).text = $"소탕 {g_DungeonPopupData.Data.SweepCount} / 2";
    }

    private void ShowAdditionalSweepPopup()
    {
        GameManager.UI.ClosePopUpUI();
        GameManager.UI.ShowPopupUI<UI_PopUp>("DungeonAdditionalSweepPopUp");
        OnAdditionalSweepBtnClicked?.Invoke(stageIndex);
    }

}
