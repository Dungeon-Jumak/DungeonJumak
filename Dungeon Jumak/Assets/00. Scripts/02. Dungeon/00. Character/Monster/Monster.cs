//Engine
using UnityEngine;

//Interface
using Interfaces;

//Ect
using Data.Character;

public class Monster : MonoBehaviour, IDamageable, ITurnable, IMovable
{
    public MonsterDataSO data;

    private Mo_AnimationHandler animationHandler;
    private Mo_MoveHandler moveHandler;
    private MO_DamageHandler damageHandler;

    private Transform playerTransform;

    private void Awake()
    {
        var animator = GetComponent<Animator>();
        var spriteRenderer = GetComponent<SpriteRenderer>();

        // 핸들러 초기화
        animationHandler = new Mo_AnimationHandler(spriteRenderer, animator);
        playerTransform = GameObject.FindWithTag("Player").transform;
        moveHandler = new Mo_MoveHandler(transform, playerTransform, data.speed);

        // DamageHandler 초기화
        damageHandler = gameObject.AddComponent<MO_DamageHandler>();
        damageHandler.Initialize(data);
    }

    private void Update()
    {
        moveHandler.SetPlayerTransform(playerTransform);
        moveHandler.FixedUpdate();
    }
}
