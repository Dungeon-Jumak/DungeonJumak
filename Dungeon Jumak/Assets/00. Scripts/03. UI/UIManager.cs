//System
using System.Collections;
using System.Collections.Generic;

//Unity
using UnityEngine;
using UnityEngine.UI;


//UI�� ���� �ϱ� ���� Ŭ����
// Scene UI : Scene�� ������ ��, �ѹ��� ������ �Ǵ� UI
// Popup UI : Scene �߰��� �ö�� �� �ִ� UI | Stack���� ����
public class UIManager
{
    int m_order = 10;                                           

    Stack<UI_PopUp> m_popupStack = new Stack<UI_PopUp>();      
    UI_Scene m_sceneUI = null;                                  

    public GameObject Root
    {
        get
        {
            GameObject root = GameObject.Find("@UI_Root");      //UI �ֻ��� �θ� Ž��
            if (root == null)
                root = new GameObject { name = "@UI_Root" };    //���� ��� ���� ����

            return root;
        }
    }   

    /// <summary>
    /// ĵ������ �����ϱ� ���� �޼ҵ�
    /// </summary>
    /// <param name="_go"></param>
    /// <param name="_sort"></param>
    public void SetCanvas(GameObject _go, bool _sort = true)
    {
        Canvas canvas = Util.GetOrAddComponent<Canvas>(_go);        //�Ķ���ͷ� ���� ���ӿ�����Ʈ�� Canvas ������Ʈ�� ������

        canvas.renderMode = RenderMode.ScreenSpaceOverlay;          //���� ��� ����

        canvas.overrideSorting = true;                              //���� ���� Ȱ��ȭ

        if (_sort)
        {
            canvas.sortingOrder = m_order;                          //�ʱ⿡ ������ (10+@)�� ������ ���� ������ ����
            m_order++;                                              //������ �ö�� UI�� ���� +1
        }
        else
            canvas.sortingOrder = 0;                                //���� ���ΰ� False�� ��� ���� ������ 0���� ����
    }

    /// <summary>
    /// UI ���� �ڽ� �������� �����ϱ� ���� �޼ҵ�
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="_parent"></param>
    /// <param name="_name"></param>
    /// <returns></returns>
    public T MakeSubItem<T> (Transform _parent = null, string _name = null) where T : UI_Base
    {
        if (string.IsNullOrEmpty(_name))                                             //���� �������� �̸��� ���� ���
            _name = typeof(T).Name;                                                  //Ÿ��, Ŭ������ �̸����� ����

        GameObject go = GameManager.Resource.Instantiate($"UI/SubItem/{_name}");     //�ش� �������� �ҷ���
        if (_parent != null)                                                        //�θ� �Ķ���ͷ� �޾��� ���
            go.transform.SetParent(_parent);                                        //�θ�� ����

        return Util.GetOrAddComponent<T>(go);                                       //�����տ��� ������Ʈ�� ������ ����
    }

    /// <summary>
    /// Scene UI�� �����ֱ� ���� �޼ҵ�
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="_name"></param>
    /// <returns></returns>
    public T ShowSceneUI<T> (string _name = null) where T : UI_Scene
    {
        if (string.IsNullOrEmpty(_name))                                            //�̸��� �������� �ʾ��� ���
            _name = typeof(T).Name;                                                 //���׸� Ÿ������ �̸� ����

        GameObject go = GameManager.Resource.Instantiate($"UI/Scene/{_name}");      //�̸��� �ش��ϴ� �������� ����
        T sceneUI = Util.GetOrAddComponent<T>(go);                                  //T ������Ʈ�� �ҷ���
        m_sceneUI = sceneUI;

        go.transform.SetParent(Root.transform);                                     //@UI_Root ������ ����

        return sceneUI;                                                             //������Ʈ ����
    }

    /// <summary>
    /// �Ϲ� �˾��� �����ֱ� ���� �޼ҵ�
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="_name"></param>
    /// <returns></returns>
    public T ShowPopupUI<T> (string _name = null) where T : UI_PopUp
    {
        if (string.IsNullOrEmpty(_name))
            _name = typeof(T).Name;

        GameObject go = GameManager.Resource.Instantiate($"UI/PopUp/{_name}");


        T popup = Util.GetOrAddComponent<T>(go);
        m_popupStack.Push(popup);

        go.transform.SetParent(Root.transform);

        return popup;
    }

    /// <summary>
    /// ���ϴ� �˾��� �ݱ� ���� �޼ҵ�
    /// </summary>
    /// <param name="_popup"></param>
    public void ClosePopUpUI(UI_PopUp _popup)
    {
        if (m_popupStack.Count == 0) return;

        if (m_popupStack.Peek() != _popup)
        {
            Debug.Log("Failed to Close Popup");
            return;
        }

        ClosePopUpUI();
    }

    /// <summary>
    /// ���� ���� �˾��� �ݱ� ���� �޼ҵ�
    /// </summary>
    public void ClosePopUpUI()
    {
        if (m_popupStack.Count == 0) return;

        UI_PopUp popup = m_popupStack.Pop();
        GameManager.Resource.Destroy(popup.gameObject);
        popup = null;

        m_order--;
    }

    /// <summary>
    /// ��� �˾��� �ѹ��� �ݱ� ���� �޼ҵ�
    /// </summary>
    public void CloseAllPopUpUI()
    {
        while (m_popupStack.Count > 0) ClosePopUpUI();
    }

    /// <summary>
    /// �ʱ�ȭ �ϱ� ���� �޼ҵ�
    /// </summary>
    public void Clear()
    {
        CloseAllPopUpUI();
        m_sceneUI = null;
    }
}
