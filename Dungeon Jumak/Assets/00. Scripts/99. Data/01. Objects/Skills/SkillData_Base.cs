//Engine
using UnityEngine;

namespace Data.Object
{
    [CreateAssetMenu(fileName = "SkillData", menuName = "Scriptable/Skill")]
    public class SkillData_Base : ScriptableObject
    {
        [Header("스킬 정보")]
        [Tooltip("스킬의 이름을 설정합니다.")]
        [SerializeField] private string m_name;

        [Tooltip("스킬이 몬스터에게 가하는 공격 데미지 수치를 설정합니다.")]
        [SerializeField] private float m_attackDamage;
    }
}