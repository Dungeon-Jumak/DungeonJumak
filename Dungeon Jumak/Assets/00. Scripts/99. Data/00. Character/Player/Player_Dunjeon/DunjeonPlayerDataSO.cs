//Engine
using UnityEngine;

namespace Data.Character
{
    [CreateAssetMenu(fileName = "Dunjeon Player Data", menuName = "Scriptable/Character/Player/DunjeonPlayer")]
    public class DunjeonPlayerDataSO : ScriptableObject
    {
        //-- 기본 정보 --//
        [Header("기본 정보")]
        [Tooltip("이동 속도를 설정합니다.")]
        [SerializeField] private float m_speed; // 이동 속도

        [Tooltip("넉백력을 설정합니다.")]
        [SerializeField] private float knockBack; // 넉백력

        //-- 전투 스택 --//
        [Space(10)]
        [Header("전투 스탯")]
        [Tooltip("플레이어의 던전에서의 최대 체력 수치를 설정합니다.")]
        [SerializeField] private float m_hp; // 최대 HP

        public float Hp { get { return m_hp; } }
        public float Speed { get { return m_speed; } }
        public float KnockBack { get { return knockBack; } }
    }
}

