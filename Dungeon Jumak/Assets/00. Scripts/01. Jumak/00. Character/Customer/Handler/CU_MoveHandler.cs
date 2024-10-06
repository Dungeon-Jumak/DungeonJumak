//Engine
using UnityEngine;

public class CU_MoveHandler
{
    private float m_speed;
    private float m_currentX;
    private float m_lastX;

    private SpriteRenderer m_spriteRenderer;
    private Customer m_customer;
    private Pool m_pool;

    public CU_MoveHandler (float _speed, float _currentX, float _lastX, SpriteRenderer _spriteRenderer, Customer _customer, Pool pool)
    {
        m_speed = _speed;
        m_currentX = _currentX;
        m_lastX = _lastX;

        m_spriteRenderer = _spriteRenderer;
        m_customer = _customer;
        m_pool = pool;
    }

    /// <summary>
    /// 손님이 움직이기 위한 기본 Move 메소드
    /// </summary>
    /// <param name="_transform"></param>
    /// <param name="_destination"></param>
    /// <param name="_canMove"></param>
    public void Move(Transform _transform, Transform _destination, bool _canMove)
    {
        if (!_canMove) return;

        var dir = _destination.position - _transform.position;
        _transform.position += dir.normalized * Time.deltaTime * m_speed;
    }

    /// <summary>
    /// 손님의 방향에 따라 Flip을 변화시키기 위한 메소드
    /// </summary>
    /// <param name="_transform"></param>
    public void Turn(Transform _transform, bool _canMove = false)
    {
        if (!_canMove) return;

        m_currentX = _transform.localPosition.x;

        if (m_currentX > m_lastX && !m_spriteRenderer.flipX)
            LookRight();
        else if (m_currentX < m_lastX && m_spriteRenderer.flipX)
            LookLeft();

        m_lastX = m_currentX;
    }

    private void LookRight()
    {
        m_spriteRenderer.flipX = true;
    }

    private void LookLeft()
    {
        m_spriteRenderer.flipX = false;
    }

    /// <summary>
    /// 현재 손님이 향하고 있는 목적지에 도착 여부를 판단하기 위한 메소드
    /// </summary>
    /// <param name="_transform"></param>
    /// <param name="_destination"></param>
    /// <param name="_willVisit"></param>
    public void CheckDestination(Transform _transform, Transform _destination, bool _canMove = false)
    {
        if (!_canMove) return;

        switch (_destination.tag)
        {
            case "Terminal Point":
                ArriveTerminalPoint(_transform, _destination);
                break;

            case "Seat Point_Left":
                ArriveSeatPoint(_transform, _destination, "Left");
                break;

            case "Seat Point_Right":
                ArriveSeatPoint(_transform, _destination, "Right");
                break;

            case "Waiting Point":
                ArriveWaitingPoint(_transform, _destination);   
                break;

            default:
                break;
        }
    }

    private void ArriveTerminalPoint(Transform _transform, Transform _destination)
    {
        if (Vector3.Distance(_transform.localPosition, _destination.localPosition) <= 0.1f)
        {
            //가게에 방문할 생각이 없다면 목적지에 도달했을 때 초기화 시키고 풀에 반환
            m_customer.Init();
            m_pool.ObjectPool.Release(_transform.gameObject);
        }
    }

    private void ArriveSeatPoint(Transform _transform, Transform _destination, string _direction)
    {
        if (Vector3.Distance(_transform.position, _destination.position) <= 0.1f)
        {
            m_customer.canMove = false;
            _transform.position = _destination.position;

            m_customer.isSit = true;

            if (_direction.Equals("Left")) LookRight();
            else if (_direction.Equals("Right")) LookLeft();

            var seat = _destination.GetComponent<Seat>();

            if (seat == null) Debug.LogError("Can not load Seat Component");

            seat.SelectMenu();
        }
    }

    private void ArriveWaitingPoint(Transform _transform, Transform _destination)
    {
        if (Vector3.Distance(_transform.position, _destination.position) <= 0.1f)
        {
            m_customer.canMove = false;
            LookLeft();
            _transform.position = _destination.position;
        }
    }
}
