//Engine
using UnityEngine;

namespace Data.Object
{
    [CreateAssetMenu(fileName = "Field Item Data", menuName = "Scriptable/Item/FieldItem")]
    public class FieldItemData_Base : ScriptableObject
    {
        [Header("필드 아이템 정보")]
        [Tooltip("필드 아이템의 이름을 설정합니다.")]
        [SerializeField] private string m_name;

        public string Name { get { return m_name; } }
    }
}