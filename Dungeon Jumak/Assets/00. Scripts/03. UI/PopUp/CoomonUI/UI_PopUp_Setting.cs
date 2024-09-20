//System
using System;
using System.Collections;
using System.Collections.Generic;

//Unity
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_PopUp_Setting : UI_PopUp
{
    enum Buttons
    {
        BgmOn,
        BgmOff
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

        GetButton((int)Buttons.BgmOn).gameObject.BindEvent(BgmOff);
        GetButton((int)Buttons.BgmOff).gameObject.BindEvent(BgmOn);
    }

    private void BgmOn(PointerEventData _data)
    {
        data.g_onBgm = true;
        DataManager.Instance.SaveGameData();
    }

    private void BgmOff(PointerEventData _data)
    {
        data.g_onBgm = false;
        DataManager.Instance.SaveGameData();
    }
}
