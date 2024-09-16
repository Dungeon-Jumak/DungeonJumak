//System
using System.Collections;
using System.Collections.Generic;

//Unity
using UnityEngine;

public class Util : MonoBehaviour
{
    /// <summary>
    /// 게임 오브젝트의 컴포넌트를 가져오거나 추가하기 위한 메소드
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="_go"></param>
    /// <returns></returns>
    public static T GetOrAddComponent<T> (GameObject _go) where T : UnityEngine.Component
    {
        T component = _go.GetComponent<T>();                        //게임 오브젝트의 컴포넌트를 가져옴

        if (component == null) component = _go.AddComponent<T>();   //만약 비어 있다면 해당하는 컴포넌트를 추가하고 가져옴
                
        return component;                                           //컴포넌트 리턴
    }

    /// <summary>
    /// 자식들 중 GameObject만 가져오기 위한 메소드
    /// </summary>
    /// <param name="_go"></param>
    /// <param name="_name"></param>
    /// <param name="_recursive"></param>
    /// <returns></returns>
    public static GameObject FindChild(GameObject _go, string _name = null, bool _recursive = false)
    {
        Transform transform = FindChild<Transform>(_go, _name, _recursive); //자식 오브젝트를 탐색
        
        if (transform == null) return null;                                 //찾지 못했다면 null값 리턴

        return transform.gameObject;                                        //찾은 GameObject 리턴
    }

    /// <summary>
    /// go 오브젝트의 모든 자식들 중 "T component를 갖고 이름이 같은 자식 오브젝트"를 찾기 위한 메소드
    /// 만약 파라미터에 이름을 넣지 않을 경우 컴포넌트에 해당되는 것만 찾으면 된다.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="_go"></param>
    /// <param name="_name"></param>
    /// <param name="_recursive"></param>
    /// <returns></returns>
    public static T FindChild<T> (GameObject _go, string _name = null, bool _recursive = false) where T : UnityEngine.Object
    {
        if (_go == null) return null;                                           //게임 오브젝트가 비어 있다면 실행 X

        if (_recursive == false)                                                //직속 자식 오브젝트만 탐색
        {
            for (int i = 0; i < _go.transform.childCount; i++)                  //파라미터에서 받은 게임 오브젝트의 자식 오브젝트 수만큼 반복
            {
                Transform transform = _go.transform.GetChild(i);                //순서대로 자식 오브젝트의 트랜스폼을 불러옴

                if (string.IsNullOrEmpty(_name) || transform.name == _name)     //만약 이름이 없거나 자식 오브젝의 이름과 같다면
                {
                    T component = transform.GetComponent<T>();                  //컴포넌트를 가져오고 리턴
                    if (component != null)
                        return component;
                }
            }
        }
        else
        {
            foreach(T component in _go.GetComponentsInChildren<T>())            //모든 자식 오브젝트를 탐색
            {
                if (string.IsNullOrEmpty(_name) || component.name == _name)     //만약 이름이 없거나, 가져온 컴포넌의 이름과 같다면
                    return component;                                           //컴포넌트 리턴
            }
        }

        return null;

    }
}
