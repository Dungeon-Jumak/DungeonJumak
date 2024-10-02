using UnityEngine;

public class CameraResolution : MonoBehaviour
{
    private const float m_TARGET_ASPECT = (float) 9 / 16;

    private void Awake()
    {
        Init();
    }

    public void InitButton()
    {
        Init();
    }

    private void Init()
    {
        Camera camera = GetComponent<Camera>();

        Rect rect = camera.rect;

        float scaleHeight = ((float)Screen.width / Screen.height) / m_TARGET_ASPECT;
        float scaleWidth = 1f / scaleHeight;

        if (scaleHeight < 1)
        {
            rect.height = scaleHeight;
            rect.y = (1f - scaleHeight) / 2f;
        }
        else
        {
            rect.width = scaleWidth;
            rect.x = (1f - scaleWidth) / 2f;
        }

        camera.rect = rect;
    }
}
