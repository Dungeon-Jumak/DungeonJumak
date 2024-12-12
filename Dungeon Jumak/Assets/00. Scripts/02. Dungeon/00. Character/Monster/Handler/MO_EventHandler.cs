// Engine
using UnityEngine;

// Ect
using Utils.EnumTypes;

public class MO_EventHandler : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 스킬과 충돌했을 경우
        if (other.CompareTag("Skill"))
        {
            Skill skill = other.gameObject.GetComponent<Skill>();

            EventManager<MonsterEventType>.Instance.PostNotification(
                MonsterEventType.HitBySkill, 
                this, 
                new TransformEventArgs(transform, skill.data.damage));
        }
    }
}