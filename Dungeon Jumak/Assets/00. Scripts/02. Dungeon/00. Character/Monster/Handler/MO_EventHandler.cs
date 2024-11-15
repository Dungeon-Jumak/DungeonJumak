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
            EventManager<MonsterEventType>.Instance.PostNotification(MonsterEventType.HitBySkill, this, new TransformEventArgs(transform));
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 플레이어와 충돌한 경우
        if (collision.gameObject.CompareTag("Player"))
        {
            EventManager<MonsterEventType>.Instance.PostNotification(MonsterEventType.PlayerCollision, this, new TransformEventArgs(transform));
        }
    }
}