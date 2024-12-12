// Engine
using UnityEngine;
using Utils.EnumTypes;

public class Mo_MoveHandler
{
    #region Variables

    private float speed;
    private Transform monsterTransform;
    private Transform playerTransform;
    private Rigidbody2D monsterRigidbody; // Rigidbody2D 추가

    // 대기 시간 관련 변수
    private float waitTime = 1f;
    private float waitTimer = 0f;
    private bool isWaiting = false;

    #endregion

    public Mo_MoveHandler(Transform _monsterTransform, Transform _playerTransform, float _speed)
    {
        this.monsterTransform = _monsterTransform;
        this.playerTransform = _playerTransform;
        this.speed = _speed;

        // Rigidbody2D 컴포넌트를 가져옵니다.
        monsterRigidbody = _monsterTransform.GetComponent<Rigidbody2D>();
    }

    private void OnPlayerCollision(MonsterEventType eventType, Component sender, TransformEventArgs args)
    {
        // 이벤트 발신자가 이 몬스터인지 확인
        if (sender.transform == monsterTransform)
        {
            isWaiting = true;
            waitTimer = waitTime;
        }
    }

    public void FixedUpdate()
    {
        // 대기 중일 경우 타이머를 감소시키고, 대기 시간이 끝나면 이동 시작
        if (isWaiting)
        {
            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0f)
            {
                isWaiting = false;
            }
        }
        else
        {
            Moving();
        }
    }

    public void SetPlayerTransform(Transform player)
    {
        playerTransform = player;
    }

    private void Moving()
    {
        if (playerTransform != null && monsterRigidbody != null)
        {
            // Rigidbody2D를 이용하여 부드럽게 이동
            Vector3 targetPosition = playerTransform.position;
            Vector3 newPosition = Vector3.MoveTowards(monsterTransform.position, targetPosition, speed * Time.deltaTime);

            // Rigidbody2D의 MovePosition을 사용하여 이동
            monsterRigidbody.MovePosition(newPosition);
        }
    }
}
