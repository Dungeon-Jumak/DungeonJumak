using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDungeon : MonoBehaviour
{
    public void Onclick()
    {
        GameManager.UI.ShowPopupUI<UI_PopUp>("DungeonRetryPopUp");
    }

    public void Onclick1()
    {
        GameManager.UI.ShowPopupUI<UI_PopUp>("DungeonClearPopUp");
    }

    public void Onclick2()
    {
        GameManager.UI.ShowPopupUI<UI_PopUp>("DungeonGameOverPopUp");
    }
}
