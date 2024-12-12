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
        Encyclopedia,
        Daily,
    }

    private void Start()
    {
        Bind<Button>(typeof(Buttons));

        GetButton((int)Buttons.Setting).gameObject.BindEvent(OpenSettingPopUp);
        GetButton((int)Buttons.Encyclopedia).gameObject.BindEvent(OpenEncyclopediaPopUp);
        GetButton((int)Buttons.Daily).gameObject.BindEvent(OpenDailyQuestPopup);
    }

    public void OpenSettingPopUp(PointerEventData _data)
    {
        GameManager.UI.ShowPopupUI<UI_PopUp>("SettingPopUp");
    }

    public void OpenEncyclopediaPopUp(PointerEventData _data)
    {
        GameManager.UI.ShowPopupUI<UI_PopUp>("EncyclopediaPopUp");
    }

    public void OpenDailyQuestPopup(PointerEventData _data)
    {
        GameManager.UI.ShowPopupUI<UI_PopUp>("DailyQuestPopup");
    }
}
