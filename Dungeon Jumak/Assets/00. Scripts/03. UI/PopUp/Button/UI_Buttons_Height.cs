//System
using System;
using System.Collections;
using System.Collections.Generic;

//Unity
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Buttons_Height : UI_PopUp
{
    enum Buttons
    {
        Setting,
    }

    private void Start()
    {
        Bind<Button>(typeof(Buttons));

        GetButton((int)Buttons.Setting).gameObject.BindEvent(OpenSettingPopUp);
    }

    public void OpenSettingPopUp(PointerEventData _data)
    {
        GameManager.UI.ShowPopupUI<UI_PopUp>("SettingPopUp");
    }
}
