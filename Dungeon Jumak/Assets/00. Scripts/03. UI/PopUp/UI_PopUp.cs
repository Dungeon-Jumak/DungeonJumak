//System
using System.Collections;
using System.Collections.Generic;

//Unity
using UnityEngine;

/// <summary>
/// 캔버스를 초기화하기 위한 클래스
/// </summary>
public class UI_PopUp : UI_Base
{
    public virtual void Init()
    {
        GameManager.UI.SetCanvas(gameObject, true);
    }

    /// <summary>
    /// 팝업을 닫기 위한 메소드
    /// </summary>
    public virtual void ClosePopUpUI()
    {
        GameManager.UI.ClosePopUpUI(this);
    }
}
