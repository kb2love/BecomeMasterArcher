using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class EnemyDamage : MonoBehaviour
{
    [SerializeField] private EnemyData _enemyData;
    private EnemyMove enemyMove;
    private float hp;
    private Animator animator;
    void Start()
    {
        hp = _enemyData.Hp;
        enemyMove = GetComponent<EnemyMove>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        
    }
    public void RecieveDamage(float damage)
    {
        hp -= damage;
        Debug.Log(hp);
        animator.SetTrigger("HitTrigger");
        Sequence sequence = DOTween.Sequence();
        sequence.AppendCallback(()  => enemyMove.IsHitBool(true)).SetDelay(0).SetUpdate(true);
        sequence.AppendInterval(0.5f);
        sequence.AppendCallback(() => enemyMove.IsHitBool(false)).SetDelay(0).SetUpdate(true);
        enemyMove.IsHitBool(true);
        if (hp <= 0)
        {
            gameObject.SetActive(false);
            PlayerAttack playerAttack = GameObject.Find("Player").GetComponent<PlayerAttack>();
            playerAttack.EnemyDie(transform);
        }
    }
}
