//Engine
using UnityEngine;

namespace Data.Character
{
    [CreateAssetMenu(fileName = "MonsterData", menuName = "Scriptable/Character/Monster")]
    public class MonsterData_Base : ScriptableObject
    {
        [Header("���� ����")]
        [Tooltip("������ �̸��� �����մϴ�.")]
        [SerializeField] private string m_name;

        [Space(10)]
        [Header("���� ����")]
        [Tooltip("���Ͱ� �÷��̾�� ���ϴ� ���� ������ ��ġ�� �����մϴ�.")]
        [SerializeField] private float m_attackDamage;

        [Tooltip("������ ü��(HP)�� �����մϴ�.")]
        [SerializeField] private float m_hp;

        [Tooltip("���� óġ �� �÷��̾�� �����ϴ� ����ġ(XP)�� �����մϴ�.")]
        [SerializeField] private float m_xp;

        [Space(10)]
        [Header("�̵� ����")]
        [Tooltip("������ �̵� �ӵ��� �����մϴ�.")]
        [SerializeField] private float m_speed;

        public string MonsterName { get { return m_name; } }
        public float Damage { get { return m_attackDamage; } }
        public float Hp { get { return m_hp; } }
        public float Xp { get { return m_xp; } }
        public float Speed { get { return m_speed; } }
    }
}
