//Unity
using UnityEngine;

[DisallowMultipleComponent]
public class Scanner : MonoBehaviour
{
    [Header("스캔 범위")]
    [SerializeField] private float scanRange;

    [Header("레이어 마스크")]
    [SerializeField] private LayerMask targetLayer;

    [Header("레이 캐스트 힛 배열")]
    [SerializeField] private RaycastHit2D[] targets;

    [Header("가장 가까운 타겟")]
    public Transform nearestTarget;

    private void FixedUpdate()
    {
        //Set Targets
        targets = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0, targetLayer);

        //Search nearest target
        nearestTarget = GetNearestTarget();
    }

    //Search nearest target
    private Transform GetNearestTarget()
    {
        Transform result = null;
        float lastDistance = 100f;

        foreach (RaycastHit2D target in targets)
        {
            //Player's pos
            Vector3 playerPos = transform.position;

            //Target's pos
            Vector3 targetPos = target.transform.position;

            //Compute Distance
            float curDistance = Vector3.Distance(playerPos, targetPos);

            //Update last distance
            if (curDistance < lastDistance)
            {
                lastDistance = curDistance;
                result = target.transform;
            }
        }

        //return nearest transform
        return result;
    }
}
