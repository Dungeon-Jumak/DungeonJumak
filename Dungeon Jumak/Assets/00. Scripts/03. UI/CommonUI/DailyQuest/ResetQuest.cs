using System;
using UnityEngine;
using TMPro;

public class ResetQuest : UI_PopUp
{
    enum Texts { ResetTimeText }

    private DataManager<QuestSaveData> g_QuestData;

    private void OnEnable()
    {
        g_QuestData = DataManager<QuestSaveData>.Instance;
        Init();
    }

    void Update()
    {
        DateTime currentTimeKst = GameManager.UITimeManager.GetCurrentKST();
        CheckAndResetQuests(currentTimeKst);
        UpdateResetTimeDisplay(currentTimeKst);
    }

    public override void Init()
    {
        base.Init();

        Bind<TextMeshProUGUI>(typeof(Texts));
    }

    private void UpdateResetTimeDisplay(DateTime currentTime)
    {
        DateTime midnight = currentTime.Date.AddDays(1);
        TimeSpan timeLeft = midnight - currentTime;

        if (timeLeft < TimeSpan.Zero)
        {
            timeLeft = TimeSpan.Zero;
        }

        GetTMP((int)Texts.ResetTimeText).text = $"미션 새로 고침 {FormatTime(timeLeft)}";
    }

    private string FormatTime(TimeSpan time)
    {
        return string.Format("{0:D2}:{1:D2}:{2:D2}", time.Hours, time.Minutes, time.Seconds);
    }

    private void CheckAndResetQuests(DateTime currentTime)
    {
        foreach (Quest quest in g_QuestData.Data.QuestDictionary.Values)
        {
            if (currentTime >= quest.NextResetTimeKST)
            {
                
                GameManager.QuestManager.ResetQuests(quest);
                g_QuestData.Data.HasReceivedAllClearReward = false;
            }
        }
    }
}
