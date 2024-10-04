// System
using System;

/// <summary>
/// BGM, SFX 등 오디오와 관련된 데이터를 저장하는 클래스입니다.
/// </summary>
[Serializable]
public class AudioData
{
    // 재생 여부
    private bool isPlayBGM = true;
    private bool isPlaySFX = true;

    #region Property - Get / Set

    public bool IsPlayBGM
    {
        get { return isPlayBGM; }
        set { isPlayBGM = value; }
    }

    public bool IsPlaySFX
    {
        get { return isPlaySFX; }
        set { isPlaySFX = value; }
    }

    #endregion
}
