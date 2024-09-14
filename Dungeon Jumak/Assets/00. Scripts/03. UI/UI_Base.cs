//System
using System;
using System.Collections;
using System.Collections.Generic;

//Unity
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//��� UI���� ���� Ŭ����
public class UI_Base : MonoBehaviour
{
    protected Dictionary<Type, UnityEngine.Object[]> m_objects = new Dictionary<Type, UnityEngine.Object[]>();

    /// <summary>
    /// UI�� �̸��� ã�� ���ε����ִ� �Լ�
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="type"></param>
    protected void Bind<T> (Type _type) where T : UnityEngine.Object
    {
        string[] names = Enum.GetNames(_type);                                  //�ش��ϴ� Ÿ���� Enum���� ��ҵ��� �迭�� �ҷ���
        UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];    //�ҷ��� ��� �迭�� ���� ��ŭ ������Ʈ �迭�� ����
        m_objects.Add(typeof(T), objects);                                      //��Ŵ����� ������Ʈ �߰�

        for (int i = 0; i < names.Length; i++)                                  //����� ���̸�ŭ �ݺ�
        {
            if (typeof(T) == typeof(GameObject))                                //���׸� Ÿ���� GameObject�� ���
                objects[i] = Util.FindChild(gameObject, names[i], true);        //���� ������Ʈ�� ã�� FindChild ȣ�� (��� �ڽĵ��� Ž��)
            else                                                                //���׸� Ÿ���� GameObjec�� �ƴ� ���
                objects[i] = Util.FindChild<T>(gameObject, names[i], true);     //FincChild<T> ȣ�� (��� �ڽĵ��� Ž��)


            if (objects[i] == null)                                             //Ž�� ��� �ƹ��͵� ���ٸ�
                Debug.Log($"Failed to bind({names[i]}");                        //���ε� ����!
                
        }
    }

    /// <summary>
    /// T ������Ʈ�� ������, �ε����� �ش��ϴ� ������Ʈ�� TŸ������ �������� ���� �޼ҵ�
    /// Enum�� int�� ��ȯ�Ͽ� �Ķ���ͷ� ����
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="_idx"></param>
    /// <returns></returns>
    protected T Get<T> (int _idx) where T : UnityEngine.Object
    {
        UnityEngine.Object[] objects = null;

        if (m_objects.TryGetValue(typeof(T), out objects) == false) //��ųʸ����� key�� �ش��ϴ� value�� ����, ���� ���ٸ� null�� ����
            return null;

        return objects[_idx] as T;                                  //�ش��ϴ� ������Ʈ �迭�� T ������Ʈ�� ����
    }

    protected GameObject GetObject (int idx) { return Get<GameObject>(idx); }
    protected Text GetText(int idx) { return Get<Text>(idx); }
    protected Button GetButton(int idx) { return Get<Button>(idx); }
    protected Image GetImage(int idx) { return Get<Image>(idx); }

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
