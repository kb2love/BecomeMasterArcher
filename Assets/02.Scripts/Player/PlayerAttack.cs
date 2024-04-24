using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] PlayerData playerData;
    private Animator animator;
    private TouchPad touchPad;
    [SerializeField] private List<Transform> enemiesList = new List<Transform>();
    [SerializeField] private bool isFind = true;

    private Transform attackPos;
    private int idx = 0;
    void Start()
    {
        touchPad = GameObject.Find("TouchPad_Image").GetComponent<TouchPad>();
        animator = transform.GetChild(0).GetComponent<Animator>();
        animator.SetFloat("AttackSpeed", playerData.plAtcSpeed);
        Transform parentTransform = GameObject.Find("Enemies").transform;
        for (int i = 0; i < parentTransform.childCount; i++)
        {
            enemiesList.Add(parentTransform.GetChild(i));
        }
        if(enemiesList.Count > 0)
        {
            FindEnemy();
        }
        attackPos = GameObject.Find("AttackPos").transform;
    }

    void Update()
    {
        if (touchPad.buttonPress)
        {//�����ϋ� ����x
            isFind = true;
            animator.SetBool("IsWalk", true);

        }
        else
        {//���缭 ����o
            if (enemiesList.Count > 0)
            {

                FindEnemy();
                Quaternion rot = Quaternion.LookRotation((enemiesList[idx].position - transform.position).normalized);
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

    private void FindEnemy()
    {
        if (isFind)
        {
            float shortestDistance = Mathf.Infinity;
            idx = 0;
            for (int i = 0; i < enemiesList.Count; i++)
            {
                float distanceToEnemy = Vector3.Distance(transform.position, enemiesList[i].position); // �÷��̾�� �� ������ �Ÿ� ���
                
                if (distanceToEnemy < shortestDistance)
                {
                    idx = i;
                    shortestDistance = distanceToEnemy; // ���� ������ �Ÿ��� ��������� ���� ª�� �Ÿ����� ª���� ����
                    isFind = false;
                    break;
                }
            }
        }
    }
    public void Attack()
    {
        if (ObjectPoolingManager.objInstance.GetArrow() != null)
        {
            GameObject arrow = ObjectPoolingManager.objInstance.GetArrow();
            arrow.transform.position = attackPos.position;
            arrow.transform.rotation = attackPos.rotation;
            arrow.SetActive(true);
        }
    }
    public void EnemyDie(Transform tr)
    {
        isFind = true;
        idx = 0;
        enemiesList.Remove(tr);
        FindEnemy();
        
    }
}
