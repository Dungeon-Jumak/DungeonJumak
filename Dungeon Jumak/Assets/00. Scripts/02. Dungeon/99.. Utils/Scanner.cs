// Unity
using UnityEngine;

[DisallowMultipleComponent]
public class Scanner : MonoBehaviour
{
    [Header("스캔 범위")]
    public float scanRange;

    [Header("레이어 마스크")]
    [SerializeField] private LayerMask targetLayer;

    [Header("레이 캐스트 힛 배열")]
    [SerializeField] private RaycastHit2D[] targets;

    [Header("가장 가까운 타겟")]
    public Transform nearestTarget;

    private void FixedUpdate()
    {
        targets = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0, targetLayer);

        nearestTarget = GetNearestTarget();
    }

    private Transform GetNearestTarget()
    {
        Transform result = null;
        float lastDistance = 100f;

        foreach (RaycastHit2D target in targets)
        {
            Vector3 playerPos = transform.position;
            Vector3 targetPos = target.transform.position;

            float curDistance = Vector3.Distance(playerPos, targetPos);

            if (curDistance < lastDistance)
            {
                lastDistance = curDistance;
                result = target.transform;
            }
        }

        return result;
    }

    // 스캔 범위를 씬 뷰에 시각화
    private void OnDrawGizmosSelected()
    {
        // Gizmos 색상 설정 (검은색)
        Gizmos.color = Color.black;

        // 스캐너의 스캔 범위를 원으로 그림
        Gizmos.DrawWireSphere(transform.position, scanRange);
    }
}
