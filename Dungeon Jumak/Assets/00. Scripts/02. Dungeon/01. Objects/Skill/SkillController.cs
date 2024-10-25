//System
using System.Collections;
using UnityEngine;

// Ect
using Data.Object;

namespace Skill.Controller
{
    public class SkillController : MonoBehaviour
    {
        [Header("SO")]
        public SkillDataSO dataSO;

        [Header("스킬 사용 가능 여부")]
        private bool canSkill;

        [Header("스캐너")]
        [SerializeField] private Scanner scanner;

        [Header("풀 생성용 프리팹")]
        [SerializeField] private Skill prefab;

        [Header("풀 생성 개수")]
        [SerializeField] private int maxSpawnCount = 5;

        private float currentDuration = 0f;
        private float timer = 0f;
        private PoolManager<Skill> poolManager;
        private Coroutine autoFireBallCoroutine;

        private void Start()
        {
            // 풀 매니저 초기화
            poolManager = new PoolManager<Skill>(transform);
            poolManager.CreatePool(prefab, maxSpawnCount);

            if (dataSO.skillId == 0) // 0번 스킬이 FireBall이라 가정
            {
                // 자동 FireBall 발사 코루틴 시작
                autoFireBallCoroutine = StartCoroutine(AutoFireBall());
            }
        }

        private void Update()
        {
            // CoolTime 관리
            CoolTime();

            // 스킬 ID에 따라 각각의 처리
            switch (dataSO.skillId)
            {
                case 0: // FireBall (자동 발사)
                    break;
                case 1: // FireRing (플레이어 주변을 도는 불꽃 고리)
                    HandleFireRing();
                    break;
                default:
                    break;
            }
        }

        #region CoolTime 관리

        private void CoolTime()
        {
            if (!canSkill)
            {
                timer += Time.deltaTime;
                if (timer > dataSO.coolTime)
                {
                    canSkill = true;
                    timer = 0f;
                }
            }
        }

        #endregion

        #region FireBall

        private IEnumerator AutoFireBall()
        {
            while (true)
            {
                yield return new WaitForSeconds(2f);
                FireBall();
            }
        }

        public void FireBall()
        {
            if (!canSkill || !scanner.nearestTarget) return;

            canSkill = false;

            // 타겟 몬스터 위치
            Vector3 targetPos = scanner.nearestTarget.position;

            // 방향 벡터 계산
            Vector3 direction = (targetPos - transform.position).normalized;

            // 풀링
            Transform fireball = poolManager.GetFromPool(prefab).transform;

            // position 설정
            fireball.position = transform.position;

            // 회전
            fireball.rotation = Quaternion.FromToRotation(Vector3.up, direction);

            // Skill.cs Init() 호출
            fireball.GetComponent<Skill>().Init(direction);
        }

        #endregion

        #region FireRing

        private void HandleFireRing()
        {
            int activeChildCount = GetActiveChildCount(transform);

            if (activeChildCount > 0)
            {
                transform.Rotate(Vector3.back * dataSO.speed * Time.deltaTime);
                currentDuration += Time.deltaTime;

                if (currentDuration >= dataSO.duration)
                {
                    currentDuration = 0f;
                    Demolition();
                }
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }

        public void FireRing()
        {
            if (!canSkill) return;

            canSkill = false;
            Batch();
        }

        private void Batch()
        {
            Transform skillRound = poolManager.GetFromPool(prefab).transform;
            skillRound.transform.SetParent(transform);
            skillRound.localPosition = Vector3.zero;
            skillRound.localRotation = Quaternion.Euler(90, 0, 0);

            for (int i = 0; i < dataSO.count; i++)
            {
                Transform skill = poolManager.GetFromPool(prefab).transform;
                skill.SetParent(transform);
                skill.localPosition = Vector3.zero;
                skill.localRotation = Quaternion.identity;

                Vector3 rotationVec = Vector3.forward * (360f * i / dataSO.count);
                skill.Rotate(rotationVec);
                skill.Translate(skill.up * 2f, Space.World);

                skill.GetComponent<Skill>().Init(Vector3.zero);
            }
        }

        private void Demolition()
        {
            foreach (Transform child in transform)
            {
                if (child == transform) continue;
                child.gameObject.SetActive(false);
            }
        }

        private int GetActiveChildCount(Transform parent)
        {
            int count = 0;
            foreach (Transform child in parent)
            {
                if (child.gameObject.activeSelf)
                {
                    count++;
                }
            }
            return count;
        }

        #endregion
    }
}
