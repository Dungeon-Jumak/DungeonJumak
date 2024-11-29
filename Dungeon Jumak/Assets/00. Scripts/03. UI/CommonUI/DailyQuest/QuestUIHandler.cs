using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class QuestUIHandler : UI_PopUp
{
    enum Buttons
    {
        Dim, MaxReward,
        DQMoveBtn1, DQMoveBtn2, DQMoveBtn3, DQMoveBtn4, DQMoveBtn5,
        DQRewardBtn1, DQRewardBtn2, DQRewardBtn3, DQRewardBtn4, DQRewardBtn5,
    }
    enum Sliders { All, DQ1, DQ2, DQ3, DQ4, DQ5 }
    enum Texts { DQText1, DQText2, DQText3, DQText4, DQText5 }

    private DataManager<QuestSaveData> g_QuestData;

    private void Awake()
    {
        g_QuestData = DataManager<QuestSaveData>.Instance;
        Init();
        UpdateMissionStatus();
        DisplayOverallProgress();
    }

    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons)); 
        Bind<Slider>(typeof(Sliders)); 
        Bind<TextMeshProUGUI>(typeof(Texts)); 

        GetButton((int)Buttons.Dim).gameObject.BindEvent(ClosePopUp);
    }

    private void UpdateMissionStatus()
    {
        foreach (var quest in g_QuestData.Data.QuestDictionary.Values)
        {
            SetBtnMode(quest);
            UpdateQuestSliderAndText(quest);
        }

        //Debug.Log($"현재 완료된 미션 수 : {GameManager.QuestManager.CompletedQuests} / {GameManager.QuestManager.MaxQuests}");
    }

    private void SetBtnMode(Quest _quest)
    {
        bool isCompleted = _quest.IsCompleted;
        ToggleQuestButtons(_quest, !isCompleted, isCompleted);
    }

    private void UpdateQuestSliderAndText(Quest _quest)
    {
        int questID = _quest.QuestData.QuestID;

        Slider questSlider = GetSlider((int)Sliders.All + questID);
        questSlider.maxValue = _quest.QuestData.ProgressGoal;
        questSlider.value = _quest.Progress;

        TextMeshProUGUI questText = GetTMP((int)Texts.DQText1 + questID - 1);
        questText.text = $"{_quest.Progress} / {_quest.QuestData.ProgressGoal}";

        //Debug.Log($"미션 {questID} 상태 : {_quest.Progress} / {_quest.QuestData.ProgressGoal}, 완료 : {_quest.IsCompleted}");
    }

    private void DisplayOverallProgress()
    {
        Slider overallSlider = GetSlider((int)Sliders.All);
        overallSlider.maxValue = GameManager.QuestManager.MaxQuests;
        overallSlider.value = GameManager.QuestManager.CompletedQuests;

        bool canGetMaxReward = overallSlider.value == GameManager.QuestManager.MaxQuests && !g_QuestData.Data.HasReceivedAllClearReward;
        SetMaxRewardButtonState(canGetMaxReward);
    }

    private void GetReward(Quest _quest, Button _rewardButton)
    {
        int questID = _quest.QuestData.QuestID;
 
        if (!_quest.HasReceivedReward)
        {
            _quest.HasReceivedReward = true;
            _rewardButton.interactable = false;
            GameManager.QuestManager.CheckQuestCompletion(questID);
        }
    }

    private void GetAllClearReward(Button maxRewardButton)
    {
        if (!g_QuestData.Data.HasReceivedAllClearReward)
        {
            g_QuestData.Data.HasReceivedAllClearReward = true;
            GameManager.QuestManager.GiveAllClearReward();
            maxRewardButton.interactable = false;
        }
    }

    public void ClosePopUp(PointerEventData _data)
    {
        GameManager.UI.ClosePopUpUI();
    }

    private void ToggleQuestButtons(Quest _quest, bool _moveButtonState, bool _rewardButtonState)
    {
        int questID = _quest.QuestData.QuestID;

        Button moveBtn = GetButton((int)Buttons.DQMoveBtn1 + questID - 1);
        Button rewardBtn = GetButton((int)Buttons.DQRewardBtn1 + questID - 1);

        moveBtn.gameObject.SetActive(_moveButtonState);
        rewardBtn.gameObject.SetActive(_rewardButtonState);

        if (_rewardButtonState && !_quest.HasReceivedReward)
        {
            rewardBtn.gameObject.BindEvent((PointerEventData _data) => GetReward(_quest, rewardBtn));
        }
        else
        {
            rewardBtn.interactable = false;
        }
    }

    private void SetMaxRewardButtonState(bool _canGetMaxReward)
    {
        Debug.Log(_canGetMaxReward);
        Button maxRewardButton = GetButton((int)Buttons.MaxReward);
        maxRewardButton.interactable = _canGetMaxReward;

        if (_canGetMaxReward)
        {
            maxRewardButton.gameObject.BindEvent((PointerEventData _data) => GetAllClearReward(maxRewardButton));
        }
    }
}
