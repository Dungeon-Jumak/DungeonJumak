//Engine
using UnityEngine;

namespace Data.Object
{
    [CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable/Item")]
    public class ItemData_Base : ScriptableObject
    {
        [Header("아이템 정보")]
        [Tooltip("아이템의 이름을 설정합니다.")]
        [SerializeField] private string m_name;
    }
}