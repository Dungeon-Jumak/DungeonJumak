using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITimeManager : MonoBehaviour
{
    private TimeZoneInfo _koreaTimeZone;

    public TimeZoneInfo KoreaTimeZone => _koreaTimeZone;

    public UITimeManager()
    {
        _koreaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Korea Standard Time");
    }

    public DateTime ConvertToKST(DateTime utcTime)
    {
        return TimeZoneInfo.ConvertTimeFromUtc(utcTime, _koreaTimeZone);
    }

    public DateTime GetCurrentKST()
    {
        return ConvertToKST(DateTime.UtcNow);
    }
}
