//Unity
using UnityEngine;
using UnityEngine.UI;

public class CountButton : MonoBehaviour
{
    private Button button;
    private MenuData menuData;

    private FoodOnTable foodOnTable;
    private Customer customer;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ShowCountPopup);
    }

    public void Init(MenuData _menuData, FoodOnTable _foodOnTable, Customer _customer)
    {
        menuData = _menuData;
        foodOnTable = _foodOnTable;
        customer = _customer;
    }

    private void ShowCountPopup()
    {
        //Debug
        Debug.Log("결제 금액 : " + menuData.prices[menuData.level - 1]);

        //계산 전표 출력
        Sales_Slip slip = GameManager.UI.ShowPopupUI<Sales_Slip>("Sales_Slip");
        slip.InitSlip(menuData, this);

    }

    public void Count(bool _double = false)
    {
        customer.ExitJumak();

        //아래 돈 증가 코드 추가

        foodOnTable.ActivateCleaningButton();
        gameObject.SetActive(false);
    }
}
