//Unity
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 식사, 계산, 청소의 전반적인 것들을 관리
/// </summary>
public class FoodOnTable : MonoBehaviour
{
    [SerializeField] private Table table;

    [Header("테이블 위 음식 자리 번호")]
    [SerializeField] private int tableNumber;

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

    private EntranceController entranceController;

    private JumakDataManager jumakDataManager;

    private void Awake()
    {
        table = transform.parent.GetComponent<Table>();

        startEat = false;
        image = GetComponent<Image>();

        customerTempParent = GameObject.Find("Customer's Temp Parent").transform;
        entranceController = FindObjectOfType<EntranceController>();
        jumakDataManager = FindObjectOfType<JumakDataManager>();
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
            if (startEatingTime + jumakDataManager.GetEatingTime(table.tableID) < Time.time)
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

    public void Clean()
    {
        Invoke("FinishClean", DataManager<JumakData>.Instance.Data.cleaningTime);
    }

    public void FinishClean()
    {
        entranceController.EntranceJumakInWatingQueue(GetTableNumber());
    }

    public int GetTableNumber()
    {
        return tableNumber;
    }
}
