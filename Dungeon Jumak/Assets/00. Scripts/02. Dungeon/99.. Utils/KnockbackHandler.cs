// System
using System.Collections.Generic;

// Engine
using UnityEngine;

[System.Serializable]
public class KnockbackSettings
{
    [Header("충돌 오브젝트 태그")]
    public string targetTag;

    [Header("넉백 수치")]
    public float knockbackForce; 
}

public class KnockbackHandler : MonoBehaviour
{
    [Header("넉백 지속 시간")]
    [SerializeField] private float knockbackDuration = 0.5f;

    [Header("태그별 넉백 설정")]
    [SerializeField] private List<KnockbackSettings> knockbackSettingsList = new List<KnockbackSettings>();

    private bool isKnockedBack = false; // 넉백 상태 여부
    private float knockbackTimer = 0f; // 넉백 지속 시간 타이머
    private Rigidbody2D rb; 

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (isKnockedBack)
        {
            knockbackTimer -= Time.deltaTime;

            if (knockbackTimer <= 0)
            {
                isKnockedBack = false;
            }
        }
    }

    // GetKnockbackForce: 태그에 따른 넉백 값 찾기
    private float GetKnockbackForce(string tag)
    {
        foreach (var settings in knockbackSettingsList)
        {
            if (settings.targetTag == tag)
            {
                return settings.knockbackForce;
            }
        }
        return 0f; // 해당 태그가 없으면 0을 반환
    }

    // ApplyKnockback: 넉백 적용 메소드
    public void ApplyKnockback(Vector2 direction, float knockbackForce)
    {
        if (!isKnockedBack)
        {
            rb.velocity = Vector2.zero; // 현재 속도 초기화
            rb.AddForce(direction.normalized * knockbackForce, ForceMode2D.Impulse); // 넉백 방향으로 힘 적용

            // 넉백 상태와 타이머 설정
            isKnockedBack = true;
            knockbackTimer = knockbackDuration;
        }
    }

    // OnCollisionEnter2D: 콜라이더 충돌 처리
    private void OnCollisionEnter2D(Collision2D collision)
    {
        float knockbackForce = GetKnockbackForce(collision.gameObject.tag);
        if (knockbackForce > 0)
        {
            // 충돌한 오브젝트와의 충돌 위치에서 반대 방향으로 넉백
            Vector2 collisionDirection = (rb.position - (Vector2)collision.contacts[0].point).normalized;
            ApplyKnockback(collisionDirection, knockbackForce);
        }
    }
}
