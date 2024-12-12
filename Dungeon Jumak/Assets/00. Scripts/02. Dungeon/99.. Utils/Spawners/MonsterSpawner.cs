using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class SpawnData
{
    public float spawnTime;         // 시간 (초 단위)
    public Monster monsterPrefab;   // 스폰할 몬스터 프리팹
    public int spawnCount;          // 스폰 수
}

public class MonsterSpawner : MonoBehaviour
{
    [Header("스폰 데이터 목록")]
    [SerializeField] private List<SpawnData> spawnDataList = new List<SpawnData>();

    [Header("플레이어 트랜스폼")]
    [SerializeField] private Transform playerTransform;

    [Header("랜덤 스폰 범위")]
    [SerializeField] private Vector2 randomSpawnRange = new Vector2(50f, 50f);

    private PoolManager<Monster> poolManager;
    private float timer;
    private List<SpawnData> processedSpawns = new List<SpawnData>();

    private void Start()
    {
        // 풀 매니저 초기화
        poolManager = new PoolManager<Monster>(transform);

        // 각 몬스터에 대한 풀 생성
        foreach (var spawnData in spawnDataList)
        {
            poolManager.CreatePool(spawnData.monsterPrefab, spawnData.spawnCount);
        }
    }

    private void Update()
    {
        timer += Time.deltaTime;

        // Check each SpawnData
        foreach (var spawnData in spawnDataList)
        {
            // If it's time to spawn this data
            if (timer >= spawnData.spawnTime && !processedSpawns.Contains(spawnData))
            {
                SpawnMonsters(spawnData);
                processedSpawns.Add(spawnData); // Mark as processed
            }
        }
    }

    private void SpawnMonsters(SpawnData spawnData)
    {
        for (int i = 0; i < spawnData.spawnCount; i++)
        {
            // 풀에서 몬스터 가져오기
            Monster monster = poolManager.GetFromPool(spawnData.monsterPrefab);

            // 스폰 위치 설정 (플레이어 근처 랜덤 위치 또는 고정 위치)
            Vector3 spawnPosition = GetRandomSpawnPosition();
            monster.transform.position = spawnPosition;
        }
    }

    private Vector3 GetRandomSpawnPosition()
    {
        // 플레이어 근처에서 랜덤 위치 계산
        if (playerTransform != null)
        {
            float randomX = Random.Range(-randomSpawnRange.x, randomSpawnRange.x);
            float randomY = Random.Range(-randomSpawnRange.y, randomSpawnRange.y);
            return playerTransform.position + new Vector3(randomX, randomY, 0f);
        }

        // 기본 위치 반환 (디버그 용도로 사용)
        return Vector3.zero;
    }
}
