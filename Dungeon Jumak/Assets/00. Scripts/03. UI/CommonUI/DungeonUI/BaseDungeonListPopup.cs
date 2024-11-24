using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BaseDungeonListPopup : UI_PopUp
{
    private enum Buttons
    {
        Dim,
        EntryStage1, EntryStage2,
        SweepStage1, SweepStage2,
    }

    [Header("UI Group")]
    [SerializeField] private GameObject[] EntryGroup;  
    [SerializeField] private GameObject[] SweepGroup;

    private DataManager<DungeonPopupData> g_DungeonPopupData;
    private bool isInit;

    public static event System.Action<int> OnSweepBtnClicked;
    public static event System.Action<int> OnEntryBtnClicked;

    private void Awake()
    {
        g_DungeonPopupData = DataManager<DungeonPopupData>.Instance;
    }

    private void Start()
    {
        g_DungeonPopupData.Data.StageCleared[0] = true;
        HandleStageButtons();
    }

    public override void Init()
    {
        base.Init();
        Bind<Button>(typeof(Buttons));
        GetButton((int)Buttons.Dim).gameObject.BindEvent(ClosePopUp);
    }

    private void HandleStageButtons()
    {
        for (int i = 0; i < g_DungeonPopupData.Data.StageCleared.Length; i++)
        {
            SetStageUI(i);

            BindStageButtons(i);
        }
    }

    private void SetStageUI(int _index)
    {
        if (g_DungeonPopupData.Data.StageCleared[_index])
        {
            EntryGroup[_index].SetActive(false);
            SweepGroup[_index].SetActive(true);
        }
        else
        {
            SweepGroup[_index].SetActive(false);
        }
    }

    private void BindStageButtons(int _index)
    {
        if (!isInit) 
        { 
            isInit = true; 
            Init(); 
        }

        int entryButtonIndex = (int)Buttons.EntryStage1 + _index;
        int sweepButtonIndex = (int)Buttons.SweepStage1 + _index;

        Button entryButton = GetButton(entryButtonIndex);
        entryButton.gameObject.BindEvent((PointerEventData _data) => OpenEntryStagePopup(_index));

        if (g_DungeonPopupData.Data.StageCleared[_index])
        {
            Button sweepButton = GetButton(sweepButtonIndex);
            sweepButton.gameObject.BindEvent((PointerEventData _data) => OpenSweepStagePopup(_index));
        }
    }

    private void OpenEntryStagePopup(int _stageIndex)
    {
        GameManager.UI.ShowPopupUI<UI_PopUp>("DungeonEntryPopUp");
        OnSweepBtnClicked?.Invoke(_stageIndex + 1);
    }

    private void OpenSweepStagePopup(int _stageIndex)
    {
        GameManager.UI.ShowPopupUI<UI_PopUp>("DungeonSweepPopUp");
        OnSweepBtnClicked?.Invoke(_stageIndex + 1);
    }

    public void ClosePopUp(PointerEventData _data)
    {
        GameManager.UI.ClosePopUpUI();
    }
}
