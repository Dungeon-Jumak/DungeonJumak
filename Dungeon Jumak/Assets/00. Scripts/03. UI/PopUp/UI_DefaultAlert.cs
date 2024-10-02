//Unity
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_DefaultAlert : UI_PopUp
{
    [Header("알럿이 닫히는데 걸리는 시간")]
    [SerializeField] private float m_closedTIme = 3f;

    private void OnEnable()
    {
        Invoke("CloseAlert", m_closedTIme);
    }

    enum Texts
    {
        AlertText,
    }

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        //부모 초기화 함수 오버라이딩
        base.Init();

        //오브젝트 바인딩
        Bind<Text>(typeof(Texts));
    }

    /// <summary>
    /// 알럿을 닫기 위한 함수, 3초 후에 실행 (Invoke 사용)
    /// </summary>
    public void CloseAlert()
    {
        GameManager.UI.ClosePopUpUI(this);
    }
}
