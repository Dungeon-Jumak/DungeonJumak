using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DungeonAdditionalSweepPopup : UI_PopUp
{
    enum Texts { StageTitle, YeouijuConfirm }
    enum Buttons { Confirm }

    private DataManager<DungeonPopupData> g_DungeonPopupData;

    private void Awake()
    {
        g_DungeonPopupData = DataManager<DungeonPopupData>.Instance;
        Init();
        DungeonSweepPopup.OnAdditionalSweepBtnClicked += SetUI;
    }

    public override void Init()
    {
        base.Init();
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Button>(typeof(Buttons));
        GetButton((int)Buttons.Confirm).gameObject.BindEvent(GetSweepReward);
    }

    private void SetUI(int _StageIndex)
    {
        GetTMP((int)Texts.StageTitle).text = $"{_StageIndex}스테이지 소탕";
    }

    public void GetSweepReward(PointerEventData _data)
    {
        // TO Do : 여의주 소모
        // To Do : 보상 획득
        // TO DO : UI 업데이트
        GameManager.UI.ClosePopUpUI();
    }
}
