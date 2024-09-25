//Engine
using UnityEngine;

//Interface
using Interfaces;

//Ect
using Data.Character;

public class DunjeonPlayer : MonoBehaviour, IDamageable, ITurnable, IMovable
{
    // 던전 플레이어 스크립터블 데이터
    [SerializeField] private DunjeonPlayerData_Base data;

    // 스캐너
    [SerializeField] private Scanner scanner;

    // 핸들러 스크립트들
    private DP_AnimationHandler animationHandler;
    private DP_MoveHandler moveHandler;

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        animationHandler = new DP_AnimationHandler(spriteRenderer, animator);
        moveHandler = new DP_MoveHandler(transform, data.Speed, scanner);
    }
}
