using UnityEngine;
using UnityEngine.UI;

public class ServingTable : MonoBehaviour
{
    [Header("현재 서빙 테이블 위에 있는 음식을 저장할 배열")]
    [SerializeField] private Food[] foods;

    [Header("테이블 위에 있는 음식 이미지를 저장할 배열")]
    [SerializeField] private Image[] foodsOnTable;

    private const int START_INDEX = 0;

    public void ServingFood(Food _food)
    {
        for (int i = START_INDEX; i < foods.Length; i++)
        {
            if (_food == foods[i])
            {
                foods[i].gameObject.SetActive(false);

                foodsOnTable[_food.seatNumber].gameObject.SetActive(true);
                foodsOnTable[_food.seatNumber].sprite = _food.menuData.sprite;
            }

        }
    }

    public void AddFoodOnServingTable(MenuData _menuData, int _seatNumber)
    {
        for (int i = START_INDEX; i < foods.Length; i++)
        {
            if (!foods[i].gameObject.activeSelf)
            {
                foods[i].GetComponent<Image>().sprite = _menuData.sprite;
                foods[i].gameObject.SetActive(true);

                foods[i].menuData = _menuData;
                foods[i].seatNumber = _seatNumber;

                return;
            }
        }
    }

    public bool IsFull()
    {
        for (int i = START_INDEX; i < foods.Length; i++)
        {
            if (!foods[i].gameObject.activeSelf) return false;
        }

        return true;
    }

}
