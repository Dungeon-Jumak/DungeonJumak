//System
using System;
using System.Collections;
using System.Collections.Generic;

//Unity
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_DefaultPopUp : UI_PopUp
{
    enum Buttons
    {
        Dim,
    }

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        //�θ� �ʱ�ȭ �Լ� �������̵�
        base.Init();

        //������Ʈ ���ε�
        Bind<Button>(typeof(Buttons));

        //�̺�Ʈ ���ε�
        GetButton((int)Buttons.Dim).gameObject.BindEvent(ClosePopUp);
    }

    public void ClosePopUp(PointerEventData _data)
    {
        GameManager.UI.ClosePopUpUI();
    }
}
