//System
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

//Unity
using UnityEngine;
using UnityEngine.EventSystems;

public static class Extension
{
    public static void BindEvent (this GameObject _go, Action<PointerEventData> _action, Define.UIEvent _type = Define.UIEvent.Click)
    {
        UI_Base.BindEvent(_go, _action, _type);
    }

}
