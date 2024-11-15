// Unity
using UnityEngine;

[DisallowMultipleComponent]
public class Scanner : MonoBehaviour
{
    [Header("스캔 범위")]
    public float scanRange;

    [Header("레이어 마스크")]
    [SerializeField] private LayerMask targetLayers; // 여러 레이어를 인식할 수 있도록 변수명 변경

    [Header("레이 캐스트 힛 배열")]
    [SerializeField] private RaycastHit2D[] targets;

    [Header("가장 가까운 타겟")]
    public Transform nearestTarget;

    private void FixedUpdate()
    {
        targets = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0, targetLayers);

        nearestTarget = GetNearestTarget();
    }

    private Transform GetNearestTarget()
    {
        Transform result = null;
        float lastDistance = Mathf.Infinity;

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

    #region gizmo

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, scanRange);
    }

    #endregion
}
