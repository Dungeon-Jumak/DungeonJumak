// Engine
using UnityEngine;

// Ect
using Utils.EnumTypes;

public class MO_EventHandler : MonoBehaviour
{
    private MO_DamageHandler damageHandler;

    private void Awake()
    {
        damageHandler = GetComponent<MO_DamageHandler>();
    }

    private void OnEnable()
    {
        EventManager<MonsterEventType>.Instance.AddListener(MonsterEventType.HitBySkill, OnHitBySkill);
    }

    private void OnHitBySkill(MonsterEventType eventType, Component sender, TransformEventArgs args)
    {
        if (sender is Monster)
        {
            float damage = args.m_Value.Length > 0 ? (float)args.m_Value[0] : 0;
            damageHandler.TakeDamage(damage);
        }
    }
}
