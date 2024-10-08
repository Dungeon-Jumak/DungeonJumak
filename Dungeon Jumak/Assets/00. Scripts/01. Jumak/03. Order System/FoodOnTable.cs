//Unity
using UnityEngine;
using UnityEngine.UI;

public class FoodOnTable : MonoBehaviour
{
    [Header("테이블 위 음식 자리 번호")]
    [SerializeField] private int tableNumber;

    //후에 프로퍼티로 변경 - 테이블 단계에 맞게 변경되도록
    [Header("음식 먹는데 걸리는 시간")]
    public float eatingTime = 5f;

    [Header("계산 버튼")]
    [SerializeField] private CountButton countButton;

    [Header("청소 버튼")]
    [SerializeField] private CleaningButton cleaningButton;

    private MenuData menuData;
    private Image image;

    private float startEatingTime;
    private bool startEat;

    private Transform customerTempParent;

    private Customer customer;

    private void Awake()
    {
        startEat = false;
        image = GetComponent<Image>();

        customerTempParent = GameObject.Find("Customer's Temp Parent").transform;
    }

    private void OnEnable()
    {
        startEat = true;
        startEatingTime = Time.time;

        customer = FindCustomer();

        customer.StartEat();
    }

    private void Update()
    {
        EatFood();
    }

    public void Init(MenuData _menuData)
    {
        menuData = _menuData;
        image.sprite = menuData.sprite;
    }

    private void EatFood()
    {
        if (startEat)
        {
            if (startEatingTime + eatingTime < Time.time)
            {
                startEat = false;
                image.sprite = menuData.emptySprite;

                customer.FinishEat();

                ActivateCountButton();
            }
        }
    }


    /// <summary>
    /// 현재 테이블에 앉아있는 손님을 찾기 위한 메소드
    /// </summary>
    private Customer FindCustomer()
    {
        Customer[] customer = customerTempParent.GetComponentsInChildren<Customer>();

        foreach (Customer c in customer)
        {
            if (c.cuurentSeatNubmer == tableNumber) return c;
        }

        Debug.LogError("현재 테이블에 앉아 있는 손님을 찾지 못했습니다.");
        return null;
    }

    /// <summary>
    /// 계산 버튼 활성화를 위한 메소드
    /// </summary>
    private void ActivateCountButton()
    {
        countButton.gameObject.SetActive(true);

        countButton.Init(menuData, this, customer);
    }

    /// <summary>
    /// 청소 메소드
    /// </summary>
    public void ActivateCleaningButton()
    {
        cleaningButton.gameObject.SetActive(true);

        cleaningButton.Init(this);
    }

    public int GetTableNumber()
    {
        return tableNumber;
    }
}
