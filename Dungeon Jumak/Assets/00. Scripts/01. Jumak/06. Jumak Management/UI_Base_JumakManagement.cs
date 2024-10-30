using System.Reflection;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Base_JumakManagement : UI_PopUp
{
    private const string VILLAGE = "JumakManager_Village";
    private const string HALL = "JumakManager_Hall";
    private const string KITCHEN = "JumakManager_Kitchen";
    private const string EMPLOYEE = "JumakManager_Employee";
    private const string MENU = "JumakManager_Menu";

    protected void ClosePopUp(PointerEventData _data)
    {
        GameManager.UI.ClosePopUpUI();
    }

    #region For Change Categories
    protected void ChangeCategoryToVillage(PointerEventData _data)
    {
        GameManager.UI.ClosePopUpUI();
        GameManager.UI.ShowPopupUI<UI_PopUp>(VILLAGE);
    }
    protected void ChangeCategoryToHall(PointerEventData _data)
    {
        GameManager.UI.ClosePopUpUI();
        GameManager.UI.ShowPopupUI<UI_PopUp>(HALL);
    }
    protected void ChangeCategoryToKitchen(PointerEventData _data)
    {
        GameManager.UI.ClosePopUpUI();
        GameManager.UI.ShowPopupUI<UI_PopUp>(KITCHEN);
    }
    protected void ChangeCategoryToEmployee(PointerEventData _data)
    {
        GameManager.UI.ClosePopUpUI();
        GameManager.UI.ShowPopupUI<UI_PopUp>(EMPLOYEE);
    }
    protected void ChangeCategoryToMenu(PointerEventData _data)
    {
        GameManager.UI.ClosePopUpUI();
        GameManager.UI.ShowPopupUI<UI_PopUp>(MENU);
    }
    #endregion
}
