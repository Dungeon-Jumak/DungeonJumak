// Engine
using UnityEngine;

public class DP_MoveHandler
{
    public bool isMoving = false; // 초기 상태는 멈춘 상태

    private float speed; // 이동 속도
    private float knockbackForce = 10.0f; // 넉백 힘
    private const float MIN_SAFE_DISTANCE = 5.0f; // 최소 이동 거리

    private Rigidbody2D rigidbody;
    private Scanner scanner;

    private Vector2 targetPosition; // 이동 목표 지점

    public DP_MoveHandler(Transform _transform, Rigidbody2D _rigidbody, float _speed, Scanner _scanner)
    {
        this.rigidbody = _rigidbody;
        this.speed = _speed;
        this.scanner = _scanner;
    }

    public void FixedUpdate()
    {
        // 이동 X, 이동할 위치 계산
        if (!isMoving && scanner.nearestTarget != null)
        {
            SetTargetPosition(scanner.nearestTarget);
            isMoving = true; 
        }

        // 이동
        if (isMoving)
        {
            MoveTowardsTarget();
        }
    }

    //-- 이동할 위치 계산 --//
    private void SetTargetPosition(Transform target)
    {
        Vector2 directionAwayFromTarget = (rigidbody.position - (Vector2)target.position).normalized;

        targetPosition = rigidbody.position + directionAwayFromTarget * MIN_SAFE_DISTANCE;
    }

    //-- 목표 지점으로의 이동 --//
    private void MoveTowardsTarget()
    {
        Vector2 newPosition = Vector2.MoveTowards(rigidbody.position, targetPosition, speed * Time.fixedDeltaTime);
        rigidbody.MovePosition(newPosition);

        // 목표 지점에 도달하면 이동을 멈추고, 다시 스캔 시작
        if (Vector2.Distance(rigidbody.position, targetPosition) < 0.1f)
        {
            isMoving = false;
            scanner.enabled = true;
        }
    }
}
