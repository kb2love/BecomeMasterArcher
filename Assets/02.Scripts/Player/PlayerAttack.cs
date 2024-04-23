using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] PlayerData playerData;
    private Animator animator;
    private TouchPad touchPad;
    private Vector3 enemyPos = Vector3.zero;
    [SerializeField] private List<Transform> enemiesList = new List<Transform>();
    private bool isZoom = false;
    [SerializeField] private bool isFind = true;
    void Start()
    {
        touchPad = GameObject.Find("TouchPad_Image").GetComponent<TouchPad>();
        animator = transform.GetChild(0).GetComponent<Animator>();
        Transform parentTransform = GameObject.Find("Enemies").transform;
        for (int i = 0; i < parentTransform.childCount; i++)
        {
            enemiesList.Add(parentTransform.GetChild(i));
        }
    }

    void Update()
    {
        if (touchPad.buttonPress)
        {//움직일떄 공격x
            isFind = false;
            animator.SetBool("IsWalk", true);

        }
        else
        {//멈춰서 공격o
            isFind = true;
            if (isFind)
            {
                float shortestDistance = Mathf.Infinity;
                foreach (Transform enemy in enemiesList)
                {
                    float distanceToEnemy = Vector3.Distance(transform.position, enemy.position); // 플레이어와 적 사이의 거리 계산
                    if (distanceToEnemy < shortestDistance)
                    {
                        shortestDistance = distanceToEnemy; // 현재 적과의 거리가 현재까지의 가장 짧은 거리보다 짧으면 갱신
                        enemyPos = enemy.position; // 가장 가까운 적의 위치를 현재 적의 위치로 설정
                    }
                }
                Quaternion rot = Quaternion.LookRotation((enemyPos - transform.position).normalized);
                rot.x = rot.z = 0f;
                transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * 10.0f);
                animator.SetBool("IsWalk", false);
                animator.SetTrigger("AttackTrigger");
            }
            else
            {
                animator.SetBool("IsWalk", true);
            }
        }

    }
    public void IsFind()
    {
        isFind = false;
    }
}
