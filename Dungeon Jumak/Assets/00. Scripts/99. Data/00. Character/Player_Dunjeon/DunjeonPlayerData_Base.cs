//Engine
using UnityEngine;

namespace Data.Character
{
    [CreateAssetMenu(fileName = "Dunjeon Player Data", menuName = "Scriptable/Character/Player/DunjeonPlayer")]
    public class DunjeonPlayerData_Base : ScriptableObject
    {
        [Header("기본 정보")]
        [Tooltip("이동 속도를 설정합니다.")]
        [SerializeField] private float m_speed;

        [Space(10)]
        [Header("전투 스탯")]
        [Tooltip("플레이어의 던전에서의 최대 체력 수치를 설정합니다.")]
        [SerializeField] private float m_hp;

        public float Hp { get { return m_hp; } }
        public float Speed { get { return m_speed; } }
    }
}

