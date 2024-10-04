//System
using System.Collections.Generic;

//Unity
using UnityEngine;

public class EntranceController : MonoBehaviour
{
    [Header("왼쪽 포인트")]
    public Transform leftPoint;

    [Header("오른쪽 포인트")]
    public Transform rightPoint;

    [Header("주막 입구 포인트")]
    public Transform entrancePoint;

    [Header("주막에서 밖으로 나갈 때 지점")]
    public Transform exitPoint;

    [Header("손님 스폰을 관리하는 CustomerSpawner 스크립트")]
    [SerializeField] private CustomerSpawner m_customerSpawner;

    [Header("주막을 관리하는 주막 Controller 스크립트")]
    [SerializeField] private JumakController m_jumakController;

    [Header("주막으로 들어왔을 때 시작지점")]
    [SerializeField] private RectTransform m_entrancePoint;

    [Header("이동할 캔버스의 렌더링 카메라")]
    [SerializeField] private Camera m_camera;

    [Header("손님이 주막에 있는동안 갖고 있을 부모 오브젝트")]
    [SerializeField] private Transform m_tempParent;

    private CustomerSpawner customerSpawner;

    private Queue<Customer> m_waitingQueue;

    [Header("기다리는 포인트들의 Transform 배열")]
    [SerializeField] private Transform[] m_waitingTransform;

    [SerializeField] private int m_currentWaitingCount;

    //Temp
    JumakData jumakData;

    private const int MIN_WAITING_COUNT = 0;

    private void Awake()
    {
        m_waitingQueue = new Queue<Customer>();

        //Temp
        jumakData = new JumakData();

        customerSpawner = FindObjectOfType<CustomerSpawner>();
    }

    private void Update()
    {
        if (CheckWaitingCountIsFull()) m_customerSpawner.canSpawn = false;
        else m_customerSpawner.canSpawn = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Customer")) return;

        var customer = collision.GetComponent<Customer>();

        if (customer == null) Debug.LogError("충돌한 오브젝트에서 Customer 컴포넌트가 감지되지 않음");


        if (m_jumakController.CheckEmptySeat()) return;

        //자리가 안비어있다면 대기 설정
        WaitingInQueue(customer);
    }

    private void WaitingInQueue(Customer _customer)
    {
        _customer.destination = m_waitingTransform[m_currentWaitingCount];
        IncreaseWaitingCount();
        m_waitingQueue.Enqueue(_customer);
    }

    /// <summary>
    /// 마을에서 주막으로 이동하기 위한 메소드
    /// </summary>
    /// <param name="_customer"></param>
    public void VillageToJumak(Customer _customer)
    {
        _customer.transform.SetParent(m_tempParent);
        _customer.transform.position = ConvertRectTransfromToTransform(m_entrancePoint);

        SetDestinatonToEmpty(_customer);
    }

    /// <summary>
    /// 주막에서 마을로 이동하기 위한 메소드
    /// </summary>
    /// <param name="_customer"></param>
    public void JumakToVillage(Customer _customer)
    {
        _customer.transform.SetParent(customerSpawner.transform);
        _customer.transform.position = entrancePoint.position;

        SetDestinationToRandomTerminal(_customer);
    }

    /// <summary>
    /// 대기 중 자리가 생겼을 때 주막으로 이동하기 위한 메소드
    /// </summary>
    public void EntranceJumakInWatingQueue(int _tableNumber)
    {
        //큐가 비어 있다면 리턴
        if (m_waitingQueue.Count == 0) return;

        Customer customer = m_waitingQueue.Dequeue();

        m_jumakController.allocatedSeats[_tableNumber] = false;

        customer.EntranceJumak();

        ReadjustWaitingQueue();
    }

    /// <summary>
    /// 줄을 앞으로 당기기 위한 메소드
    /// </summary>
    private void ReadjustWaitingQueue()
    {
        Customer[] customers = m_waitingQueue.ToArray();

        int currentQueueCount = m_waitingQueue.Count;

        for (int i = 0; i < currentQueueCount; i++)
        {
            customers[i].GoForwardInWaitingQueue(m_waitingTransform[i]);
        }

        DecreaseWaitingCount();
    }


    /// <summary>
    /// Customer의 Destination을 주막 내 빈 자리로 설정하기 위한 메소드
    /// </summary>
    /// <param name="_customer"></param>
    private void SetDestinatonToEmpty(Customer _customer)
    {
        Seat seat = m_jumakController.GetEmptySeat();

        if (seat == null) WaitingInQueue(_customer);
        else
        {
            _customer.destination = seat.transform;
            _customer.cuurentSeatNubmer = seat.GetSeatNubmer();
        }
    }

    /// <summary>
    /// Customer가 주막에서 나온 후 랜덤으로 터미널 포인트를 결정하기 위한 함수
    /// </summary>
    /// <param name="_customer"></param>
    private void SetDestinationToRandomTerminal(Customer _customer)
    {
        int random = Random.Range(0, 2);

        if (random == 0)
            _customer.destination = leftPoint;
        else
            _customer.destination = rightPoint;
    }

    /// <summary>
    /// 캔버스 위 RectTransform을 Transform으로 변환하기 위한 메소드
    /// </summary>
    /// <param name="_rectTransform"></param>
    /// <returns></returns>
    private Vector3 ConvertRectTransfromToTransform(RectTransform _rectTransform)
    {
       //스크린 포인트 변환
        Vector3 screenPos = RectTransformUtility.WorldToScreenPoint(m_camera, _rectTransform.position);

        Vector3 worldPos;

        //Transform 좌표로 변환
        RectTransformUtility.ScreenPointToWorldPointInRectangle(_rectTransform, screenPos, m_camera, out worldPos);

        return worldPos;
    }

    private void IncreaseWaitingCount()
    {
        if (m_currentWaitingCount >= jumakData.maxWaitingCount) return;

        m_currentWaitingCount++;
    }

    private void DecreaseWaitingCount()
    {
        if (m_currentWaitingCount <= MIN_WAITING_COUNT) return;

        m_currentWaitingCount--;
    }

    private bool CheckWaitingCountIsFull()
    {
        if (m_currentWaitingCount == jumakData.maxWaitingCount) return true;
        else return false;
    }
}
