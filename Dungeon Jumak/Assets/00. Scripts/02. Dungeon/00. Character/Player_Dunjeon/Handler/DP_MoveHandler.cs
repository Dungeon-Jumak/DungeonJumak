// Engine
using UnityEngine;

public class DP_MoveHandler
{
    public bool isMoving = false;

    // 이동 속도
    [SerializeField] private float speed;

    // 플레이어 트랜스폼
    private Rigidbody2D playerRigidbody; // Rigidbody2D 사용

    // 스캐너
    private Scanner scanner;

    // 스캔 범위
    private float scanRange;

    // 목표 지점
    private Vector2 targetPosition;

    public DP_MoveHandler(Rigidbody2D _playerRigidbody, float _speed, Scanner _scanner)
    {
        if (_playerRigidbody == null || _scanner == null)
        {
            Debug.LogError("playerRigidbody 또는 scanner가 null입니다. 초기화 확인 필요.");
            return;
        }

        this.playerRigidbody = _playerRigidbody;
        this.speed = _speed;
        this.scanner = _scanner;
        this.scanRange = _scanner.scanRange; // 스캔 범위 초기화
    }

    public void Update()
    {
        if (isMoving)
        {
            MoveTowardsTarget(); // 목표 지점으로 이동 중
        }
        else if (scanner.nearestTarget != null)
        {
            // 새로운 몬스터를 스캔했을 때만 실행
            SetTargetPosition(scanner.nearestTarget);
        }
    }

    private void SetTargetPosition(Transform target)
    {
        if (target == null)
        {
            Debug.LogError("Target is null. Unable to set target position.");
            return;
        }

        isMoving = true; // 이동 시작

        // 타겟으로부터 반대 방향 계산
        Vector2 directionAwayFromTarget = (playerRigidbody.position - (Vector2)target.position).normalized;

        // 목표 위치: 플레이어 위치에서 스캔 범위만큼 반대 방향으로 설정
        targetPosition = playerRigidbody.position + directionAwayFromTarget * scanRange;

        // 스캔을 잠시 중지
        scanner.enabled = false;
    }

    private void MoveTowardsTarget()
    {
        // 목표 지점으로 부드럽게 이동 (Rigidbody2D 사용)
        Vector2 newPosition = Vector2.MoveTowards(
            playerRigidbody.position,  // 현재 위치
            targetPosition,            // 목표 위치 (스캔 범위만큼 반대 방향)
            speed * Time.fixedDeltaTime // 속도
        );

        // Rigidbody2D를 통해 이동
        playerRigidbody.MovePosition(newPosition);

        // 목표 지점에 도착했는지 확인
        if (Vector2.Distance(playerRigidbody.position, targetPosition) < 0.1f)
        {
            StopMoving();
        }
    }

    private void StopMoving()
    {
        isMoving = false; // 이동 종료

        // 스캐너 다시 활성화
        scanner.enabled = true;
    }
}
