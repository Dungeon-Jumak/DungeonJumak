//System
using System;
using System.Collections;
using System.Collections.Generic;

//Unity
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_LanguagePopUp : UI_PopUp
{
    enum Buttons
    {
        Dim,
        Korean,
        English,
    }

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));

        GetButton((int)Buttons.Dim).gameObject.BindEvent(ClosePopUp);

        GetButton((int)Buttons.Korean).gameObject.BindEvent(SetLanguageKr);
        GetButton((int)Buttons.English).gameObject.BindEvent(SetLanguageEn);
    }

    public void ClosePopUp(PointerEventData _data)
    {
        GameManager.UI.ClosePopUpUI();

    }

    public void SetLanguageKr(PointerEventData _data)
    {
        GameManager.UI.ClosePopUpUI();
    }

    private void SetLanguageEn(PointerEventData _data)
    {
        GameManager.UI.ClosePopUpUI();
    }

}
