using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    public bool canSpawn;

    [Header("스폰 최소 시간")]
    [SerializeField] private float m_minTime;
    [Header("스폰 최대 시간")]
    [SerializeField] private float m_maxTime;

    [Header("X 시작 좌표 절댓값")]
    [SerializeField] private int m_absX;

    [Header("왼쪽 포인트")]
    [SerializeField] private Transform m_leftPoint;
    [Header("오른쪽 포인트")]
    [SerializeField] private Transform m_rightPoint;
    [Header("주막 입구 포인트")]
    [SerializeField] private Transform m_entrancePoint;

    [Header("주막에 방문할 확률")]
    [SerializeField] private int visitChance;

    [Header("해당하는 오브젝트 풀 매니저 컴포넌트")]
    [SerializeField] private ObjectPoolManager objectPoolManager;

    private float m_timeToNextSpawn;

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        if (canSpawn) SpawnCustomer();
    }

    private void Init()
    {
        canSpawn = true;
        m_timeToNextSpawn = GetRandomTime();
    }

    /// <summary>
    /// 일정 시간마다 손님 프리팹을 오브젝트 풀링에서 가져오기 위한 메소드
    /// </summary>
    private void SpawnCustomer()
    {
        m_timeToNextSpawn -= Time.deltaTime;

        if (m_timeToNextSpawn <= 0f)
        {
            var customer = objectPoolManager.Pool.Get();                                    //풀링에서 오브젝트를 가져옴
            SetCustomer(customer);
            m_timeToNextSpawn = GetRandomTime();                                            //랜덤 시간 재설정
        }
    }

    /// <summary>
    /// 풀에서 불러온 손님을 기본 세팅하기 위한 함수
    /// </summary>
    /// <param name="_customer"></param>
    private void SetCustomer(GameObject _customer)
    {
        _customer.transform.localPosition = GetRandomTransform();
        _customer.GetComponent<Customer>().destination = GetDestination(_customer);
    }

    /// <summary>
    /// 랜덤 위치를 갖고 오기 위한 메소드
    /// </summary>
    /// <returns></returns>
    private Vector3 GetRandomTransform()
    {
        int randomValue = Random.Range(0, 2) == 0 ? -1 : 1;
        return new Vector3(randomValue * m_absX, 0, 0);
    }

    /// <summary>
    /// 손님의 목적지를 설정하기 위한 메소드
    /// </summary>
    /// <param name="_gameObject"></param>
    /// <returns></returns>
    private Transform GetDestination(GameObject _customer)
    {
        var random = Random.Range(1, 101);

        if (random < visitChance)                                       //주막을 방문할 확률에 해당된다면,
        {
            _customer.GetComponent<Customer>().willVisit = true;        //주막을 방문하고 싶어하는 손님의 bool 변수 변경
            return m_entrancePoint;
        }

        if (_customer.transform.localPosition.x < 0)
            return m_rightPoint;

        return m_leftPoint;
    }

    /// <summary>
    /// 랜덤 시간을 가져오기 위한 메소드
    /// </summary>
    /// <returns></returns>
    private float GetRandomTime()
    {
        return Random.Range(m_minTime, m_maxTime);
    }

    public void StopSpawn()
    {
        canSpawn = false;
    }

    public void ResumeSpawn()
    {
        m_timeToNextSpawn = GetRandomTime();

        canSpawn = true;
    }
}
