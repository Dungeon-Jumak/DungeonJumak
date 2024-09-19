//Engine
using UnityEngine;

namespace Data.Character
{
    [CreateAssetMenu(fileName = "Monster Data", menuName = "Scriptable/Character/Monster")]
    public class MonsterData_Base : ScriptableObject
    {
        [Header("몬스터 정보")]
        [Tooltip("몬스터의 이름을 설정합니다.")]
        [SerializeField] private string m_name;

        [Tooltip("몬스터의 드롭 아이템 풀 매니저를 설정합니다.")]
        [SerializeField] private GameObject m_dropItemPool;

        [Tooltip("몬스터 처치 시 드롭하는 아이템의 프리팹 ID를 설정합니다.")]
        [SerializeField] private int m_dropItemId;

        [Tooltip("몬스터 처치 시 플레이어에게 제공하는 경험치(XP)를 설정합니다.")]
        [SerializeField] private float m_xp;

        [Space(10)]
        [Header("전투 스탯")]
        [Tooltip("몬스터가 플레이어에게 가하는 공격 데미지 수치를 설정합니다.")]
        [SerializeField] private float m_attackDamage;

        [Tooltip("몬스터의 체력(HP)를 설정합니다.")]
        [SerializeField] private float m_hp;

        [Space(10)]
        [Header("이동 스탯")]
        [Tooltip("몬스터의 이동 속도를 설정합니다.")]
        [SerializeField] private float m_speed;

        public string Name { get { return m_name; } }
        public GameObject DropItemPool { get { return m_dropItemPool; } }
        public int DropItemId { get { return m_dropItemId; } }
        public float Xp { get { return m_xp; } }
        public float Damage { get { return m_attackDamage; } }
        public float Hp { get { return m_hp; } }
        public float Speed { get { return m_speed; } }
    }
}
