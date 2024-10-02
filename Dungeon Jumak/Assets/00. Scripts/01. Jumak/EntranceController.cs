//System
using System.Collections.Generic;

//Unity
using UnityEngine;

public class EntranceController : MonoBehaviour
{
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

    private Queue<Customer> m_waitingQueue;

    [Header("기다리는 포인트들의 Transform 배열")]
    [SerializeField] private Transform[] m_waitingTransform;

    private int m_currentWaitingCount;

    //Temp
    JumakData jumakData;

    private const int MIN_WAITING_COUNT = 0;

    private void Awake()
    {
        m_waitingQueue = new Queue<Customer>();

        //Temp
        jumakData = new JumakData();
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

        customer.destination = m_waitingTransform[m_currentWaitingCount];
        IncreaseWaitingCount();
        m_waitingQueue.Enqueue(customer);
    }

    /// <summary>
    /// 입구에서 주막으로 이동하기 위한 메소드
    /// </summary>
    /// <param name="_customer"></param>
    public void EntranceToJumak(Customer _customer)
    {
        _customer.transform.SetParent(m_tempParent);
        _customer.transform.position = ConvertRectTransfromToTransform(m_entrancePoint);

        SetDestinatonToEmpty(_customer);
    }


    /// <summary>
    /// Customer의 Destination을 주막 내 빈 자리로 설정하기 위한 메소드
    /// </summary>
    /// <param name="_customer"></param>
    private void SetDestinatonToEmpty(Customer _customer)
    {
        _customer.destination = m_jumakController.GetEmptySeat();
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
