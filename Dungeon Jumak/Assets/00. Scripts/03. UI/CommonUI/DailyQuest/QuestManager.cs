using System.Collections.Generic;
using System;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    private int completedQuests = 0;
    private const int maxQuests = 5;

    private DataManager<QuestSaveData> g_QuestData;
    private DataManager<GoodsData> g_GoodsData;

    #region Property
    public int CompletedQuests => completedQuests;
    public int MaxQuests => maxQuests;
    #endregion

    private void Init()
    {
        g_QuestData = DataManager<QuestSaveData>.Instance;
        g_GoodsData = DataManager<GoodsData>.Instance;
    }

    private Quest GetQuest(int _questID)
    {
        Init();

        return g_QuestData.Data.QuestDictionary.ContainsKey(_questID)
           ? g_QuestData.Data.QuestDictionary[_questID]
           : null;
    }


    private void HandleQuestCompletion(Quest quest)
    {
        if (quest.IsCompleted && !quest.IsProgressIncreased)
        {
            completedQuests++;
            quest.IsProgressIncreased = true;
        }
    }

    public void UpdateQuestProgress(int _questID, int _amount)
    {
        Quest quest = GetQuest(_questID);
        if (quest != null)
        {
            quest.UpdateProgress(_amount);
            HandleQuestCompletion(quest);
        }
    }


    public void CheckQuestCompletion(int _questID)
    {
        Quest quest = GetQuest(_questID);
        if (quest != null)
        {
            RewardReceived(quest);
        }
    }

    private void RewardReceived(Quest _quest)
    {
        g_GoodsData.Data.Yeouiju += _quest.QuestData.Reward;
        //Debug.Log($"보상 획득: {_quest.QuestData.Reward} 개");
    }

    private bool AllQuestsCompleted()
    {
        return completedQuests >= maxQuests;
    }

    public void GiveAllClearReward()
    {
        if (AllQuestsCompleted())
        {
            g_GoodsData.Data.Yeouiju += 30;
            //Debug.Log("모든 퀘스트 클리어 보상 지급!");
        }
    }

    public void ResetQuests(Quest _quest)
    {
        Init();
        DateTime nowKst = GameManager.UITimeManager.GetCurrentKST();

        if (_quest.IsTimeLimitExceeded())
        {
            _quest.Reset();
            _quest.UpdateNextResetTime(nowKst);
            completedQuests = 0;
        }
    }

}