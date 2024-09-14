//System
using System.Collections;
using System.Collections.Generic;

//Unity
using UnityEngine;

public class Util : MonoBehaviour
{
    /// <summary>
    /// ���� ������Ʈ�� ������Ʈ�� �������ų� �߰��ϱ� ���� �޼ҵ�
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="_go"></param>
    /// <returns></returns>
    public static T GetOrAddComponent<T> (GameObject _go) where T : UnityEngine.Component
    {
        T component = _go.GetComponent<T>();                        //���� ������Ʈ�� ������Ʈ�� ������

        if (component == null) component = _go.AddComponent<T>();   //���� ��� �ִٸ� �ش��ϴ� ������Ʈ�� �߰��ϰ� ������
                
        return component;                                           //������Ʈ ����
    }

    /// <summary>
    /// �ڽĵ� �� GameObject�� �������� ���� �޼ҵ�
    /// </summary>
    /// <param name="_go"></param>
    /// <param name="_name"></param>
    /// <param name="_recursive"></param>
    /// <returns></returns>
    public static GameObject FindChild(GameObject _go, string _name = null, bool _recursive = false)
    {
        Transform transform = FindChild<Transform>(_go, _name, _recursive); //�ڽ� ������Ʈ�� Ž��
        
        if (transform == null) return null;                                 //ã�� ���ߴٸ� null�� ����

        return transform.gameObject;                                        //ã�� GameObject ����
    }

    /// <summary>
    /// go ������Ʈ�� ��� �ڽĵ� �� "T component�� ���� �̸��� ���� �ڽ� ������Ʈ"�� ã�� ���� �޼ҵ�
    /// ���� �Ķ���Ϳ� �̸��� ���� ���� ��� ������Ʈ�� �ش�Ǵ� �͸� ã���� �ȴ�.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="_go"></param>
    /// <param name="_name"></param>
    /// <param name="_recursive"></param>
    /// <returns></returns>
    public static T FindChild<T> (GameObject _go, string _name = null, bool _recursive = false) where T : UnityEngine.Object
    {
        if (_go == null) return null;                                           //���� ������Ʈ�� ��� �ִٸ� ���� X

        if (_recursive == false)                                                //���� �ڽ� ������Ʈ�� Ž��
        {
            for (int i = 0; i < _go.transform.childCount; i++)                  //�Ķ���Ϳ��� ���� ���� ������Ʈ�� �ڽ� ������Ʈ ����ŭ �ݺ�
            {
                Transform transform = _go.transform.GetChild(i);                //������� �ڽ� ������Ʈ�� Ʈ�������� �ҷ���

                if (string.IsNullOrEmpty(_name) || transform.name == _name)     //���� �̸��� ���ų� �ڽ� �������� �̸��� ���ٸ�
                {
                    T component = transform.GetComponent<T>();                  //������Ʈ�� �������� ����
                    if (component != null)
                        return component;
                }
            }
        }
        else
        {
            foreach(T component in _go.GetComponentsInChildren<T>())            //��� �ڽ� ������Ʈ�� Ž��
            {
                if (string.IsNullOrEmpty(_name) || component.name == _name)     //���� �̸��� ���ų�, ������ �������� �̸��� ���ٸ�
                    return component;                                           //������Ʈ ����
            }
        }

        return null;

    }
}
