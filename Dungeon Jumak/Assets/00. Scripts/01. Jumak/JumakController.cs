using System.Xml.Serialization;
using UnityEngine;

public class JumakData
{
    public int maxWaitingCount = 5;
    public int maxSeatCount = 2;
}

public class JumakController : MonoBehaviour
{
    public JumakData jumakData
    {
        get 
        {
            return _jumakData;
        }
        set
        {
            _jumakData = value;
        }
    }

    private JumakData _jumakData;

    [Header("전체 자리 위치 배열")]
    [SerializeField] private Seat[] seats;

    [Header("현재 할당되어 있는 자리 배열  : 자리가 할당되어 있다면 True")]
    public bool[] allocatedSeats;

    [Header("최대 자리 개수")]
    [SerializeField] private int m_maxSeatCount;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        allocatedSeats = new bool[seats.Length];

        m_maxSeatCount = 2;
    }

    /// <summary>
    /// 비어 있는 자리가 있는지 체크하기 위한 메소드
    /// </summary>
    /// <returns></returns>
    public bool CheckEmptySeat()
    {
        for (int i = 0; i < m_maxSeatCount; i++)
        {
            if (!allocatedSeats[i]) return true;   //비어 있는 자리가 하나라도 있다면 True 반환
        }

        return false;   //비어 있는 자리가 하나도 없다면 False 반환
    }

    /// <summary>
    /// 비어있는 자리를 찾아 반환해주는 메소드
    /// </summary>
    /// <returns></returns>
    public Seat GetEmptySeat()
    {
        if (!CheckEmptySeat()) return null;

        var random = GetRandomIndex();      //비어 있는 랜덤 인덱스 값을 하나 불러옴
        allocatedSeats[random] = true;    //자리 할당
        return seats[random];             //할당된 자리 반환
    }

    public void ReturnSeat(int _index)
    {
        allocatedSeats[_index] = false;
    }

    /// <summary>
    /// 할당되지 않은 자리의 인덱스 값을 반환하는 메소드
    /// </summary>
    /// <returns></returns>
    private int GetRandomIndex()
    {
        //할당되지 않은 랜덤 변수를 뽑을 때 까지 반복
        int random;

        do
            random = Random.Range(0, m_maxSeatCount);
        while (allocatedSeats[random]);

        return random;
    }
}
