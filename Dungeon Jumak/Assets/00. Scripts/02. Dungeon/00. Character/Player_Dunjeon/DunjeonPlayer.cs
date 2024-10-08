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
    private Transform transform;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        animationHandler = new DP_AnimationHandler(spriteRenderer, animator);
        moveHandler = new DP_MoveHandler(transform, rigidbody, data.Speed, scanner);
    }

    private void Update()
    {
        moveHandler.FixedUpdate();
    }

    #region 플레이어 충돌 처리

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Background_Obj"))
        {
            Debug.Log("배경 오브젝트와 충돌");

            Vector2 collisionDirection = (rigidbody.position - (Vector2)collision.contacts[0].point).normalized;
            rigidbody.AddForce(collisionDirection * 10.0f, ForceMode2D.Impulse);

            moveHandler.isMoving = false;
        }
    }

    #endregion
}
