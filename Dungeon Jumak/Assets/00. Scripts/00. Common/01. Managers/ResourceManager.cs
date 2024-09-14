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

    //---Instantiate 함수---//
    //GameManager.Resource.Instantiate("인스턴스화 하고 싶은 Prefabs 폴더 안의 이름")
    public GameObject Instantiate(string path, Transform parent = null)
    {
        GameObject prefab = Load<GameObject>($"Prefabs/{path}");
        if (prefab == null)
        {
            Debug.Log($"prefab loading fail : {path}");
            return null;
        }

        //---(Clone) 텍스트 삭제---//
        GameObject go = Object.Instantiate(prefab, parent);
        int index = go.name.IndexOf("(Clone)");
        if (index > 0)
        {
            go.name = go.name.Substring(0, index);
        }
        return go;
    }

    //---destroy 함수---//
    //GameManager.Resource.Destroy(게임오브젝트)
    public void Destroy(GameObject go)
    {
        if (go == null)
        {
            return;
        }

        Object.Destroy(go);
    }
}