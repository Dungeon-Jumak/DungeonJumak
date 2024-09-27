//System
using System;
using System.Collections;
using System.Collections.Generic;

//Unity
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SettingPopUp: UI_PopUp
{
    enum Buttons
    {
        Dim,
        BgmOn,
        BgmOff,
        SfxOn,
        SfxOff,
        PushOn,
        PushOff,
        NPushOn,
        NPushOff,
        Language,
    }

    private GlobalData data;
    private void Awake()
    {
        data = DataManager.Instance.data;
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

        GetButton((int)Buttons.BgmOn).gameObject.BindEvent(BgmOff);
        GetButton((int)Buttons.BgmOff).gameObject.BindEvent(BgmOn);

        GetButton((int)Buttons.SfxOn).gameObject.BindEvent(SfxOff);
        GetButton((int)Buttons.SfxOff).gameObject.BindEvent(SfxOn);

        GetButton((int)Buttons.PushOn).gameObject.BindEvent(PushOff);
        GetButton((int)Buttons.PushOff).gameObject.BindEvent(PushOn);

        GetButton((int)Buttons.NPushOn).gameObject.BindEvent(N_PushOff);
        GetButton((int)Buttons.NPushOff).gameObject.BindEvent(N_PushOn);

        GetButton((int)Buttons.Language).gameObject.BindEvent(SetLanguage);
    }

    public void ClosePopUp(PointerEventData _data)
    {
        GameManager.UI.ClosePopUpUI();
    }

    private void BgmOn(PointerEventData _data)
    {   
        //data.g_onBgm = true;
    }

    private void BgmOff(PointerEventData _data)
    {
        //data.g_onBgm = false;
    }

    private void SfxOn(PointerEventData _data)
    {
        //data.g_onSfx = true;
    }

    private void SfxOff(PointerEventData _data)
    {
        //data.g_onSfx = false;
    }

    private void PushOn(PointerEventData _data)
    {
        //data.g_onSfx = true;
    }

    private void PushOff(PointerEventData _data)
    {
        //data.g_onSfx = false;
    }

    private void N_PushOn(PointerEventData _data)
    {
        //data.g_onSfx = true;
    }

    private void N_PushOff(PointerEventData _data)
    {
        //data.g_onSfx = false;
    }

    private void SetLanguage(PointerEventData _data)
    {
        GameManager.UI.ShowPopupUI<UI_PopUp>("LanguagePopUp");
    }


}
