using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemySwarm : MonoBehaviour
{
    [SerializeField] private EnemyData enemyData;
    private NavMeshAgent agent;
    private EnemyDamage enemyDamage;
    private Transform plTr;
    private Animator animator;
    private float dist = 0.5f;
    [SerializeField] private float attackRadius = 0.8f;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        plTr = GameObject.Find("Player").transform;
        enemyDamage = GetComponent<EnemyDamage>();
    }

    void Update()
    {
        agent.destination = plTr.position; 
        float dis = Vector3.Distance(transform.position, plTr.position);
        if(enemyDamage.isHit || enemyDamage.isDie)
        {
            agent.isStopped = true;
            animator.SetBool("IsWalk", false);
        }
        else if(dis < dist && !enemyDamage.isHit)
        {
            agent.isStopped = true;
            animator.SetTrigger("AttackTrigger");
            animator.SetBool("IsWalk", false);
        }
        else if(dis > dist && !enemyDamage.isHit)
        {
            agent.isStopped = false;
            animator.SetBool("IsWalk", true);
        }
    }
    public void EnemyAttack()
    {
        Collider[] overSh = Physics.OverlapSphere(transform.position, attackRadius, 1 << 3);
        
        foreach(Collider col in overSh)
        {
            col.GetComponent<PlayerMovement>().PlayerRecieveDamage(enemyData.Damage);
        }
    }
}
