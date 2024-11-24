using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;
using UnityEngine.EventSystems;

public class DungeonEndPopup : UI_PopUp
{
    enum Texts { StageTitle }
    enum Buttons { Lobby, BonusReward }

    private DataManager<DungeonPopupData> g_DungeonPopupData;
    private Dictionary<int, string> stageTitles;

    private void Awake()
    {
        g_DungeonPopupData = DataManager<DungeonPopupData>.Instance;
        InitializeStageTitles();
        Init();
    }

    private void InitializeStageTitles()
    {
        stageTitles = new Dictionary<int, string>
        {
            { 1, "시작의 초원" },
            { 2, "물안개 계곡" }
        };
    }

    public override void Init()
    {
        base.Init();
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Button>(typeof(Buttons));

        GetButton((int)Buttons.Lobby).gameObject.BindEvent(GoToLobbyScene);
        GetButton((int)Buttons.BonusReward).gameObject.BindEvent(GetBonusReward);

        SetStageTitle();
    }

    private void GoToLobbyScene(PointerEventData _data)
    {
        GameManager.UI.ClosePopUpUI();
        SceneManager.LoadScene("Main");
    }

    private void GetBonusReward(PointerEventData _data)
    {
        // TODO : 광고 재생 및 보상 제공
        Debug.Log("광고를 재생하고 보상을 제공합니다.");
    }

    private void SetStageTitle()
    {
        if (stageTitles.ContainsKey(g_DungeonPopupData.Data.CurrentStage))
        {
            GetTMP((int)Texts.StageTitle).text = stageTitles[g_DungeonPopupData.Data.CurrentStage];
        }
        else
        {
            GetTMP((int)Texts.StageTitle).text = "알 수 없는 스테이지";
        }
    }
}
