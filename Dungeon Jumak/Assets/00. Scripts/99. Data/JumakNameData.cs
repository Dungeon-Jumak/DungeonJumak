// System
using System;

/// <summary>
/// 주막 이름 데이터를 저장하는 클래스입니다.
/// </summary>
[Serializable]
public class JumakNameData
{
    private string jumakName = "";

    #region Property - Get / Set

    public string JumakName
    {
        get { return jumakName; }
        set { jumakName = value; }
    }

    #endregion
}
