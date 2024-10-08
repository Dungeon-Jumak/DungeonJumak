//Unity
using UnityEngine;
using UnityEngine.UI;

public class CleaningButton : MonoBehaviour
{
    private Button button;

    private FoodOnTable foodOnTable;

    private EntranceController entranceController;

    private void Awake()
    {
        entranceController = FindObjectOfType<EntranceController>();

        button = GetComponent<Button>();
        button.onClick.AddListener(Cleaning);
    }

    public void Init(FoodOnTable _foodOnTable)
    {
        foodOnTable = _foodOnTable;
    }

    private void Cleaning()
    {
        foodOnTable.gameObject.SetActive(false);

        entranceController.EntranceJumakInWatingQueue(foodOnTable.GetTableNumber());

        gameObject.SetActive(false);
    }
}
