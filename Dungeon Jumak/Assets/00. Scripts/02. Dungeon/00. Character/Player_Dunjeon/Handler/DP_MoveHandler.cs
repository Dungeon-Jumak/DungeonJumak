// Engine
using UnityEngine;

public class DP_MoveHandler
{
    public bool isMoving = false;

    private float speed;
    private const float MIN_SAFE_DISTANCE = 5.0f;
    private Rigidbody2D rigidbody;
    private Scanner scanner;
    private LayerMask movementLayer; // 이동용 레이어 마스크
    private Vector2 targetPosition;

    public DP_MoveHandler(Transform _transform, Rigidbody2D _rigidbody, float _speed, Scanner _scanner, LayerMask _movementLayer)
    {
        this.rigidbody = _rigidbody;
        this.speed = _speed;
        this.scanner = _scanner;
        this.movementLayer = _movementLayer;
    }

    public void FixedUpdate()
    {
        Transform target = scanner.GetNearestTarget(movementLayer); // 이동용 타겟 필터링

        if (!isMoving && target != null)
        {
            SetTargetPosition(target);
            isMoving = true;
        }

        if (isMoving)
        {
            MoveTowardsTarget();
        }
    }

    private void SetTargetPosition(Transform target)
    {
        Vector2 directionAwayFromTarget = (rigidbody.position - (Vector2)target.position).normalized;
        targetPosition = rigidbody.position + directionAwayFromTarget * MIN_SAFE_DISTANCE;
    }

    private void MoveTowardsTarget()
    {
        Vector2 newPosition = Vector2.MoveTowards(rigidbody.position, targetPosition, speed * Time.fixedDeltaTime);
        rigidbody.MovePosition(newPosition);

        if (Vector2.Distance(rigidbody.position, targetPosition) < 0.1f)
        {
            isMoving = false;
        }
    }
}
