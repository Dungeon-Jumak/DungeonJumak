// Engine
using UnityEngine;

// Interface
using Interfaces;

// Ect
using Data.Character;

public class DunjeonPlayer : MonoBehaviour, IDamageable, ITurnable, IMovable
{
    #region Variables

    [Header("SO")]
    public DunjeonPlayerDataSO data;

    [Header("스캐너")]
    public Scanner scanner; // 스캐너

    // 핸들러
    private DP_AnimationHandler animationHandler;
    private DP_MoveHandler moveHandler;

    // 컴포넌트
    private Rigidbody2D rigidbody;
    private Transform transform;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    #endregion

    private void Awake()
    {
        // 컴포넌트 할당
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // 핸들러 인스턴스 생성
        animationHandler = new DP_AnimationHandler(spriteRenderer, animator);
        moveHandler = new DP_MoveHandler(transform, rigidbody, data.Speed, scanner);
    }

    private void Update()
    {
        moveHandler.FixedUpdate();
    }
}
