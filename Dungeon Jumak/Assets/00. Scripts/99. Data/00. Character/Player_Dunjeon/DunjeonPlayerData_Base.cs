//Engine
using UnityEngine;

namespace Data.Character
{
    [CreateAssetMenu(fileName = "Dunjeon Player Data", menuName = "Scriptable/Character/Player/DunjeonPlayer")]
    public class DunjeonPlayerData_Base : ScriptableObject
    {
        [Header("플레이어 정보")]
        [Tooltip("플레이어의 던전에서의 최대 체력 수치를 설정합니다.")]
        [SerializeField] private float m_hp;

        public float Hp { get { return m_hp; } }
    }
}

