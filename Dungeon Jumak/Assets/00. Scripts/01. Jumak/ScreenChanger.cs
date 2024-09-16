using UnityEngine;
using UnityEngine.UI;

public class ScreenChanger : MonoBehaviour
{
    //현재 스크린 이름
    //Jumak, Village
    public string m_currentScreen;

    [Header("주막 카메라 컴포넌트")]
    [SerializeField] private Camera m_jumakCamera;
    [Header("마을 카메라 컴포넌트")]
    [SerializeField] private Camera m_villageCamera;

    [Header("주막 스크롤 렉트")]
    [SerializeField] private ScrollRect m_jumakScrollRect;

    private void Start()
    {
        m_currentScreen = "Jumak";
    }

    /// <summary>
    /// 스크린을 주막에서 마을로 이동시키는 메소드
    /// </summary>
    public void JumakToVillage()
    {
        m_currentScreen = "Village";

        m_jumakScrollRect.enabled = false;

        m_jumakCamera.gameObject.SetActive(false);
        m_villageCamera.gameObject.SetActive(true);
    }

    /// <summary>
    /// 스크린을 마을에서 주막으로 이동시키는 메소드
    /// </summary>
    public void VillageToJumak()
    {
        m_currentScreen = "Jumak";

        m_jumakCamera.gameObject.SetActive(true);
        m_villageCamera.gameObject.SetActive(false);

        m_jumakScrollRect.enabled = true;
    }
}
