namespace Utils.EnumTypes {

    /// <summary>
    /// 글로벌하게 쓰일 Enum 열거형 타입들을 모아두는 공간입니다.
    /// </summary>

    /*
    ex)
    public enum DPEventTypes
    {
        Fail, Success, None
    }
    */

    /// <summary>
    /// SoundManager SFX_Label
    /// </summary>
    public enum SFX_Label
    {
        Main_SFX,
        Dungeon_SFX,
        ETC_SFX,
    }

    public enum SoundType
    {
        Bgm,
        Sfx,
        MaxCount,
    }

    /// <summary>
    ///몬스터 이벤트 관리
    /// </summary>
    public enum MonsterEventType
    {
        HitBySkill, // 스킬에 맞았을 경우
    }

    public enum PlayerEventType
    {
        HitByMonster, // 몬스터와 충돌했을 경우
    }
}

