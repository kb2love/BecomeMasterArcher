using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class PlayerAttack : MonoBehaviour
{
    [SerializeField] PlayerData playerData;
    [SerializeField] SoundData soundData;
    private Animator animator;
    private TouchPad touchPad;
    private AudioSource source;

    private Transform attackPos;
    private LayerMask enemyLayer = 1 << 6;
    Transform closeEnemy;
    Transform enemyTr;
    void Start()
    {
        touchPad = GameObject.Find("TouchPad_Image").GetComponent<TouchPad>();
        animator = transform.GetChild(0).GetComponent<Animator>();
        animator.SetFloat("AttackSpeed", playerData.plAtcSpeed);
        attackPos = GameObject.Find("AttackPos").transform;
        source = GetComponent<AudioSource>();
        FindEnemy();
    }

    void Update()
    {
        if (touchPad.buttonPress)
        {//움직일떄 공격x
            animator.SetBool("IsWalk", true);

        }
        else
        {//멈춰서 공격o
            if (closeEnemy != null)
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

    public void FindEnemy()
    {//피직스 스페어케스트를 활용해서 가장가까운 적을찾는게 맞을듯?
        Debug.Log("FindEnemy");
        RaycastHit[] hits;
        hits = Physics.SphereCastAll(transform.position, 10f, transform.forward, 20f, enemyLayer);
        if(hits.Length > 0 )
        {
            float closeEnemydis = Mathf.Infinity;
            foreach (RaycastHit hit in hits)
            {
                float dis = Vector3.Distance(transform.position, hit.point);
                if (dis < closeEnemydis)
                {
                    closeEnemydis = dis;
                    closeEnemy = hit.transform;
                    enemyTr = hit.transform;
                }
            }
        }
        else
        {
            closeEnemy = null;
        }
        
        /*for (int i = 0; i < enemiesList.Count; i++)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemiesList[i].position); // 플레이어와 적 사이의 거리 계산

            if (distanceToEnemy < closeEnemydis)
            {
                idx = i;
                closeEnemydis = distanceToEnemy; // 현재 적과의 거리가 현재까지의 가장 짧은 거리보다 짧으면 갱신
                isFind = false;
                break;
            }
        }*/
    }
    public void Attack()
    {
        if (ObjectPoolingManager.objInstance.GetArrow() != null)
        {
            if(!playerData.isDoubleAtc)
            {
                ArrowShot();
            }
            else
            {
                ArrowShot();
                Sequence sequence = DOTween.Sequence();
                sequence.AppendInterval(0.15f)
                        .AppendCallback(() => ArrowShot());
            }
        }
    }

    void ArrowShot()
    {
        GameObject arrow = ObjectPoolingManager.objInstance.GetArrow();
        arrow.transform.position = attackPos.position;
        arrow.transform.rotation = attackPos.rotation;
        arrow.SetActive(true);
        source.PlayOneShot(soundData.arrowClip);
    }

    public void AttackSpeedUp()
    {
        animator.SetFloat("AttackSpeed", playerData.plAtcSpeed);
    }
}
