using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySwarm : MonoBehaviour
{
    [SerializeField] private EnemyData enemyData; // �� ������ ��ũ���ͺ� ������Ʈ
    private NavMeshAgent agent; // �׺�޽� ������Ʈ ������Ʈ
    private EnemyDamage enemyDamage; // ���� ������ ������Ʈ
    private Transform playerTransform; // �÷��̾��� Ʈ������
    private Animator animator; // ���� �ִϸ����� ������Ʈ
    private float stoppingDistance = 0.5f; // ���� �÷��̾�� ������ �� ���ߴ� �Ÿ�
    [SerializeField] private float attackRadius = 0.8f; // ���� ���� �ݰ�

    void Start()
    {
        agent = GetComponent<NavMeshAgent>(); // NavMeshAgent ������Ʈ ��������
        animator = GetComponent<Animator>(); // Animator ������Ʈ ��������
        playerTransform = GameObject.Find("Player").transform; // �÷��̾��� Transform ��������
        enemyDamage = GetComponent<EnemyDamage>(); // EnemyDamage ������Ʈ ��������
    }

    void Update()
    {
        agent.destination = playerTransform.position; // �÷��̾ �������� ����

        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position); // ���� �÷��̾� ������ �Ÿ� ���

        if (enemyDamage.isHit || enemyDamage.isDie) // ���� ������ �ްų� ���� �����̸�
        {
            agent.isStopped = true; // �̵� ����
            animator.SetBool("IsWalk", false); // �ȱ� �ִϸ��̼� ����
        }
        else if (distanceToPlayer < stoppingDistance && !enemyDamage.isHit) // ���� �÷��̾�� �������� ��
        {
            agent.isStopped = true; // �̵� ����
            animator.SetTrigger("AttackTrigger"); // ���� �ִϸ��̼� Ʈ����
            animator.SetBool("IsWalk", false); // �ȱ� �ִϸ��̼� ����
        }
        else // ���� �÷��̾�� �������� �ʾ��� ��
        {
            agent.isStopped = false; // �̵� �簳
            animator.SetBool("IsWalk", true); // �ȱ� �ִϸ��̼� ���
        }
    }

    public void EnemyAttack()
    {
        // ���� �ݰ� ���� �÷��̾� ã��
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRadius, 1 << 3); // ���� �ݰ� ���� �÷��̾� �ݶ��̴� ã��

        foreach (Collider col in hitColliders)
        {
            col.GetComponent<PlayerMovement>().PlayerRecieveDamage(enemyData.Damage); // �÷��̾�� ������ �ֱ�
        }
    }
}