// Engine
using UnityEngine;

// System
using System.Collections.Generic;

public class Scanner : MonoBehaviour
{
    [Header("스캔 범위")]
    public float scanRange;

    public Transform nearestTarget;

    [Header("레이어 마스크")]
    [SerializeField] private LayerMask targetLayers; // 스캔 가능한 모든 레이어

    [Header("레이 캐스트 힛 배열")]
    private RaycastHit2D[] targets;

    [Header("탐지된 모든 타겟")]
    private List<Transform> allTargets = new List<Transform>();

    private void FixedUpdate()
    {
        targets = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0, targetLayers);
        allTargets.Clear();

        foreach (var hit in targets)
        {
            allTargets.Add(hit.transform);
        }

        nearestTarget = GetNearestTarget(targetLayers); // 기본 레이어 필터링
    }

    /// <summary>
    /// 특정 레이어에 맞는 가장 가까운 타겟 반환
    /// </summary>
    public Transform GetNearestTarget(LayerMask layerMask)
    {
        Transform result = null;
        float lastDistance = Mathf.Infinity;

        foreach (Transform target in allTargets)
        {
            if (((1 << target.gameObject.layer) & layerMask) != 0)
            {
                float curDistance = Vector3.Distance(transform.position, target.position);
                if (curDistance < lastDistance)
                {
                    lastDistance = curDistance;
                    result = target;
                }
            }
        }

        return result;
    }
}
