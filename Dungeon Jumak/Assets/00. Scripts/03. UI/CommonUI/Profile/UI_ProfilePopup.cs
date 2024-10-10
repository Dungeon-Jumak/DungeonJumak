//System
using CartoonFX;
using System;
using System.Collections;
using System.Collections.Generic;

//Unity
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ProfilePopup : UI_PopUp
{
    enum Buttons
    {
        Dim,
        Reinforce,
    }

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        //부모 초기화 함수 오버라이딩
        base.Init();

        //오브젝트 바인딩
        Bind<Button>(typeof(Buttons));

        //이벤트 바인딩
        GetButton((int)Buttons.Dim).gameObject.BindEvent(ClosePopUp);
        GetButton((int)Buttons.Reinforce).gameObject.BindEvent(OpenJumakManagePopup);
    }

    public void ClosePopUp(PointerEventData _data)
    {
        GameManager.UI.ClosePopUpUI();
    }

    private void OpenJumakManagePopup(PointerEventData _data)
    {
        GameManager.UI.ClosePopUpUI();
        GameManager.UI.ShowPopupUI<UI_PopUp>("JumakManagePopup");
    }
}
