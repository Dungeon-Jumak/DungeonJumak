// Engine
using UnityEngine;
using UnityEngine.EventSystems;

public class DP_MoveHandler
{
    public bool isMoving = false;

    // 이동 속도
    [SerializeField] private float speed;

    // 트랜스폼
    private Transform playerTransform;

    // 스캐너
    private Scanner scanner;

    public DP_MoveHandler(Transform _playerTransform = null, float _speed = 0f, Scanner _scanner = null)
    {
        this.playerTransform = _playerTransform;
        this.speed = _speed;
        this.scanner = _scanner;
    }

    private void Update() {

        Moving();
    }

    public void Moving()
    {
        // 스캔 범위 내에 타겟이 있는지 확인
        if (scanner.nearestTarget != null)
        {
            MoveAwayFromTarget(scanner.nearestTarget);
        }
        else
        {
            StopMoving();
        }
    }

    private void StopMoving()
    {
        // 멈추기
    }

    private void MoveAwayFromTarget(Transform target)
    {
        if (target != null)
        {
            // 반대 방향 계산
            Vector3 directionAwayFromTarget = (playerTransform.position - target.position).normalized;

            // 이동
            playerTransform.Translate(directionAwayFromTarget * speed * Time.deltaTime);
        }
    }
}
