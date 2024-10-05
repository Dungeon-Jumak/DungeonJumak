// System
using System;

/// <summary>
/// 등급과 관련된 데이터를 저장하는 클래스입니다.
/// </summary>
[Serializable]
public class RatingData
{
    private int rating;
    private int interiorScore;
    private float bonusRevenue;
    private float maxOfflineDuration;

    #region Property - Get / Set
    public int Rating
    {
        get { return rating; }
        set { rating = value; }
    }

    public int InteriorScore
    {
        get { return interiorScore; }
        set { interiorScore = value; }
    }

    public float BonusRevenue
    {
        get { return bonusRevenue; }
        set { bonusRevenue = value; }
    }

    public float MaxOfflineDuration
    { 
        get { return maxOfflineDuration; } 
        set { maxOfflineDuration = value; } 
    }

    #endregion
}
