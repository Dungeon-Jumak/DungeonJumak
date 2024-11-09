// Engine
using UnityEngine;

// Ect
using Utils.EnumTypes;

public class MO_EventHandler : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Skill"))
        {
            // HitBySkill 이벤트 발생
            EventManager<MonsterEventType>.Instance.PostNotification(MonsterEventType.HitBySkill, this, new TransformEventArgs(transform));

            // 로그 찌금 ....
            Debug.Log("스킬한테 맞았다 ㅡㅜㅜ 아 힘들어");
        }
    }
}