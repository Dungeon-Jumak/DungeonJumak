//Engine
using UnityEngine;

namespace Data.Character
{
    [CreateAssetMenu(fileName = "Monster Data", menuName = "Scriptable/Character/Monster")]
    public class MonsterDataSO : ScriptableObject
    {
        [Header("기본 정보")]
        public string name; // 이름
        public float hp; // 체력
        public float speed; // 이동 속도
        public float dropRate; // 아이템 드랍율

        [Header("전투 스탯")]
        public float damage; // 공격력
    }
}
