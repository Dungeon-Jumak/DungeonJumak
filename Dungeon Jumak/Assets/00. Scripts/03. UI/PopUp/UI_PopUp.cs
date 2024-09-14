//System
using System.Collections;
using System.Collections.Generic;

//Unity
using UnityEngine;

/// <summary>
/// ĵ������ �ʱ�ȭ�ϱ� ���� Ŭ����
/// </summary>
public class UI_PopUp : UI_Base
{
    public virtual void Init()
    {
        GameManager.UI.SetCanvas(gameObject, true);
    }

    /// <summary>
    /// �˾��� �ݱ� ���� �޼ҵ�
    /// </summary>
    public virtual void ClosePopUpUI()
    {
        GameManager.UI.ClosePopUpUI(this);
    }
}
