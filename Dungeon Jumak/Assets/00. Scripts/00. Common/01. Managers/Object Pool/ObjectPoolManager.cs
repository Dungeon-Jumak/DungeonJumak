//Unity
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPoolManager : MonoBehaviour
{
    [SerializeField] private int m_defaultCapacity = 10;
    [SerializeField] private int m_maxPoolSize = 15;

    [SerializeField] private GameObject m_parent;

    [SerializeField] private GameObject m_prefab;        //프리팹

    public IObjectPool<GameObject> Pool { get; private set; }   //유니티 내장 오브젝트 풀 프로퍼티

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        Pool = new ObjectPool<GameObject>(CreatePoolItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, true, m_defaultCapacity, m_maxPoolSize);
    }

    private GameObject CreatePoolItem()
    {
        GameObject poolGameObject = Instantiate(m_prefab);
        poolGameObject.GetComponent<Pool>().ObjectPool = this.Pool;

        if(m_parent != null) poolGameObject.transform.SetParent(this.transform);

        return poolGameObject;
    }

    private void OnTakeFromPool(GameObject _poolGameObject)
    {
        _poolGameObject.SetActive(true);
    }

    private void OnReturnedToPool(GameObject _poolGameObject)
    {
        _poolGameObject.SetActive(false);   
    }

    private void OnDestroyPoolObject(GameObject _poolGameObject)
    {
        Destroy(_poolGameObject);
    }
}
