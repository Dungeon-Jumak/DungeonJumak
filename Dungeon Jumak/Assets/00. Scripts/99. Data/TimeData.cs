// System
using System;

/// <summary>
/// 게임에서 흘러가는 계절, 날짜, 낮밤 등 시간과 관련된 데이터들을 저장하는 클래스입니다.
/// </summary>
[Serializable]
public class TimeData
{
    private string[] season = new string[4] { "봄", "여름", "가을", "겨울" };
    private int day = 1;
    private string[] dayNight = new string[2] { "오전", "오후" };

    #region Property - Get / Set

    public string[] Season
    {
        get { return season; }
        set { season = value; }
    }

    public int Day
    {
        get { return day; }
        set { day = value; }
    }

    public string[] DayNight
    {
        get { return dayNight; }
        set { dayNight = value; }
    }

    #endregion
}
