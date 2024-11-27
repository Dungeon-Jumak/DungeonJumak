// Engine
using UnityEngine;

// UI
using UnityEngine.UI;

// Ect
using Data.Character;
using Utils.EnumTypes;

public class DP_HPController : MonoBehaviour
{
    // SO
    public DunjeonPlayerDataSO data;

    [Header("HP바")]
    [SerializeField] private Slider hpBar;

    [Header("HP바 위치")]
    [SerializeField] private Transform hpBarPos;

    // 현재 HP
    private float currentHp;

    private void OnEnable()
    {
        // HP 초기화
        currentHp = data.hp;

        // HP 바 설정
        if (hpBar != null)
        {
            hpBar.maxValue = data.hp;
            hpBar.value = currentHp;
        }

        // 리스너(스킬 데미지) 등록
        EventManager<PlayerEventType>.Instance.AddListener(PlayerEventType.HitByMonster, OnHitMonster);
    }

    private void FixedUpdate()
    {
        // HP바 위치 업데이트
        if (hpBar != null && hpBarPos != null)
        {
            hpBar.transform.position = Camera.main.WorldToScreenPoint(hpBarPos.position);
        }
    }

    #region Listner

    private void OnHitMonster(PlayerEventType eventType, Component sender, TransformEventArgs args)
    {
        if (args.m_Value[0] is float monsterDamage)
        {
            TakeDamage(monsterDamage);
        }
    }

    #endregion

    /// <summary>
    /// TakeDamage: 데미지로 인한 체력 감소 로직
    /// </summary>
    public void TakeDamage(float damage)
    {
        currentHp -= damage;
        currentHp = Mathf.Max(currentHp, 0); // HP가 0 이하로 내려가지 않도록 처리

        // HP 바 업데이트
        if (hpBar != null)
        {
            hpBar.value = currentHp;
        }

        if (currentHp <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// Die: 체력 0 -> 플레이어 사망
    /// </summary>
    private void Die()
    {
        Debug.Log($"{gameObject.name} is dead.");
        gameObject.SetActive(false);
        //GameManager.UI.ShowPopupUI<UI_PopUp>("DungeonGameOverPopUp");
    }
}
