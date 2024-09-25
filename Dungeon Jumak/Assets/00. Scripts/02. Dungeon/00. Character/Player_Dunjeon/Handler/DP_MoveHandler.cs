// Engine
using UnityEngine;
using UnityEngine.EventSystems;

public class DP_MoveHandler
{
    public bool isMoving = false;

    // 이동 속도
    [SerializeField] private float speed;

    // 플레이어 트랜스폼
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

        if( isMoving )
        {
            // 스캔 범위 내에 타겟이 있는지 확인
            if (scanner.nearestTarget != null)
            {
                // 타겟 존재
                MoveAwayFromTarget(scanner.nearestTarget);
            }
            else
            {
                // 타겟 존재하지 않음
                StopMoving();
            }
        }
    }

    public void Moving()
    {
    }

    private void StopMoving()
    {
        // 멈추기
    }

    private void MoveAwayFromTarget(Transform target)
    {
        if (target != null)
        {
            // 타겟과 반대 방향을 계산
            Vector3 directionAwayFromTarget = (playerTransform.position - target.position).normalized;

            // 플레이어 이동
            playerTransform.Translate(directionAwayFromTarget * speed * Time.deltaTime);
        }
    }
}
