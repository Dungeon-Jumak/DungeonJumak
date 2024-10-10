//System
using System.Collections;
using System.Collections.Generic;

//Unity
using UnityEngine;
using UnityEngine.UI;


//UI를 관리 하기 위한 클래스
// Scene UI : Scene이 열렸을 때, 한번만 열리면 되는 UI
// Popup UI : Scene 중간에 올라올 수 있는 UI | Stack으로 관리
public class UIManager
{
    int m_order = 10;                                           

    Stack<UI_PopUp> m_popupStack = new Stack<UI_PopUp>();      
    UI_Scene m_sceneUI = null;                                  

    public GameObject Root
    {
        get
        {
            GameObject root = GameObject.Find("@UI_Root");      //UI 최상위 부모를 탐색
            if (root == null)
                root = new GameObject { name = "@UI_Root" };    //없을 경우 새로 생성

            return root;
        }
    }   

    /// <summary>
    /// 캔버스를 세팅하기 위한 메소드
    /// </summary>
    /// <param name="_go"></param>
    /// <param name="_sort"></param>
    public void SetCanvas(GameObject _go, bool _sort = true)
    {
        Canvas canvas = Util.GetOrAddComponent<Canvas>(_go);        //파라미터로 받은 게임오브젝트의 Canvas 컴포넌트를 가져옴

        canvas.renderMode = RenderMode.ScreenSpaceOverlay;          //렌더 모드 설정

        canvas.overrideSorting = true;                              //정렬 순서 활성화

        if (_sort)
        {
            canvas.sortingOrder = m_order;                          //초기에 설정한 (10+@)의 값으로 정렬 순서를 설정
            m_order++;                                              //다음에 올라올 UI를 위해 +1
        }
        else
            canvas.sortingOrder = 0;                                //정렬 여부가 False일 경우 정렬 순서를 0으로 설정
    }

    /// <summary>
    /// UI 안의 자식 아이템을 생성하기 위한 메소드
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="_parent"></param>
    /// <param name="_name"></param>
    /// <returns></returns>
    public T MakeSubItem<T> (Transform _parent = null, string _name = null) where T : UI_Base
    {
        if (string.IsNullOrEmpty(_name))                                             //만들 아이템의 이름이 없을 경우
            _name = typeof(T).Name;                                                  //타입, 클래스의 이름으로 설정

        GameObject go = GameManager.Resource.Instantiate($"UI/SubItem/{_name}");     //해당 프리팹을 불러옴
        if (_parent != null)                                                        //부모를 파라미터로 받았을 경우
            go.transform.SetParent(_parent);                                        //부모로 설정

        return Util.GetOrAddComponent<T>(go);                                       //프리팹에서 컴포넌트를 가져와 리턴
    }

    /// <summary>
    /// Scene UI를 보여주기 위한 메소드
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="_name"></param>
    /// <returns></returns>
    public T ShowSceneUI<T> (string _name = null) where T : UI_Scene
    {
        if (string.IsNullOrEmpty(_name))                                            //이름을 설정하지 않았을 경우
            _name = typeof(T).Name;                                                 //제네릭 타입으로 이름 설정

        GameObject go = GameManager.Resource.Instantiate($"UI/Scene/{_name}");      //이름에 해당하는 프리팹을 생성
        T sceneUI = Util.GetOrAddComponent<T>(go);                                  //T 컴포넌트를 불러옴
        m_sceneUI = sceneUI;

        go.transform.SetParent(Root.transform);                                     //@UI_Root 하위에 생성

        return sceneUI;                                                             //컴포넌트 리턴
    }

    /// <summary>
    /// 일반 팝업을 보여주기 위한 메소드
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
    /// 원하는 팝업을 닫기 위한 메소드
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
    /// 가장 상위 팝업을 닫기 위한 메소드
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
    /// 모든 팝업을 한번에 닫기 위한 메소드
    /// </summary>
    public void CloseAllPopUpUI()
    {
        while (m_popupStack.Count > 0) ClosePopUpUI();
    }

    /// <summary>
    /// 초기화 하기 위한 메소드
    /// </summary>
    public void Clear()
    {
        CloseAllPopUpUI();
        m_sceneUI = null;
    }
}
