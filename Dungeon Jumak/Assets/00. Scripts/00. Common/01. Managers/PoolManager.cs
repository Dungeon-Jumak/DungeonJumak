//System
using System.Collections.Generic;

// Engine
using UnityEngine;

public class PoolManager<T> where T : Component
{
    private Dictionary<T, List<T>> poolDictionary = new Dictionary<T, List<T>>();
    private Transform poolParent; // 풀링된 오브젝트들이 생성될 부모 transform 지정

    public PoolManager(Transform poolParent)
    {
        this.poolParent = poolParent;
    }

    public void CreatePool(T prefab, int initialSize)
    {
        if (!poolDictionary.ContainsKey(prefab))
        {
            poolDictionary[prefab] = new List<T>();

            for (int i = 0; i < initialSize; i++)
            {
                T obj = Object.Instantiate(prefab, poolParent);
                obj.gameObject.SetActive(false);
                poolDictionary[prefab].Add(obj);
            }
        }
    }
    
    public T GetFromPool(T prefab)
    {
        if (!poolDictionary.ContainsKey(prefab))
        {
            Debug.LogError($"풀에 {prefab.name}이(가) 없습니다.");
            return null;
        }

        foreach (T obj in poolDictionary[prefab])
        {
            if (!obj.gameObject.activeSelf)
            {
                obj.gameObject.SetActive(true);
                return obj;
            }
        }

        T newObj = Object.Instantiate(prefab, poolParent);
        poolDictionary[prefab].Add(newObj);
        return newObj;
    }
}
