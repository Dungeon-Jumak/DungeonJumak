//System
using System;
using System.Collections.Generic;

//Engine
using UnityEngine;


public class TransformEventArgs : EventArgs
{
    public Transform m_Transform;
    public object[] m_Value;

    public TransformEventArgs(Transform _transform, params object[] _value)
    {
        m_Transform = _transform;
        m_Value = _value;
    }
}

/// <summary>
/// 이벤트를 관리하는 싱글톤 이벤트 매니저 클래스입니다.
/// 다양한 열거형 타입에 대해 이벤트를 등록하고 실행할 수 있습니다.
/// </summary>
/// <typeparam name="TEnum">열거형 타입</typeparam>

public class EventManager<TEnum> where TEnum : Enum
{
    #region Singleton

    // 싱글톤 인스턴스
    private static EventManager<TEnum> s_Instance;
    public static EventManager<TEnum> Instance
    {
        get
        {
            if (s_Instance == null)
            {
                s_Instance = new EventManager<TEnum>();
            }
            return s_Instance;
        }
    }

    #endregion

    public delegate void OnEvent(TEnum _eventType, Component _sender, TransformEventArgs _args = null);
    private Dictionary<TEnum, List<OnEvent>> m_Listeners = new Dictionary<TEnum, List<OnEvent>>();

    /// <summary>
    /// 리스너를 추가하는 함수
    /// </summary>
    /// <param name="_eventType">이벤트 타입</param>
    /// <param name="_listener">이벤트 리스너</param>
    public void AddListener(TEnum _eventType, OnEvent _listener)
    {
        if (!m_Listeners.TryGetValue(_eventType, out var listenList))
        {
            listenList = new List<OnEvent>();
            m_Listeners.Add(_eventType, listenList);
        }
        listenList.Add(_listener);
    }

    /// <summary>
    /// 이벤트를 발생시키는 함수
    /// </summary>
    /// <param name="_eventType">이벤트 타입</param>
    /// <param name="_sender">이벤트를 발생시킨 객체</param>
    /// <param name="_args">이벤트와 함께 전달되는 추가 데이터</param>
    public void PostNotification(TEnum _eventType, Component _sender, TransformEventArgs _args = null)
    {
        if (!m_Listeners.TryGetValue(_eventType, out var listenList))
            return;

        foreach (var listener in listenList)
        {
            listener?.Invoke(_eventType, _sender, _args);
        }
    }

    /// <summary>
    /// 리스너를 제거하는 함수
    /// </summary>
    /// <param name="_eventType">이벤트 타입</param>
    /// <param name="_target">제거할 타겟 객체</param>
    public void RemoveListener(TEnum _eventType, object _target)
    {
        if (!m_Listeners.ContainsKey(_eventType))
            return;

        List<OnEvent> listenList = m_Listeners[_eventType];
        for (int i = listenList.Count - 1; i >= 0; i--)
        {
            if (listenList[i].Target == _target)
            {
                listenList.RemoveAt(i);
                Debug.Log("Event Listener Removed Successfully"); //Debug
                return;
            }
        }
        Debug.Log("Failed to Remove Event Listener"); //Debug
    }

    /// <summary>
    /// 특정 이벤트 타입의 모든 리스너를 제거하는 함수
    /// </summary>
    /// <param name="_eventType">이벤트 타입</param>
    public void RemoveEvent(TEnum _eventType)
    {
        m_Listeners.Remove(_eventType);
    }

    /// <summary>
    /// null 값이 포함된 리스너들을 제거하는 함수
    /// </summary>
    public void RemoveRedundancies()
    {
        Dictionary<TEnum, List<OnEvent>> newListeners = new Dictionary<TEnum, List<OnEvent>>();

        foreach (var item in m_Listeners)
        {
            for (int i = item.Value.Count - 1; i >= 0; i--)
            {
                if (item.Value[i].Equals(null))
                {
                    item.Value.RemoveAt(i);
                }
            }

            if (item.Value.Count > 0)
            {
                newListeners.Add(item.Key, item.Value);
            }
        }

        m_Listeners = newListeners;
    }
}
