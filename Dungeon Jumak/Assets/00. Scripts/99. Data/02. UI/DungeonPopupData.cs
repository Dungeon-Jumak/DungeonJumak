// System
using System;

/// <summary>
/// 던전 UI 정보와 관련된 데이터를 저장하는 클래스입니다.
/// </summary>
[Serializable]
public class DungeonPopupData
{
    private bool[] stageCleared = new bool[2];
    private int currentStage;
    private int sweepCount;
    private int consumeYeouiju;

    #region Property - Get / Set
    public bool[] StageCleared
    {
        get { return stageCleared; }
        set { stageCleared = value; }
    }

    public int SweepCount
    {
        get { return sweepCount; }
        set { sweepCount = value; }

    }

    public int CurrentStage
    {
        get { return currentStage; }
        set { currentStage = value; }
    }

    public int ConsumeYeouiju
    {
        get { return consumeYeouiju; }
        set { consumeYeouiju = value; }
    }
    #endregion
}
