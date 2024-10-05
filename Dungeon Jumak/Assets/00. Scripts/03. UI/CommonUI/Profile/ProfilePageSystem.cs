//System
using CartoonFX;
using System;
using System.Collections;
using System.Collections.Generic;

//Unity
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ProfilePageSystem : UI_PopUp
{
    enum Buttons
    {
        PreviousPage,
        NextPage,
    }

    [SerializeField] private GameObject[] ratingIcons;

    private int page = 0;

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));

        GetButton((int)Buttons.PreviousPage).gameObject.BindEvent(ToPreviousPage);
        GetButton((int)Buttons.NextPage).gameObject.BindEvent(ToNextPage);
    }

    private void ToPreviousPage(PointerEventData _data)
    {
        if (page > 0)
        {
            page--;
            AdjustmentPage();
        }
    }

    private void ToNextPage(PointerEventData _data)
    {
        if (page < ratingIcons.Length - 1)
        {
            page++;
            AdjustmentPage();
        }
    }

    private void AdjustmentPage()
    {
        page = Mathf.Clamp(page, 0, ratingIcons.Length - 1);

        foreach (var icon in ratingIcons)
        {
            icon.SetActive(false);
        }

        ratingIcons[page].SetActive(true);
    }
}
