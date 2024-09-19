// Engine
using UnityEngine;

// Ect
using Data.Object;

public class Skill : MonoBehaviour
{
    [SerializeField]
    private SkillData_Base m_data;

    // 스킬 정보 확인
    public void WatchSkillInfo()
    {
        Debug.Log("스킬 이름: " + m_data.Name);
        Debug.Log("스킬 데미지: " + m_data.Damage);
        Debug.Log("스킬 관통력: " + m_data.Per);
        Debug.Log("스킬 넉백력: " + m_data.KnockBack);
    }

    // 몬스터와 스킬이 충돌했을 때
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Monster"))
            return;
    }
}
