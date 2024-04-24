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
        HitEff();

        DOTween.Sequence()
            .AppendCallback(() => enemyMove.IsHitBool(true))
            .AppendInterval(0.5f)
            .AppendCallback(() => enemyMove.IsHitBool(false))
            .SetUpdate(true);
        if (hp <= 0)
        {
            gameObject.SetActive(false);
            PlayerAttack playerAttack = GameObject.Find("Player").GetComponent<PlayerAttack>();
            playerAttack.EnemyDie(transform);
        }
    }

    private void HitEff()
    {
        GameObject hitEff = ObjectPoolingManager.objInstance.GetHitEff();
        hitEff.transform.position = transform.position;
        hitEff.transform.rotation = transform.rotation;
        hitEff.SetActive(true);
        DOTween.Sequence()
        .AppendCallback(() => hitEff.SetActive(true))
        .AppendInterval(0.15f)
        .AppendCallback(() => hitEff.SetActive(false))
        .SetUpdate(true);
    }
}
