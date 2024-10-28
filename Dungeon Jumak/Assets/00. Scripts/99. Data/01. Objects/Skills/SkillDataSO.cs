//Engine
using UnityEngine;

namespace Data.Object
{
    [CreateAssetMenu(fileName = "Skill Data", menuName = "Scriptable/Skill")]
    public class SkillDataSO: ScriptableObject
    {
        [Header("기본 정보")]
        public string name; // 이름
        [Tooltip("ID를 설정합니다. (0: 기본공격, 1: 불꽃고리, 2: 바람베기, 3: 천둥번개, 계속 추가.. ")]
        public int skillId; // ID
        public int count; // 투사체 수
        public float per; // 관통력
        public float speed; // 속도
        public float damage; // 적용 데미지

        [Space(10)]
        [Header("쿨타임 관련")]
        public float coolTime; // 쿨타임
        public float waitingTime; // 대기 시간
        public float duration; // 지속 시간
    }
}