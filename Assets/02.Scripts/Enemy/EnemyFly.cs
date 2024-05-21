using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class EnemyFly : MonoBehaviour
{
    [SerializeField] private EnemyData enemyData; // 적 데이터 스크립터블 오브젝트
    private EnemyDamage enemyDamage; // 적의 데미지 컴포넌트
    private Transform playerTransform; // 플레이어의 트랜스폼
    private Tweener tweener; // 적의 이동 애니메이션 관리
    private bool isPlaying = true; // 애니메이션 플레이 상태를 나타내는 변수

    void Start()
    {
        playerTransform = GameObject.Find("Player").transform; // 플레이어의 Transform 가져오기
        enemyDamage = GetComponent<EnemyDamage>(); // EnemyDamage 컴포넌트 가져오기
        MoveSlowlyToPlayer(); // 적을 천천히 플레이어에게 이동시키는 함수 호출
    }

    void Update()
    {
        if (playerTransform != null)
        {
            // 플레이어 쪽을 바라보게 하는 조건문
            Quaternion targetRotation = Quaternion.LookRotation((playerTransform.position - transform.position).normalized);
            targetRotation.z = targetRotation.x = 0; // 회전 축 고정
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime); // 부드럽게 회전
            if (isPlaying)
            {
                // 애니메이션이 플레이 중이라면
                if (enemyDamage.isHit)
                {
                    // 적이 공격을 받고 있다면
                    tweener.Pause(); // 애니메이션 멈추기
                    isPlaying = false; // 플레이 상태 변경
                }
            }
            else
            {
                if (!enemyDamage.isHit)
                {
                    // 적이 공격을 받고 있지 않다면
                    tweener.Play(); // 애니메이션 다시 재생
                    isPlaying = true; // 플레이 상태로 변경
                }
            }
        }

    }

    void MoveSlowlyToPlayer()
    {
        // 플레이어 방향으로 0.5m만큼 천천히 이동
        Vector3 targetPosition = transform.position + (playerTransform.position - transform.position).normalized * 0.5f;
        tweener = transform.DOMove(targetPosition, 2f)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                // 천천히 이동이 완료되면 빠르게 이동하는 함수 호출
                MoveQuicklyToPlayer();
            });
    }

    void MoveQuicklyToPlayer()
    {
        // 플레이어 방향으로 2m만큼 빠르게 이동
        Vector3 targetPosition = transform.position + (playerTransform.position - transform.position).normalized * 2f;
        tweener = transform.DOMove(targetPosition, 0.5f)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                // 빠르게 이동이 완료되면 다시 천천히 이동하는 함수 호출
                MoveSlowlyToPlayer();
            });
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // 충돌한 오브젝트가 플레이어일 경우
            other.GetComponent<PlayerMovement>().PlayerRecieveDamage(enemyData.Damage); // 플레이어에게 데미지 주기
        }
    }
}