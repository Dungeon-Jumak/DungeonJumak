using UnityEngine;
using UnityEngine.UI;

public class Food : MonoBehaviour
{
    public MenuData menuData;
    public int seatNumber;

    private ServingTable servingTable;
    private Button button;

    private void Awake()
    {
        servingTable = GetComponentInParent<ServingTable>();
        button = GetComponent<Button>();

        button.onClick.AddListener(Serving);
    }

    private void Serving()
    {
        servingTable.ServingFood(this, menuData);
    }


}
