using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class DungeonClosePopup : UI_PopUp
{
    enum Buttons { Dim, Cancel }

    private void Awake()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();
        Bind<Button>(typeof(Buttons));

        GetButton((int)Buttons.Dim).gameObject.BindEvent(ClosePopUp);
        GetButton((int)Buttons.Cancel).gameObject.BindEvent(ClosePopUp);
    }

    public void ClosePopUp(PointerEventData _data)
    {
        GameManager.UI.ClosePopUpUI();
    }
}
