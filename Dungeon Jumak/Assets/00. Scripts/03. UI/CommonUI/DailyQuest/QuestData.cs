using System;
using System.Collections.Generic;
using UnityEngine;

public enum QuestType
{
    Daily,  
    Weekly  
}

public class QuestData
{
    public int QuestID { get; private set; }
    public int ProgressGoal { get; private set; }
    public QuestType QuestType { get; private set; }
    public DayOfWeek? ResetDayOfWeek { get; private set; }
    public int Reward { get; private set; }

    public QuestData(int questID, int progressGoal, QuestType questType, int reward)
    {
        QuestID = questID;
        ProgressGoal = progressGoal;
        QuestType = questType;
        Reward = reward;
        ResetDayOfWeek = null;
    }

    public QuestData(int questID, int progressGoal, QuestType questType, DayOfWeek resetDayOfWeek, int reward)
    {
        QuestID = questID;
        ProgressGoal = progressGoal;
        QuestType = questType;
        ResetDayOfWeek = resetDayOfWeek;
        Reward = reward;
    }
}