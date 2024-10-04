//Engine
using UnityEngine;

namespace Data.Object
{
    [CreateAssetMenu(fileName = "Skill Data", menuName = "Scriptable/Skill")]
    public class SkillData_Base : ScriptableObject
    {
        [Header("스킬 정보")]
        [Tooltip("스킬의 이름을 설정합니다.")]
        [SerializeField] private string m_name;

        [Tooltip("스킬이 몬스터에게 가하는 공격 데미지 수치를 설정합니다.")]
        [SerializeField] private float m_attackDamage;

        [Tooltip("스킬의 플레이어 관통력 수치를 설정합니다.")]
        [SerializeField] private float m_per;

        [Tooltip("스킬의 플레이어 넉백력 수치를 설정합니다.")]
        [SerializeField] private float m_knockBack;

        public string Name { get { return m_name; } }
        public float Damage { get { return m_attackDamage; } }
        public float Per { get { return m_per; } }
        public float KnockBack { get { return m_knockBack; } }
    }
}