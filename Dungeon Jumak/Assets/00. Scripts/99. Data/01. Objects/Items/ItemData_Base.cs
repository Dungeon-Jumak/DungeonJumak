//Engine
using UnityEngine;

namespace Data.Object
{
    [CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable/Item")]
    public class ItemData_Base : ScriptableObject
    {
        [Header("������ ����")]
        [Tooltip("�������� �̸��� �����մϴ�.")]
        [SerializeField] private string m_name;
    }
}