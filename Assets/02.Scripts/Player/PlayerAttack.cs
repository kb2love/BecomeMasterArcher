using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] PlayerData playerData;
    private Animator animator;
    private TouchPad touchPad;
    [SerializeField] private List<float> distances = new List<float>();
    [SerializeField] private bool isFind = true;

    private Transform attackPos;
    private LayerMask enemyLayer = 1 << 6;
    [SerializeField] RaycastHit closeEnemy = new RaycastHit();
    [SerializeField] Transform enemyTr;
    void Start()
    {
        touchPad = GameObject.Find("TouchPad_Image").GetComponent<TouchPad>();
        animator = transform.GetChild(0).GetComponent<Animator>();
        animator.SetFloat("AttackSpeed", playerData.plAtcSpeed);
        Transform parentTransform = GameObject.Find("Enemies").transform;
        attackPos = GameObject.Find("AttackPos").transform;
        FindEnemy();
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
            if (closeEnemy.transform != null)
            {
                Quaternion rot = Quaternion.LookRotation((closeEnemy.transform.position - transform.position).normalized);
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
    {//������ ������ɽ�Ʈ�� Ȱ���ؼ� ���尡��� ����ã�°� ������?
        
        RaycastHit[] hits;
        hits = Physics.SphereCastAll(transform.position, 10f, transform.forward, 20f, enemyLayer);
        if(hits.Length > 0 )
        {
            Debug.Log("��?");
            float closeEnemydis = Mathf.Infinity;
            foreach (RaycastHit hit in hits)
            {
                float dis = Vector3.Distance(transform.position, hit.point);
                if (dis < closeEnemydis)
                {
                    closeEnemydis = dis;
                    this.closeEnemy = hit;
                    enemyTr = hit.transform;
                }
            }
        }
        /*for (int i = 0; i < enemiesList.Count; i++)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemiesList[i].position); // �÷��̾�� �� ������ �Ÿ� ���

            if (distanceToEnemy < closeEnemydis)
            {
                idx = i;
                closeEnemydis = distanceToEnemy; // ���� ������ �Ÿ��� ��������� ���� ª�� �Ÿ����� ª���� ����
                isFind = false;
                break;
            }
        }*/
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
        FindEnemy();
    }
}
