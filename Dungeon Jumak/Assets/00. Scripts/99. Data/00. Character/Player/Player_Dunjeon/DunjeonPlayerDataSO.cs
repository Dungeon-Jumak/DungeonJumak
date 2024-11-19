//Engine
using UnityEngine;

namespace Data.Character
{
    [CreateAssetMenu(fileName = "Dunjeon Player Data", menuName = "Scriptable/Character/Player/DunjeonPlayer")]
    public class DunjeonPlayerDataSO : ScriptableObject
    {
        [Header("기본 정보")]
        public float hp; // 최대 HP
        public float speed; // 이동 속도
        public float dropRate; // 아이템 드랍율

        [Header("전투 스탯")]
        public float damage; // 공격력
    }
}

