using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyMove : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform plTr;
    private Animator animator;
    [SerializeField] private float dist = 0.5f;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        plTr = GameObject.Find("Player").transform;
    }

    void Update()
    {
        agent.destination = plTr.position; 
        float dis = Vector3.Distance(transform.position, plTr.position);
        if(dis < dist)
        {
            agent.isStopped = true;
            animator.SetTrigger("AttackTrigger");
            animator.SetBool("IsWalk", false);
        }
        else if(dis > dist)
        {
            agent.isStopped = false;
            animator.SetBool("IsWalk", true);
        }
    }
}
