//Engine
using UnityEngine;

//Ect
using Data.Character;

public class Monster : MonoBehaviour
{ 
    // SO
    public MonsterDataSO data;

    // Handler
    private Mo_MoveHandler moveHandler;

    // Player transform
    private Transform playerTransform;

    private void Awake()
    {
        // 플레이어 transform 할당
        playerTransform = GameObject.FindWithTag("Player").transform;

        // moveHandler 생성
        moveHandler = new Mo_MoveHandler(transform, playerTransform, data.speed);
    }

    private void Update()
    {
        //-- moveHandler 설정 --//
        moveHandler.SetPlayerTransform(playerTransform);
        moveHandler.FixedUpdate();
    }
}
