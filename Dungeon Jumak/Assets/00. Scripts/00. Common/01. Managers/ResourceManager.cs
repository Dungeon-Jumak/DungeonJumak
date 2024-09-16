using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class ResourceManager
{
    public T Load<T>(string path) where T : Object
    {
        return Resources.Load<T>(path);
    }

    //---Instantiate �Լ�---//
    //GameManager.Resource.Instantiate("�ν��Ͻ�ȭ �ϰ� ���� Prefabs ���� ���� �̸�")
    public GameObject Instantiate(string path, Transform parent = null)
    {
        GameObject prefab = Load<GameObject>($"Prefabs/{path}");
        if (prefab == null)
        {
            Debug.Log($"prefab loading fail : {path}");
            return null;
        }

        //---(Clone) �ؽ�Ʈ ����---//
        GameObject go = Object.Instantiate(prefab, parent);
        int index = go.name.IndexOf("(Clone)");
        if (index > 0)
        {
            go.name = go.name.Substring(0, index);
        }
        return go;
    }

    //---destroy �Լ�---//
    //GameManager.Resource.Destroy(���ӿ�����Ʈ)
    public void Destroy(GameObject go)
    {
        if (go == null)
        {
            return;
        }

        Object.Destroy(go);
    }
}