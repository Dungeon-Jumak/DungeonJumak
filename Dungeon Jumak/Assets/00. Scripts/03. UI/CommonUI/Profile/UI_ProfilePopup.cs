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
        PreviousPage,
        NextPage,
        Reinforce,
    }

    [SerializeField] private GameObject[] ratingIcon;

    private int page = 0;

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
        GetButton((int)Buttons.PreviousPage).gameObject.BindEvent(ToPreviousPage);
        GetButton((int)Buttons.NextPage).gameObject.BindEvent(ToNextPage);
    }

    public void ClosePopUp(PointerEventData _data)
    {
        GameManager.UI.ClosePopUpUI();
    }

    private void ToPreviousPage(PointerEventData _data)
    {
        if (page <= 0) return;
        page--;
        AdjustmentPage();
    }
    private void ToNextPage(PointerEventData _data)
    {
        if (page >= ratingIcon.Length - 1) return;
        page++;
        AdjustmentPage();
    }

    private void AdjustmentPage()
    {
        foreach (var icon in ratingIcon)
        {
            icon.SetActive(false);
        }

        if (page >= 0 && page < ratingIcon.Length)
        {
            ratingIcon[page].SetActive(true);
        }
    }
}
