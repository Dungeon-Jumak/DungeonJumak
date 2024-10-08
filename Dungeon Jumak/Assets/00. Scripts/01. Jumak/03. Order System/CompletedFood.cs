using UnityEngine;
using UnityEngine.UI;

public class CompletedFood : MonoBehaviour
{
    [SerializeField] private Button doneCookingButton;

    private Furnace furnace;    //본인 부모의 Furnace 컴포넌트
    private Button button;

    private int seatNumber;
    private MenuData menuData;

    private ServingTable servingTable;

    private const string alert = "CantTransferFoodToServingTableAlert";

    private void Awake()
    {
        furnace = GetComponentInParent<Furnace>();
        button = GetComponent<Button>();

        button.onClick.AddListener(TransferFoodtoServingTable);
        doneCookingButton.onClick.AddListener(MoveKitchen);

        servingTable = FindObjectOfType<ServingTable>();
    }

    private void Update()
    {
        CheckCurrentScene();
    }

    public void Init(MenuData _menuData, int _seatNumber)
    {
        menuData = _menuData;
        seatNumber = _seatNumber;
    }

    private void TransferFoodtoServingTable()
    {
        if (servingTable.IsFull())
        {
            GameManager.UI.ShowPopupUI<UI_PopUp>(alert);
            return;
        }

        furnace.SetCanFood(true);

        gameObject.SetActive(false);
        servingTable.AddFoodOnServingTable(menuData, seatNumber);
    }

    private void MoveKitchen()
    {
        furnace.snap.PreviousScreen();
    }

    private void CheckCurrentScene()
    {
        if (furnace.snap.CurrentPage == 1) doneCookingButton.gameObject.SetActive(true);
        else doneCookingButton.gameObject.SetActive(false);
    }

}
