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
        button.onClick.AddListener(Count);
    }

    public void Init(MenuData _menuData, FoodOnTable _foodOnTable, Customer _customer)
    {
        menuData = _menuData;
        foodOnTable = _foodOnTable;
        customer = _customer;
    }

    private void Count()
    {
        //Debug
        Debug.Log("결제 금액 : " + menuData.prices[menuData.level - 1]);

        //아래 돈 증가 코드 추가

        //손님 퇴장 코드 추가
        customer.ExitJumak();

        foodOnTable.ActivateCleaningButton();
        gameObject.SetActive(false);
    }
}
