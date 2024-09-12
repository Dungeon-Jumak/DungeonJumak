//Engine
using UnityEngine;

namespace Data.Object
{
    [CreateAssetMenu(fileName = "SkillData", menuName = "Scriptable/Skill")]
    public class SkillData_Base : ScriptableObject
    {
        [Header("��ų ����")]
        [Tooltip("��ų�� �̸��� �����մϴ�.")]
        [SerializeField] private string m_name;

        [Tooltip("��ų�� ���Ϳ��� ���ϴ� ���� ������ ��ġ�� �����մϴ�.")]
        [SerializeField] private float m_attackDamage;
    }
}