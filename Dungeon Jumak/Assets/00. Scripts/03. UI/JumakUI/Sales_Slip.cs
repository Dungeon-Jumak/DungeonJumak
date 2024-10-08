using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class Sales_Slip : UI_PopUp
{
    private MenuData menuData;
    private CountButton countButton;

    enum Buttons
    {
        AD,
        Yeouiju,
        Little_Bit,
    }

    enum TMPs 
    {
        Jumak_Name,
        Food_Name,
        Count,
        Price,
    }

    private void Awake()
    {
        Bind<Button>(typeof(Buttons));
        Bind<TextMeshProUGUI>(typeof(TMPs));

        GetButton((int)Buttons.AD).gameObject.BindEvent(ReceiveDoubleFromAD);
        GetButton((int)Buttons.Yeouiju).gameObject.BindEvent(ReceiveDoubleFromYeouiju);
        GetButton((int)Buttons.Little_Bit).gameObject.BindEvent(ReceiveLittle);
    }

    public void InitSlip(MenuData _menuData, CountButton _countButton)
    {
        menuData = _menuData;
        countButton = _countButton;

        //GetTMP((int)TMPs.Jumak_Name).text = 주막 이름;
        GetTMP((int)TMPs.Food_Name).text = menuData._name;
        GetTMP((int)TMPs.Count).text = menuData.count.ToString();
        GetTMP((int)TMPs.Price).text = menuData.prices[menuData.level - 1].ToString();
    }

    private void ReceiveLittle(PointerEventData _data)
    {
        countButton.Count(false);
        GameManager.UI.ClosePopUpUI();
    }

    private void ReceiveDoubleFromAD(PointerEventData _data)
    {
        //광고를 통해 2배 증가
        countButton.Count(true);
        GameManager.UI.ClosePopUpUI();
    }

    private void ReceiveDoubleFromYeouiju(PointerEventData _data)
    {
        //여의주를 통해 2배 증가
        countButton.Count(true);
        GameManager.UI.ClosePopUpUI();
    }

}
