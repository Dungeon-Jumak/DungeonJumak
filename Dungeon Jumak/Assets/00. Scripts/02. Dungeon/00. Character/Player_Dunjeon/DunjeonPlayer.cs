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
    public Scanner scanner;

    private DP_MoveHandler moveHandler;
    private Rigidbody2D rigidbody;
    private LayerMask movementLayers;

    #endregion

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();

        movementLayers = LayerMask.GetMask("Monster", "Environment"); // 이동에 사용할 레이어 초기화
        moveHandler = new DP_MoveHandler(transform, rigidbody, data.speed, scanner, movementLayers);
    }

    private void FixedUpdate()
    {
        moveHandler.FixedUpdate();
    }
}
