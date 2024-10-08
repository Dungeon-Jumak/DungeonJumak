//System
using System;
using System.Collections;
using System.Collections.Generic;

//Unity
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

//모든 UI들의 조상 클래스
public class UI_Base : MonoBehaviour
{
    protected Dictionary<Type, UnityEngine.Object[]> m_objects = new Dictionary<Type, UnityEngine.Object[]>();

    /// <summary>
    /// UI의 이름을 찾아 바인딩해주는 함수
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="type"></param>
    protected void Bind<T> (Type _type) where T : UnityEngine.Object
    {
        string[] names = Enum.GetNames(_type);                                  //해당하는 타임의 Enum안의 요소들을 배열에 불러옴
        UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];    //불러온 요소 배열의 길이 만큼 오브젝트 배열을 생성
        m_objects.Add(typeof(T), objects);                                      //딕셔더리에 오브젝트 추가

        for (int i = 0; i < names.Length; i++)                                  //요소의 길이만큼 반복
        {
            if (typeof(T) == typeof(GameObject))                                //제네릭 타입이 GameObject일 경우
                objects[i] = Util.FindChild(gameObject, names[i], true);        //게임 오브젝트만 찾는 FindChild 호출 (모든 자식들을 탐색)
            else                                                                //제네릭 타임이 GameObjec가 아닐 경우
                objects[i] = Util.FindChild<T>(gameObject, names[i], true);     //FincChild<T> 호출 (모든 자식들을 탐색)


            if (objects[i] == null)                                             //탐색 결과 아무것도 없다면
                Debug.Log($"Failed to bind({names[i]}");                        //바인드 실패!
                
        }
    }

    /// <summary>
    /// T 컴포넌트를 가지며, 인덱스에 해당하는 오브젝트를 T타입으로 가져오기 위한 메소드
    /// Enum을 int로 변환하여 파라미터로 전달
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="_idx"></param>
    /// <returns></returns>
    protected T Get<T> (int _idx) where T : UnityEngine.Object
    {
        UnityEngine.Object[] objects = null;

        if (m_objects.TryGetValue(typeof(T), out objects) == false) //딕셔너리에서 key에 해당하는 value를 저장, 만약 없다면 null을 리턴
            return null;

        return objects[_idx] as T;                                  //해당하는 오브젝트 배열을 T 컴포넌트로 리턴
    }

    protected GameObject GetObject (int idx) { return Get<GameObject>(idx); }
    protected Text GetText(int idx) { return Get<Text>(idx); }

    protected TextMeshProUGUI GetTMP(int idx) { return Get<TextMeshProUGUI>(idx); }

    protected Button GetButton(int idx) { return Get<Button>(idx); }
    protected Image GetImage(int idx) { return Get<Image>(idx); }

    protected Slider GetSlider(int idx) { return Get<Slider>(idx); }

    public static void BindEvent (GameObject _go, Action<PointerEventData> _action, Define.UIEvent _type = Define.UIEvent.Click)
    {
        UI_EventHandler evt = Util.GetOrAddComponent<UI_EventHandler>(_go);

        switch (_type)
        {
            case Define.UIEvent.Click:
                evt.m_onClickHandler -= _action;
                evt.m_onClickHandler += _action;
                break;
            case Define.UIEvent.Drag:
                evt.m_onDragHandler -= _action;
                evt.m_onDragHandler += _action;
                break;
        }

    }
}
