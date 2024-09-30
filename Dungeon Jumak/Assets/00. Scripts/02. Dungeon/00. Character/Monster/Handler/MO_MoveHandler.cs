// Engine
using UnityEngine;
using UnityEngine.EventSystems;

public class Mo_MoveHandler
{
    // 이동 속도
    [SerializeField] private float speed;

    // 트랜스폼
    private Transform playerTransform;

    public Mo_MoveHandler(Transform _playerTransform = null, float _speed = 0f)
    {
        this.playerTransform = _playerTransform;
        this.speed = _speed;
    }

    private void Update()
    {
        Moving();
    }

    public void Moving()
    {
        //이동 로직
        //스캔 범위 내에 플레이어 있으면 따라가고 벗어나면 멈추기
    }

    private void StopMoving()
    {
        // 멈추기
    }

    private void MoveTowardToPlayer(Transform target)
    {
        // 플레이어 따라가기
    }
}
