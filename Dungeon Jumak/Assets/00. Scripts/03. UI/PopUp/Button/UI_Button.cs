//System
using System;
using System.Collections;
using System.Collections.Generic;

//Unity
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Button : UI_PopUp
{
    enum Buttons
    {
        PointButton
    }

    enum Texts
    {
    }

    enum GameObjects
    {

    }

    enum Images
    {

    }

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));
        Bind<Text>(typeof(Texts));
        Bind<GameObject>(typeof(GameObjects));
        Bind<Image>(typeof(Images));

        GetButton((int)Buttons.PointButton).gameObject.BindEvent(OpenPopUp);
    }

    public void OpenPopUp(PointerEventData data)
    {
        GameManager.UI.ShowPopupUI<UI_PopUp>("DefaultPopUp");
    }
}
