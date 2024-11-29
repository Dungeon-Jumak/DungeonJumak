// System
using System;
using System.Collections.Generic;

/// <summary>
/// 퀘스트 데이터를 저장하는 클래스입니다.
/// </summary>
[Serializable]
public class QuestSaveData
{
    private Dictionary<int, Quest> questDictionary = new Dictionary<int, Quest>();
    private bool hasReceivedAllClearReward = false;

    #region Property - Get / Set

    public Dictionary<int, Quest> QuestDictionary
    {
        get { return questDictionary; }
        set { questDictionary = value; }
    }

    public bool HasReceivedAllClearReward
    {
        get { return hasReceivedAllClearReward; }
        set { hasReceivedAllClearReward = value; }
    }

    public QuestSaveData()
    {
        InitializeQuestData();
    }

    #endregion
    public void InitializeQuestData()
    {
        var questDataList = new List<QuestData>
        {
            new QuestData(1, 1, QuestType.Daily, DayOfWeek.Monday, 1),
            new QuestData(2, 10, QuestType.Daily, DayOfWeek.Monday, 1),
            new QuestData(3, 1, QuestType.Daily, DayOfWeek.Monday, 1),
            new QuestData(4, 1, QuestType.Daily, DayOfWeek.Monday, 1),
            new QuestData(5, 1000, QuestType.Daily, DayOfWeek.Monday, 1)
        };

        foreach (var questData in questDataList)
        {
            questDictionary[questData.QuestID] = new Quest(questData);
        }
    }
}
