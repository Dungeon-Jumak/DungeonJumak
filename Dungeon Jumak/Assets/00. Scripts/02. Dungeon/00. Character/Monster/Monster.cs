//Engine
using UnityEngine;

//Interface
using Interfaces;

//Ect
using Data.Character;

public class Monster : MonoBehaviour, IDamageable, ITurnable, IMovable
{
    // 몬스터 데이터
    public MonsterData_Base data;

    [SerializeField] private Transform playerTransform;

    private Mo_AnimationHandler animationHandler;
    private Mo_MoveHandler moveHandler;

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        animationHandler = new Mo_AnimationHandler(spriteRenderer, animator);
        moveHandler = new Mo_MoveHandler(transform, playerTransform, data.Speed);
    }

    private void Update()
    {
        moveHandler.Update();
    }
}
