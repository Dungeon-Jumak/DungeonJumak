//Engine
using UnityEngine;

//Interface
using Interfaces;

public class Customer : MonoBehaviour, ITurnable, IMovable
{
    public bool willVisit;
    public bool canMove;
    public bool isSit;

    public Transform destination;

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

        willVisit = false;
        canMove = true;

        pool = GetComponent<Pool>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        animationHandler = new CU_AnimationHandler(animator);
        audioHandler = new CU_AudioHandler();
        eventHandler = new CU_EventHandler();
        moveHandler = new CU_MoveHandler(speed, currentX, lastX, spriteRenderer, this, pool);
    }

    private void Update()
    {
        moveHandler.Move(transform, destination, canMove);
        moveHandler.Turn(transform, canMove);
        moveHandler.CheckDestination(transform, destination, canMove);

        animationHandler.PlayMoveAnimation(canMove);
        animationHandler.PlaySitAnimation(isSit);
    }

    public void Init()
    {
        canMove = false;
        isSit = false;
    }
}
    