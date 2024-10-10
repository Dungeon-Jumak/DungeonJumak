// System
using System;

/// <summary>
/// 등급과 관련된 데이터를 저장하는 클래스입니다.
/// </summary>
[Serializable]
public class RatingData
{
    private int currentRating = 0;
    private int currentInteriorScore = 0;

    #region Property - Get / Set
    public int CurrentRating
    {
        get { return currentRating; }
        set { currentRating = value; }
    }

    public int CurrentInteriorScore
    {
        get { return currentInteriorScore; }
        set { currentInteriorScore = value; }
    }
    #endregion
}
