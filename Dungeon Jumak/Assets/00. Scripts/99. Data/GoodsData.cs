// System
using System;

/// <summary>
/// 재화 관련된 데이터를 저장하는 클래스입니다.
/// </summary>
[Serializable]
public class GoodsData
{
    private int money = 0;
    private int yeouiju = 0;

    #region Property - Get / Set

    public int Money
    {
        get { return money; }
        set { money = value; }
    }

    public int Yeouiju
    {
        get { return yeouiju; }
        set { yeouiju = value; }
    }

    #endregion
}
