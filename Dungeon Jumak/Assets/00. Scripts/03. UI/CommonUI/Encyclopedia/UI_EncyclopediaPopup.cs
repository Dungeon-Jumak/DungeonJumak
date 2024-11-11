using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_EncyclopediaPopup : UI_PopUp
{
    enum Buttons
    {
        Dim,
    }

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
    }

    public void ClosePopUp(PointerEventData _data)
    {
        GameManager.UI.ClosePopUpUI();
    }
}
