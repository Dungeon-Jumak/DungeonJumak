using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class OrderButton : MonoBehaviour
{
    private OrderButton button;
    private OrderManager orderManager;

    private MenuData menuData;
    private int seatNumber;

    private void Awake()
    {
        button = GetComponent<OrderButton>();
        orderManager = FindObjectOfType<OrderManager>();

        button.GetComponent<Button>().onClick.AddListener(Order);
    }

    private void Order()
    {
        orderManager.Order(gameObject, menuData, seatNumber);
    }
    
    public void Init(MenuData _menuData, int _seatNumber)
    {
        menuData = _menuData;
        seatNumber = _seatNumber;
    }
}
