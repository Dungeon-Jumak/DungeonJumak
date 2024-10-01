//System
using System.Collections.Generic;

// Engine
using UnityEngine;

public class PoolManager<T> where T : Component
{
    // 풀 딕셔너리 생성
    private Dictionary<T, List<T>> poolDictionary = new Dictionary<T, List<T>>();

    // 풀링된 오브젝트들이 생성될 부모 transform 지정
    private Transform poolParent;

    // 생성자
    public PoolManager(Transform poolParent)
    {
        this.poolParent = poolParent;
    }

    // 풀링 생성
    public void CreatePool(T prefab, int initialSize)
    {
        if (!poolDictionary.ContainsKey(prefab))
        {
            poolDictionary[prefab] = new List<T>();

            // initialSize 수 만큼 인스턴스화 
            for (int i = 0; i < initialSize; i++)
            {
                T obj = Object.Instantiate(prefab, poolParent);
                obj.gameObject.SetActive(false);
                poolDictionary[prefab].Add(obj);
            }
        }
    }
    
    // 풀 오브젝트 가져오기
    public T GetFromPool(T prefab)
    {
        // 풀 딕셔너리에 해당 오브젝트 풀 없을 때
        if (!poolDictionary.ContainsKey(prefab))
        {
            Debug.LogWarning("해당 오브젝트의 풀이 생성되지 않았습니다. 따라서 반환할 오브젝트가 없습니다.");
            return null;
        }

        // 풀 딕셔너리에 해당 오브젝트 풀 존재할 때
        foreach (T obj in poolDictionary[prefab])
        {
            if (!obj.gameObject.activeSelf)
            {
                obj.gameObject.SetActive(true);
                return obj;
            }
        }

        //--- 풀 내에 오브젝트 수가 부족할 때 새로 생성 ---//
        T newObj = Object.Instantiate(prefab, poolParent);
        poolDictionary[prefab].Add(newObj);
        return newObj;
    }

    // 오브젝트 다시 풀에 넣기
    public void ReturnToPool(T prefab, T obj)
    {
        obj.gameObject.SetActive(false);
        poolDictionary[prefab].Add(obj);
    }
}
