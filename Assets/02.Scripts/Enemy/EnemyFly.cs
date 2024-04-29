using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class EnemyFly : MonoBehaviour
{
    [SerializeField] EnemyData enemyData;
    private EnemyDamage enemyDamage;
    private Transform plTr;
    private Tweener tweener;
    private bool isPlaying = true; // �÷��� ���¸� ��Ÿ���� ����
    void Start()
    {
        plTr = GameObject.Find("Player").transform;
        enemyDamage = GetComponent<EnemyDamage>();
        MoveSlowlyToPlayer();
    }
    void Update()
    {
        if(plTr != null)
        {   //�÷��̾� ���� �ٶ󺸰� �ϴ� ���ǹ�
            Quaternion rot = Quaternion.LookRotation((plTr.position - transform.position).normalized);
            rot.z = rot.x = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, 10f * Time.deltaTime);
        }
        if (isPlaying)
        {   //�ִϸ��̼��� �÷������̶��
            if (enemyDamage.isHit)
            {   //���ʹ̰� �°��ִٸ�
                // �ִϸ��̼� ���߱�
                tweener.Pause();
                isPlaying = false; // �÷��� ����
            }
        }
        else
        {
            if (!enemyDamage.isHit)
            {   //���ʹ̰� �°������ʴٸ�
                // �ִϸ��̼� �ٽ� ���
                tweener.Play();
                isPlaying = true; // �÷��� ���·� ����
            }
        }
 
    }
    void MoveSlowlyToPlayer()
    {
        // �÷��̾� �������� 0.5m��ŭ õõ�� �̵�
        Vector3 targetPosition = transform.position + (plTr.position - transform.position).normalized * 0.5f;
        transform.DOMove(targetPosition, 2f)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                // õõ�� �̵��� �Ϸ�Ǹ� ������ �̵��ϴ� �Լ� ȣ��
                MoveQuicklyToPlayer();
            });
    }

    void MoveQuicklyToPlayer()
    {
        // �÷��̾� �������� 3m��ŭ ������ �̵�
        Vector3 targetPosition = transform.position + (plTr.position - transform.position).normalized * 2f;
        transform.DOMove(targetPosition, 0.5f)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                // ������ �̵��� �Ϸ�Ǹ� �ٽ� õõ�� �̵��ϴ� �Լ� ȣ��
                MoveSlowlyToPlayer();
            });
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            other.GetComponent<PlayerMovement>().PlayerRecieveDamage(enemyData.Damage);
        }
    }
}
