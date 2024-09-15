//Engine
using UnityEngine;

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

}

