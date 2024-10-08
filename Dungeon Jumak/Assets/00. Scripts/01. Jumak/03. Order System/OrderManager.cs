using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class OrderManager : MonoBehaviour
{
    [Header("화구 배열 (메인, 사이드, 음료)")]
    [SerializeField] private Furnace[] furnaces;

    [Header("모든 메뉴 데이터 배열")]
    [SerializeField] private MenuData[] allMenuDatas;

    [Header("주문 가능한 메뉴를 저장할 리스트")]
    [SerializeField] private List<MenuData> menuList;

    [Header("Ingredient Manager 컴포넌트")]
    [SerializeField] private IngredientManager ingredientManager;

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
        //메뉴 리스트 업데이트
        UpdateCanCookMenuList();

        //랜덤 인덱스를 불러옴
        var random = Random.Range(0, menuList.Count);

        //Debug
        Debug.Log(menuList[random]._name + " 하나 주세요~");

        //해당 메뉴 데이터에 해당하는 재료를 모두 1만큼 감소
        for (int i = 0; i < menuList[random].ingredients.Length; i++)
        {
            ingredientManager.SubIngredientCount(menuList[random].ingredients[i], 1);
        }

        //선택된 메뉴 데이터 값 리턴
        return menuList[random];
    }

    /// <summary>
    /// 요리할 수 있는 메뉴 리스트 추가
    /// </summary>
    private void UpdateCanCookMenuList()
    {
        //리스트 초기화
        menuList.Clear();

        //리스트 초기화 - 모든 메뉴를 갖고 있도록
        for (int i = 0; i < allMenuDatas.Length; i++)
        {
            Debug.Log("반복");
            menuList.Add(allMenuDatas[i]);
        }
        
        //현재 재료 상태를 불러올 딕셔너리
        Dictionary<IngredientData, int> dictionary = ingredientManager.GetIngredientDictionary();

        //Base Gukbab 을 제외한 1번 메뉴부터 검사
        for (int i = 1; i < allMenuDatas.Length; i++)
        {
            for (int j = 0; j < allMenuDatas[i].ingredients.Length; j++)
            {
                //만약 재료가 1개라도 부족하다면
                if (dictionary[allMenuDatas[i].ingredients[j]] == 0)
                    //재료가 부족할 경우 그 음식은 리스트에서 제외
                    menuList.Remove(allMenuDatas[i]);
            }
        }
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

    public MenuData[] GetMenuDatas()
    {
        return allMenuDatas;
    }
}
