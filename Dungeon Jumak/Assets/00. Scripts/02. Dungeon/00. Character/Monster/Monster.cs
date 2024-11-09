//Engine
using UnityEngine;

//Ect
using Data.Character;
using Interfaces;
using Utils.EnumTypes;

public class Monster : MonoBehaviour, IDamageable, ITurnable, IMovable
{
    // SO
    public MonsterDataSO data;

    // Handler
    private Mo_MoveHandler moveHandler;

    // Player transform
    private Transform playerTransform;

    private void Awake()
    {
        // MoveHanndler 초기화
        playerTransform = GameObject.FindWithTag("Player").transform;
        moveHandler = new Mo_MoveHandler(transform, playerTransform, data.speed);
    }

    private void Update()
    {
        moveHandler.SetPlayerTransform(playerTransform);
        moveHandler.FixedUpdate();
    }
}
