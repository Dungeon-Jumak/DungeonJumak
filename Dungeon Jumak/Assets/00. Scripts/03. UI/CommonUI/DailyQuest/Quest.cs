using System;

public class Quest
{
    public QuestData QuestData { get; private set; }
    public int Progress { get; private set; }
    public bool IsCompleted { get; private set; }
    public bool IsProgressIncreased { get; set; }
    public bool HasReceivedReward { get; set; }
    public DateTime NextResetTimeUTC { get; private set; }

    public Quest(QuestData _questData)
    {
        QuestData = _questData;
        Progress = 0;
        IsCompleted = false;
        IsProgressIncreased = false;
        HasReceivedReward = false;
        UpdateNextResetTime(DateTime.UtcNow);
    }

    public void UpdateProgress(int _amount)
    {
        if (!IsCompleted)
        {
            Progress += _amount;
            if (Progress >= QuestData.ProgressGoal)
            {
                Progress = QuestData.ProgressGoal;
                IsCompleted = true;
            }
        }
    }

    public bool IsTimeLimitExceeded()
    {
        return DateTime.UtcNow > NextResetTimeUTC;
    }

    public void Reset()
    {
        Progress = 0;
        IsCompleted = false;
        IsProgressIncreased = false;
        HasReceivedReward = false;
        UpdateNextResetTime(DateTime.UtcNow);
    }

    public void UpdateNextResetTime(DateTime _now)
    {
        DateTime resetTimeToday = _now.Date;

        if (QuestData.QuestType == QuestType.Daily)
        {
            resetTimeToday = resetTimeToday.AddDays(1);
        }
        else if (QuestData.QuestType == QuestType.Weekly && QuestData.ResetDayOfWeek.HasValue)
        {
            int daysUntilNextReset = CalculateDaysUntilNextReset(_now);
            resetTimeToday = resetTimeToday.AddDays(daysUntilNextReset);
        }
        else if (QuestData.QuestType == QuestType.Weekly && !QuestData.ResetDayOfWeek.HasValue)
        {
            resetTimeToday = resetTimeToday.AddDays((7 - (int)_now.DayOfWeek + (int)QuestData.ResetDayOfWeek.Value) % 7);
        }

        NextResetTimeUTC = resetTimeToday;
    }

    private int CalculateDaysUntilNextReset(DateTime _now)
    {
        int daysUntilNextReset = (7 + (int)QuestData.ResetDayOfWeek.Value - (int)_now.DayOfWeek) % 7;

        if (daysUntilNextReset == 0 && _now.Hour >= 0)
        {
            daysUntilNextReset = 7;
        }

        return daysUntilNextReset;
    }
}