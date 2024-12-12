// Engine
using UnityEngine;

// Ect
using Utils.EnumTypes;

public class DP_EventHandler : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //  몬스터와 충돌했을 경우
        if (collision.gameObject.CompareTag("Monster"))
        {
            Monster monster = collision.gameObject.GetComponent<Monster>();

            if (monster != null)
            {
                // 이벤트 발생 시 몬스터 데미지와 함께 TransformEventArgs 전달
                EventManager<PlayerEventType>.Instance.PostNotification(
                    PlayerEventType.HitByMonster,
                    this,
                    new TransformEventArgs(transform, monster.data.damage)
                );
            }
        }
    }
}
