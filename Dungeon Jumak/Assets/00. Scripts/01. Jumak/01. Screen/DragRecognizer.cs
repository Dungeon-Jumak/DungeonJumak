//Unity
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class DragRecognizer : MonoBehaviour
{
    [Header("Y축 변화를 체크하기 위한 최소 드래그 길이")]
    [SerializeField] private float m_minDragDistance = 50f;

    [Header("카메라를 변경하기 위한 Screen Changer 컴포넌트")]
    [SerializeField] private ScreenChanger m_screenChanger;

    [Header("")]
    [SerializeField] private HorizontalScrollSnap m_horizontalScrollSnap;
    
    private Vector2 m_startTouchPosition;
    private Vector2 m_endTouchPosition;
    private bool isDrag = false;

    private void Update()
    {
        DetectTouch();
    }

    /// <summary>
    /// 터치를 감지하기 위한 메소드
    /// </summary>
    private void DetectTouch()
    {
        //터치가 감지될 때,
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                //터치 시작
                case TouchPhase.Began:
                    m_startTouchPosition = touch.position;
                    isDrag = true;
                    break;

                //터치 이동
                case TouchPhase.Moved:
                    if (isDrag)
                    {
                        m_endTouchPosition = touch.position;
                        CheckDragDirection();
                    }
                    break;

                //터치 종료
                case TouchPhase.Ended:
                    isDrag = false;
                    break;
            }
        }
    }

    /// <summary>
    /// 드래그의 방향을 학인하기 위한 메소드
    /// </summary>
    private void CheckDragDirection()
    {
        float differenceX = m_endTouchPosition.x - m_startTouchPosition.x;
        float differenceY = m_endTouchPosition.y - m_startTouchPosition.y;

        //최소 거리 이상을 드래그 하고, x 드래그 값보다 y 드래그 값이 2배 이상이면
        if (Mathf.Abs(differenceY) > m_minDragDistance && Mathf.Abs(differenceY) > 2 * Mathf.Abs(differenceX))
        {
            if (differenceY > 0 && m_screenChanger.m_currentScreen.Equals("Village"))
            {
                m_screenChanger.VillageToJumak();
            }
            else if (differenceY < 0 && m_screenChanger.m_currentScreen.Equals("Jumak") && m_horizontalScrollSnap.CurrentPage == 1)
            {
                m_screenChanger.JumakToVillage();
            }

            //드래그 완료시 다시 초기화
            isDrag = false;
        }
    }
}
