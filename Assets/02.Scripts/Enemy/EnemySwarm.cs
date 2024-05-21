using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySwarm : MonoBehaviour
{
    [SerializeField] private EnemyData enemyData; // 적 데이터 스크립터블 오브젝트
    private NavMeshAgent agent; // 네브메쉬 에이전트 컴포넌트
    private EnemyDamage enemyDamage; // 적의 데미지 컴포넌트
    private Transform playerTransform; // 플레이어의 트랜스폼
    private Animator animator; // 적의 애니메이터 컴포넌트
    private float stoppingDistance = 0.5f; // 적이 플레이어에게 접근할 때 멈추는 거리
    [SerializeField] private float attackRadius = 0.8f; // 적의 공격 반경

    void Start()
    {
        agent = GetComponent<NavMeshAgent>(); // NavMeshAgent 컴포넌트 가져오기
        animator = GetComponent<Animator>(); // Animator 컴포넌트 가져오기
        playerTransform = GameObject.Find("Player").transform; // 플레이어의 Transform 가져오기
        enemyDamage = GetComponent<EnemyDamage>(); // EnemyDamage 컴포넌트 가져오기
    }

    void Update()
    {
        agent.destination = playerTransform.position; // 플레이어를 목적지로 설정

        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position); // 적과 플레이어 사이의 거리 계산

        if (enemyDamage.isHit || enemyDamage.isDie) // 적이 공격을 받거나 죽은 상태이면
        {
            agent.isStopped = true; // 이동 중지
            animator.SetBool("IsWalk", false); // 걷기 애니메이션 중지
        }
        else if (distanceToPlayer < stoppingDistance && !enemyDamage.isHit) // 적이 플레이어에게 접근했을 때
        {
            agent.isStopped = true; // 이동 중지
            animator.SetTrigger("AttackTrigger"); // 공격 애니메이션 트리거
            animator.SetBool("IsWalk", false); // 걷기 애니메이션 중지
        }
        else // 적이 플레이어에게 접근하지 않았을 때
        {
            agent.isStopped = false; // 이동 재개
            animator.SetBool("IsWalk", true); // 걷기 애니메이션 재생
        }
    }

    public void EnemyAttack()
    {
        // 공격 반경 내의 플레이어 찾기
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRadius, 1 << 3); // 공격 반경 내의 플레이어 콜라이더 찾기

        foreach (Collider col in hitColliders)
        {
            col.GetComponent<PlayerMovement>().PlayerRecieveDamage(enemyData.Damage); // 플레이어에게 데미지 주기
        }
    }
}