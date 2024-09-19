//Engine
using UnityEngine;

//Interface
using Interfaces;

//Ect
using Data.Character;

public class DunjeonPlayer : MonoBehaviour, IDamageable, ITurnable, IMovable
{
    // DPlayer Data Scriptable
    [SerializeField] private DunjeonPlayerData_Base data;

    // Dependency Injection
    private DP_AnimationHandler animationHandler;
    private DP_AudioHandler audioHandler;
    private DP_MoveHandler moveHandler;
    private DP_EventHandler eventHandler;

    // Animator
    private Animator animator;

    // SpriteRenderer
    private SpriteRenderer spriteRenderer;

    // Speed
    private float speed;

    // Hp
    private float hp;

    private void OnEnable()
    {
        // Get Component
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // .
        animationHandler = new DP_AnimationHandler();
        audioHandler = new DP_AudioHandler();
        moveHandler = new DP_MoveHandler();
        eventHandler = new DP_EventHandler();

        // Set variable value with scriptable
        speed = data.Speed;
        hp = data.Hp;
    }

    private void Update()
    {
        
    }
}
