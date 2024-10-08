using UnityEngine;
using UnityEngine.UI;

public class Seat : MonoBehaviour
{
    [Header("자리 번호")]
    [SerializeField] private int seatNumber; 

    [Header("주문 말풍선이 뜨는데 걸리는 시간")]
    [SerializeField] private float bubbleStartTime = 0f; 

    private Button bubble;
    private OrderManager orderManager;

    private void Awake()
    {
        bubble = FindBubble().GetComponent<Button>();
        orderManager = FindObjectOfType<OrderManager>();
    }

    /// <summary>
    /// 외부에서 참조하기 위한 메뉴 주문 메소드
    /// </summary>
    public void SelectMenu()
    {
        Invoke("ActiveOrderBubble", bubbleStartTime);
    }

    /// <summary>
    /// 주문 말풍선을 활성화 시키기 위한 메소드
    /// </summary>
    private void ActiveOrderBubble()
    {
        bubble.gameObject.SetActive(true);

        var _orderButton = bubble.GetComponent<OrderButton>();
        orderManager.SelectMenu(_orderButton, seatNumber);
    }

    /// <summary>
    /// 같은 하이어라키 내 말풍선 오브젝트를 찾기 위한 메소드
    /// </summary>
    /// <returns></returns>
    private Transform FindBubble()
    {
        var parent = transform.parent;

        foreach (Transform p in parent)
        {
            if (p.name.Equals("[Image] Bubble")) return p;
        }

        return null;
    }

    public int GetSeatNubmer()
    {
        return seatNumber;
    }
}
