// Engine
using UnityEngine;

public class Mo_MoveHandler
{
    private float speed; 
    private Transform monsterTransform; 
    private Transform playerTransform;  

    public Mo_MoveHandler(Transform _monsterTransform, Transform _playerTransform, float _speed)
    {
        this.monsterTransform = _monsterTransform;
        this.playerTransform = _playerTransform;
        this.speed = _speed;
    }

    public void Update()
    {
        Moving();
    }

    public void Moving()
    {
        if (playerTransform != null)
        {
            // 몬스터가 플레이어의 위치로 이동하도록 함
            monsterTransform.position = Vector3.MoveTowards(
                monsterTransform.position,              // 몬스터의 현재 위치
                playerTransform.position,               // 플레이어의 위치
                speed * Time.deltaTime                  // 속도
            );
        }
    }

    public void StopMoving()
    {
        // 원하는 경우, 몬스터의 이동을 멈추는 로직
    }

    public void MoveTowardToPlayer(Transform target)
    {
        playerTransform = target;
    }
}
