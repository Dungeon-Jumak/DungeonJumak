// System
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [Header("스폰할 프리팹")]
    [SerializeField] private Monster prefab; 

    [Header("스폰 딜레이 (초 단위)")]
    [SerializeField] private float spawnDelay = 1.0f;

    [Header("최대 스폰 수")]
    [SerializeField] private int maxSpawnCount = 5;

    [Header("스폰 포인트")]
    [SerializeField] private Transform[] spawnPoints;

    private PoolManager<Monster> poolManager;   
    private float timer;

    private void Start()
    {
        poolManager = new PoolManager<Monster>(transform);
        poolManager.CreatePool(prefab, maxSpawnCount);
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnDelay)
        {
            Spawn();
            timer = 0f;
        }
    }

    private void Spawn()
    {
        int spawnIndex = Random.Range(0, spawnPoints.Length);

        Monster monster = poolManager.GetFromPool(prefab);

        monster.transform.position = spawnPoints[spawnIndex].position;
    }

    public void ReturnToPool(Monster monster)
    {
        poolManager.ReturnToPool(prefab, monster);
    }
}
