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
    private bool isPlaying = true; // 플레이 상태를 나타내는 변수
    void Start()
    {
        plTr = GameObject.Find("Player").transform;
        enemyDamage = GetComponent<EnemyDamage>();
        MoveSlowlyToPlayer();
    }
    void Update()
    {
        if(plTr != null)
        {   //플레이어 쪽을 바라보게 하는 조건문
            Quaternion rot = Quaternion.LookRotation((plTr.position - transform.position).normalized);
            rot.z = rot.x = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, 10f * Time.deltaTime);
        }
        if (isPlaying)
        {   //애니메이션이 플레이중이라면
            if (enemyDamage.isHit)
            {   //에너미가 맞고있다면
                // 애니메이션 멈추기
                tweener.Pause();
                isPlaying = false; // 플레이 중지
            }
        }
        else
        {
            if (!enemyDamage.isHit)
            {   //에너미가 맞고있지않다면
                // 애니메이션 다시 재생
                tweener.Play();
                isPlaying = true; // 플레이 상태로 변경
            }
        }
 
    }
    void MoveSlowlyToPlayer()
    {
        // 플레이어 방향으로 0.5m만큼 천천히 이동
        Vector3 targetPosition = transform.position + (plTr.position - transform.position).normalized * 0.5f;
        transform.DOMove(targetPosition, 2f)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                // 천천히 이동이 완료되면 빠르게 이동하는 함수 호출
                MoveQuicklyToPlayer();
            });
    }

    void MoveQuicklyToPlayer()
    {
        // 플레이어 방향으로 3m만큼 빠르게 이동
        Vector3 targetPosition = transform.position + (plTr.position - transform.position).normalized * 2f;
        transform.DOMove(targetPosition, 0.5f)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                // 빠르게 이동이 완료되면 다시 천천히 이동하는 함수 호출
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
