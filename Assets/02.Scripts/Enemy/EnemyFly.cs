using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class EnemyFly : MonoBehaviour
{
    [SerializeField] private EnemyData enemyData; // �� ������ ��ũ���ͺ� ������Ʈ
    private EnemyDamage enemyDamage; // ���� ������ ������Ʈ
    private Transform playerTransform; // �÷��̾��� Ʈ������
    private Tweener tweener; // ���� �̵� �ִϸ��̼� ����
    private bool isPlaying = true; // �ִϸ��̼� �÷��� ���¸� ��Ÿ���� ����

    void Start()
    {
        playerTransform = GameObject.Find("Player").transform; // �÷��̾��� Transform ��������
        enemyDamage = GetComponent<EnemyDamage>(); // EnemyDamage ������Ʈ ��������
        MoveSlowlyToPlayer(); // ���� õõ�� �÷��̾�� �̵���Ű�� �Լ� ȣ��
    }

    void Update()
    {
        if (playerTransform != null)
        {
            // �÷��̾� ���� �ٶ󺸰� �ϴ� ���ǹ�
            Quaternion targetRotation = Quaternion.LookRotation((playerTransform.position - transform.position).normalized);
            targetRotation.z = targetRotation.x = 0; // ȸ�� �� ����
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime); // �ε巴�� ȸ��
            if (isPlaying)
            {
                // �ִϸ��̼��� �÷��� ���̶��
                if (enemyDamage.isHit)
                {
                    // ���� ������ �ް� �ִٸ�
                    tweener.Pause(); // �ִϸ��̼� ���߱�
                    isPlaying = false; // �÷��� ���� ����
                }
            }
            else
            {
                if (!enemyDamage.isHit)
                {
                    // ���� ������ �ް� ���� �ʴٸ�
                    tweener.Play(); // �ִϸ��̼� �ٽ� ���
                    isPlaying = true; // �÷��� ���·� ����
                }
            }
        }

    }

    void MoveSlowlyToPlayer()
    {
        // �÷��̾� �������� 0.5m��ŭ õõ�� �̵�
        Vector3 targetPosition = transform.position + (playerTransform.position - transform.position).normalized * 0.5f;
        tweener = transform.DOMove(targetPosition, 2f)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                // õõ�� �̵��� �Ϸ�Ǹ� ������ �̵��ϴ� �Լ� ȣ��
                MoveQuicklyToPlayer();
            });
    }

    void MoveQuicklyToPlayer()
    {
        // �÷��̾� �������� 2m��ŭ ������ �̵�
        Vector3 targetPosition = transform.position + (playerTransform.position - transform.position).normalized * 2f;
        tweener = transform.DOMove(targetPosition, 0.5f)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                // ������ �̵��� �Ϸ�Ǹ� �ٽ� õõ�� �̵��ϴ� �Լ� ȣ��
                MoveSlowlyToPlayer();
            });
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // �浹�� ������Ʈ�� �÷��̾��� ���
            other.GetComponent<PlayerMovement>().PlayerRecieveDamage(enemyData.Damage); // �÷��̾�� ������ �ֱ�
        }
    }
}