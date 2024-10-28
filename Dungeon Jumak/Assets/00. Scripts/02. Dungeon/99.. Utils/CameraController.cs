// Engine
using UnityEngine;

public class CameraController : MonoBehaviour
{
    #region variables

    [SerializeField] Transform player; // 타겟 플레이어

    [SerializeField] float smoothing = 0.2f; // 스무딩 값

    //--- 카메라 이동 바운더리 ---//
    [SerializeField] Vector2 minCameraBoundary;
    [SerializeField] Vector2 maxCameraBoundary;

    #endregion

    private void FixedUpdate()
    {
        Vector3 targetPos = new Vector3(player.position.x, player.position.y, this.transform.position.z);

        targetPos.x = Mathf.Clamp(targetPos.x, minCameraBoundary.x, maxCameraBoundary.x);
        targetPos.y = Mathf.Clamp(targetPos.y, minCameraBoundary.y, maxCameraBoundary.y);

        transform.position = Vector3.Lerp(transform.position, targetPos, smoothing);
    }
}
