//Engine
using UnityEngine;

//Interface
using Interfaces;

//Ect
using Data.Character;

public class DunjeonPlayer : MonoBehaviour, IDamageable, ITurnable, IMovable
{
    // 던전 플레이어 데이터
    public DunjeonPlayerData_Base data;

    // 스캐너
    [SerializeField] private Scanner scanner;

    private DP_AnimationHandler animationHandler;
    private DP_MoveHandler moveHandler;

    private Rigidbody2D rigidbody;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        animationHandler = new DP_AnimationHandler(spriteRenderer, animator);
        moveHandler = new DP_MoveHandler(rigidbody, data.Speed, scanner);
    }

    private void Update()
    {
        moveHandler.Update();
    }
}
