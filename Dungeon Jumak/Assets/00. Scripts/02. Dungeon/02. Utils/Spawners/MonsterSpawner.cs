// System
using UnityEngine;
using System.Collections.Generic;

public class MonsterSpawner : MonoBehaviour
{
    [Header("스폰할 프리팹")]
    [SerializeField] private Monster prefab;

    [Header("스폰 딜레이 (초 단위)")]
    [SerializeField] private float spawnDelay = 1.0f;

    [Header("풀 생성 개수")]
    [SerializeField] private int maxSpawnCount = 5;

    [Header("스폰 포인트")]
    [SerializeField] private Transform[] spawnPoints;

    private PoolManager<Monster> poolManager;
    private float timer;

    // 스폰되지 않은 스폰 포인트 목록
    private List<Transform> availableSpawnPoints;

    private void Start()
    {
        // 풀 매니저 초기화
        poolManager = new PoolManager<Monster>(transform);
        poolManager.CreatePool(prefab, maxSpawnCount);

        // 스폰 포인트 초기화
        availableSpawnPoints = new List<Transform>(spawnPoints);
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
        // 스폰할 수 있는 포인트가 없다면 더 이상 스폰하지 않음
        if (availableSpawnPoints.Count == 0)
        {
            Debug.Log("모든 스폰 포인트가 사용되었습니다.");
            this.enabled = false; // 스크립트 비활성화
            return;
        }

        // 랜덤으로 스폰 포인트 선택
        int spawnIndex = Random.Range(0, availableSpawnPoints.Count);
        Transform spawnPoint = availableSpawnPoints[spawnIndex];

        // 풀에서 몬스터 가져오기
        Monster monster = poolManager.GetFromPool(prefab);

        // 몬스터 위치 설정
        monster.transform.position = spawnPoint.position;

        // 사용된 스폰 포인트 제거
        availableSpawnPoints.RemoveAt(spawnIndex);
    }

    public void ReturnToPool(Monster monster)
    {
        poolManager.ReturnToPool(prefab, monster);
    }
}
