//System
using System;
using System.Collections;
using System.Collections.Generic;

//Unity
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Buttons_Width : UI_PopUp
{
    enum Buttons
    {
        Profile,
        Yeouiju,
        Money,
        Equipment,
        Dungeon,
        JumakManagement,
        Storage,
        Market,
    }

    private void Start()
    {
        Bind<Button>(typeof(Buttons));

        GetButton((int)Buttons.Profile).gameObject.BindEvent(OpenProfilePopUp);
        GetButton((int)Buttons.Yeouiju).gameObject.BindEvent(OpenDefaultPopUp);
        GetButton((int)Buttons.Money).gameObject.BindEvent(OpenDefaultPopUp);
        GetButton((int)Buttons.Equipment).gameObject.BindEvent(OpenDefaultPopUp);
        GetButton((int)Buttons.Dungeon).gameObject.BindEvent(OpenDefaultPopUp);
        GetButton((int)Buttons.JumakManagement).gameObject.BindEvent(OpenDefaultPopUp);
        GetButton((int)Buttons.Storage).gameObject.BindEvent(OpenDefaultPopUp);
        GetButton((int)Buttons.Market).gameObject.BindEvent(OpenDefaultPopUp);
    }

    public void OpenDefaultPopUp(PointerEventData _data)
    {
        GameManager.UI.ShowPopupUI<UI_PopUp>("DefaultPopUp");
    }

    private void OpenProfilePopUp(PointerEventData _data)
    {
        GameManager.UI.ShowPopupUI<UI_PopUp>("ProfilePopUp");
    }
}
