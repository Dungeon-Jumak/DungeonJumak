//Engine
using UnityEngine;

//System
using System.Collections;

//Interface
using Interfaces;
using System.Security.Cryptography;

public class Customer : MonoBehaviour, ITurnable, IMovable
{
    public bool canMove;
    public bool isSit;

    public Transform destination;

    public int cuurentSeatNubmer;

    [SerializeField] private float speed = 0.5f;

    private float currentX;
    private float lastX;

    private Pool pool;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    //Dependency Injection
    private CU_AnimationHandler animationHandler;
    private CU_AudioHandler audioHandler;
    private CU_EventHandler eventHandler;
    private CU_MoveHandler moveHandler;

    private EntranceController entranceController;

    private const string defaultTag = "Customer";
    private const string exitTag = "Customer_Exit";

    public class CustomerData
    {
        public int id;
        public float speed;
    }

    private void OnEnable()
    {
        speed = 0.5f;

        currentX = transform.localPosition.x;
        lastX = transform.localPosition.x;

        canMove = true;

        pool = GetComponent<Pool>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        animationHandler = new CU_AnimationHandler(animator);
        audioHandler = new CU_AudioHandler();
        eventHandler = new CU_EventHandler();
        moveHandler = new CU_MoveHandler(speed, currentX, lastX, spriteRenderer, this, pool);

        entranceController = FindObjectOfType<EntranceController>();
    }

    private void Update()
    {
        //기본 이동 로직
        moveHandler.Move(transform, destination, canMove);
        moveHandler.Turn(transform, canMove);
        moveHandler.CheckDestination(transform, destination, canMove);

        //기본 애니메이션 로직
        animationHandler.PlayMoveAnimation(canMove);
        animationHandler.PlaySitAnimation(isSit);
    }

    /// <summary>
    /// 풀 반환 시 초기화 하기 위한 함수
    /// </summary>
    public void Init()
    {
        canMove = false;
        isSit = false;

        gameObject.tag = defaultTag;
    }

    /// <summary>
    /// 음식을 다먹고 주막에서 나가기 위해 CountButton.cs에서 호출하기 위한 메소드
    /// </summary>
    public void ExitJumak()
    {
        isSit = false;
        canMove = true;

        gameObject.tag = exitTag;

        destination = entranceController.exitPoint;
    }

    /// <summary>
    /// 대기가 끝나면 주막으로 들어가기 위해 EntranceController.cs에서 호출하기 위한 메소드
    /// </summary>
    public void EntranceJumak()
    {
        canMove = true;

        destination = entranceController.entrancePoint;
    }

    /// <summary>
    /// 대기 중, 앞 사람이 주막에 들어갔을 때 앞으로 땡겨서 줄서기 위해 EntranceController.cs에서 호출
    /// </summary>
    public void GoForwardInWaitingQueue(Transform _forwardTransform)
    {
        canMove = true;
        destination = _forwardTransform;
    }

    public void StartEat()
    {
        animationHandler.PlayEatAnimation();
    }

    public void FinishEat()
    {
        animationHandler.FinishEatAnimation();
    }
}
    