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
    private GameData gameData;
    private Transform attackPos;
    private LayerMask enemyLayer = 1 << 6;
    [SerializeField] Transform closeEnemy;
    void Start()
    {
        touchPad = GameObject.Find("TouchPad_Image").GetComponent<TouchPad>();
        animator = transform.GetChild(0).GetComponent<Animator>();
        gameData = DataManager.dataInst.gameData;
        animator.SetFloat("AttackSpeed", gameData.plAtcSpeed);
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
        /*RaycastHit[] hits;
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
        }*/
        RaycastHit[] hits;
        hits = Physics.SphereCastAll(transform.position, 10f, transform.forward, 20f, enemyLayer);

        if (hits.Length > 0)
        {
            System.Array.Sort(hits, (x, y) => Vector3.Distance(transform.position, x.point).CompareTo(Vector3.Distance(transform.position, y.point)));

            closeEnemy = hits[0].transform;
        }
        else
        {
            closeEnemy = null;
        }
    }
    public void Attack()
    {
        if (ObjectPoolingManager.objInstance.GetArrow() != null)
        {
            if(!gameData.isDoubleAtc)
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
        animator.SetFloat("AttackSpeed", gameData.plAtcSpeed);
    }
}
