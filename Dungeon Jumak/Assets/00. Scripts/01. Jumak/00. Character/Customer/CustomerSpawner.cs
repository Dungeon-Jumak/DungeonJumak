using System.ComponentModel;
using UnityEngine;
using UnityEngine.UIElements;

public class CustomerSpawner : MonoBehaviour
{   
    [Header("스폰 최소 시간")]
    [SerializeField] private float m_minTime;
    [Header("스폰 최대 시간")]
    [SerializeField] private float m_maxTime;

    [Header("스폰하는 X 좌표의 거리 절댓값")]
    [SerializeField] private int m_xPosition = 4;

    [Header("해당하는 오브젝트 풀 매니저 컴포넌트")]
    [SerializeField] private ObjectPoolManager objectPoolManager;

    private float m_timeToNextSpawn;

    private void Start()
    {
        //랜덤 시간 설정
        m_timeToNextSpawn = GetRandomTime();
    }

    private void Update()
    {
        SpawnCustomer();
    }

    /// <summary>
    /// 일정 시간마다 손님 프리팹을 오브젝트 풀링에서 가져오기 위한 메소드
    /// </summary>
    private void SpawnCustomer()
    {
        m_timeToNextSpawn -= Time.deltaTime;

        if (m_timeToNextSpawn <= 0f)
        {
            var customer = objectPoolManager.Pool.Get();                //풀링에서 오브젝트를 가져옴
            customer.transform.localPosition = GetRandomTransform();    //랜덤 위치 설정
            m_timeToNextSpawn = GetRandomTime();                        //랜덤 시간 재설정
        }
    }

    /// <summary>
    /// 랜덤 위치를 갖고 오기 위한 메소드
    /// </summary>
    /// <returns></returns>
    private Vector3 GetRandomTransform()
    {
        int randomValue = Random.Range(0, 2) == 0 ? -1 : 1;
        return new Vector3(randomValue * m_xPosition, objectPoolManager.transform.position.y, objectPoolManager.transform.position.z);
    }

    /// <summary>
    /// 랜덤 시간을 가져오기 위한 메소드
    /// </summary>
    /// <returns></returns>
    private float GetRandomTime()
    {
        return Random.Range(m_minTime, m_maxTime);
    }
}
