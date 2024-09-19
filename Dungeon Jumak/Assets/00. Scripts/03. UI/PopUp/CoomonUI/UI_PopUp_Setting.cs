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

    private bool m_bgOn;
    private bool m_bgOff;

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));

        GetButton((int)Buttons.BgmOn).gameObject.BindEvent(BgmOn);
        GetButton((int)Buttons.BgmOff).gameObject.BindEvent(BgmOff);
    }

    private void BgmOn(PointerEventData _data)
    {
        m_bgOn = true;
        m_bgOff = false;
    }

    private void BgmOff(PointerEventData _data)
    {
        m_bgOff= true;
        m_bgOn = false;
    }
}
