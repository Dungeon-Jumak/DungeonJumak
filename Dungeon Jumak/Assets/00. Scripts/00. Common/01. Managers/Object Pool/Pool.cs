using UnityEngine;
using UnityEngine.Pool;

public class Pool : MonoBehaviour
{
    public IObjectPool<GameObject> ObjectPool { get; set; }

    //Pool.Release
    //instance.Pool.Get 사용
}
