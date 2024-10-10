// System
using System;

/// <summary>
/// Push_M, Night_Push_M 등 설정과 관련된 데이터를 저장하는 클래스입니다.
/// </summary>
[Serializable]
public class SettingData
{
    private bool isPushEnabled = true;
    private bool isNightPushEnabled = true;

    #region Property - Get / Set

    public bool IsPushEnable
    {
        get { return isPushEnabled; }
        set { isPushEnabled = value; }
    }

    public bool IsNightPushEnable
    {
        get { return isNightPushEnabled; }
        set { isNightPushEnabled = value; }
    }

    #endregion
}
