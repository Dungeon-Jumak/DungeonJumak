// Engine
using UnityEngine;

// Ect
using Data.Object;

public class Skill : MonoBehaviour
{
    [Header("SO")]
    public SkillDataSO data;

    [Header("RigidBody2D")]
    private Rigidbody2D rigid;

    [Header("Per")]
    private float per;

    public void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    public void Init(Vector3 direction)
    {
        this.per = data.per;

        if (per > -1)
        {
            rigid.velocity = direction * 10f;
        }
    }

    #region 몬스터 충돌 처리

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Monster") || per == -1)
            return;

        rigid.velocity = Vector2.zero;
        gameObject.SetActive(false); // 비활성화
    }

    #endregion
}
