//Engine
using UnityEngine;

namespace Data.Object
{
    [CreateAssetMenu(fileName = "Drop Item Data", menuName = "Scriptable/Item/DropItem")]
    public class DropItemData_Base : ScriptableObject
    {
        [Header("드롭 아이템 정보")]
        [Tooltip("드롭 아이템의 이름을 설정합니다.")]
        [SerializeField] private string m_name;

        public string Name { get { return m_name; } }
    }
}