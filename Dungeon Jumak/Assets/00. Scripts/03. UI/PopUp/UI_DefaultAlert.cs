//Unity
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_DefaultAlert : UI_PopUp
{
    [Header("�˷��� �����µ� �ɸ��� �ð�")]
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
        //�θ� �ʱ�ȭ �Լ� �������̵�
        base.Init();

        //������Ʈ ���ε�
        Bind<Text>(typeof(Texts));
    }

    /// <summary>
    /// �˷��� �ݱ� ���� �Լ�, 3�� �Ŀ� ���� (Invoke ���)
    /// </summary>
    public void CloseAlert()
    {
        GameManager.UI.ClosePopUpUI(this);
    }
}
