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
/// �̺�Ʈ�� �����ϴ� �̱��� �̺�Ʈ �Ŵ��� Ŭ�����Դϴ�.
/// �پ��� ������ Ÿ�Կ� ���� �̺�Ʈ�� ����ϰ� ������ �� �ֽ��ϴ�.
/// </summary>
/// <typeparam name="TEnum">������ Ÿ��</typeparam>

public class EventManager<TEnum> where TEnum : Enum
{
    #region Singleton

    // �̱��� �ν��Ͻ�
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
    /// �����ʸ� �߰��ϴ� �Լ�
    /// </summary>
    /// <param name="_eventType">�̺�Ʈ Ÿ��</param>
    /// <param name="_listener">�̺�Ʈ ������</param>
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
    /// �̺�Ʈ�� �߻���Ű�� �Լ�
    /// </summary>
    /// <param name="_eventType">�̺�Ʈ Ÿ��</param>
    /// <param name="_sender">�̺�Ʈ�� �߻���Ų ��ü</param>
    /// <param name="_args">�̺�Ʈ�� �Բ� ���޵Ǵ� �߰� ������</param>
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
    /// �����ʸ� �����ϴ� �Լ�
    /// </summary>
    /// <param name="_eventType">�̺�Ʈ Ÿ��</param>
    /// <param name="_target">������ Ÿ�� ��ü</param>
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
    /// Ư�� �̺�Ʈ Ÿ���� ��� �����ʸ� �����ϴ� �Լ�
    /// </summary>
    /// <param name="_eventType">�̺�Ʈ Ÿ��</param>
    public void RemoveEvent(TEnum _eventType)
    {
        m_Listeners.Remove(_eventType);
    }

    /// <summary>
    /// null ���� ���Ե� �����ʵ��� �����ϴ� �Լ�
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
