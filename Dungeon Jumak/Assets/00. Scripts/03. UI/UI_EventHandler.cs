//System
using System;
using System.Collections;
using System.Collections.Generic;

//Unity
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EventHandler : MonoBehaviour, IPointerClickHandler, IDragHandler
{
    public Action<PointerEventData> m_onClickHandler = null;
    public Action<PointerEventData> m_onDragHandler = null;

    public void OnPointerClick(PointerEventData _eventData)
    {
        if (m_onClickHandler != null)
            m_onClickHandler.Invoke(_eventData);
    }

    public void OnDrag(PointerEventData _eventData)
    {
        if (m_onDragHandler != null)
            m_onDragHandler.Invoke(_eventData);
    }
}
