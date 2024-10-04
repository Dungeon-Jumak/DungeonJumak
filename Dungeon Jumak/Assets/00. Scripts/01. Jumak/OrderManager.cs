using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class OrderManager : MonoBehaviour
{
    [Header("화구 배열 (메인, 사이드, 음료)")]
    [SerializeField] private Furnace[] furnaces;

    [Header("메뉴 데이터 배열")]
    [SerializeField] private MenuData[] menuDatas;

    private const string alert = "CantOrderAlert";

    /// <summary>
    /// 자리에 앉았을 때 메뉴를 선택하기 위한 메소드
    /// </summary>
    public void SelectMenu(OrderButton _orderButton, int _seatNumber)
    {
        MenuData menu = GetRandomMenu();

        _orderButton.Init(menu, _seatNumber);
    }

    private MenuData GetRandomMenu()
    {
        var random = Random.Range(0, menuDatas.Length);

        return menuDatas[random];
    }

    public void Order(GameObject _gameObject, MenuData menuData, int _seatNumber)
    {
        if (!furnaces[menuData.category].CanCooking())
        {
            GameManager.UI.CloseAllPopUpUI();
            GameManager.UI.ShowPopupUI<UI_PopUp>(alert);
            return;
        }

        _gameObject.SetActive(false);
        furnaces[menuData.category].StartCook(menuData, _seatNumber); 
    }
}
