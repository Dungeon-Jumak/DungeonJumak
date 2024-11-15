using System.Timers;

//Unity
using UnityEngine;
using UnityEngine.UI;

public class CleaningButton : MonoBehaviour
{
    private Button button;

    private FoodOnTable foodOnTable;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(Cleaning);
    }

    public void Init(FoodOnTable _foodOnTable)
    {
        foodOnTable = _foodOnTable;
    }

    private void Cleaning()
    {
        //음식 비활성화
        foodOnTable.gameObject.SetActive(false);

        foodOnTable.Clean();

        gameObject.SetActive(false);
    }
}
