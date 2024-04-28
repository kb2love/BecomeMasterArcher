using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class EnemyDamage : MonoBehaviour
{
    [SerializeField] private EnemyData _enemyData;
    [SerializeField] private SoundData soundData;
    [SerializeField] private PlayerData playerData;
    private EnemyState enemyMove;
    public float hp;
    private Animator animator;
    private AudioSource source;
    void Start()
    {
        hp = _enemyData.Hp;
        source = GetComponent<AudioSource>();
        enemyMove = GetComponent<EnemyState>();
        animator = GetComponent<Animator>();
    }
    public void RecieveDamage(float damage)
    {
        hp -= damage;
        Debug.Log(hp);
        animator.SetTrigger("HitTrigger");
        HitEff();
        SoundManager.soundInst.PlaySound(source, soundData.e_hitClip);
        DOTween.Sequence()
            .AppendCallback(() => enemyMove.IsHitBool(true))
            .AppendInterval(0.5f)
            .AppendCallback(() => enemyMove.IsHitBool(false))
            .SetUpdate(true);
        if (hp <= 0)
        {
            gameObject.GetComponent<Collider>().enabled = false;
            enemyMove.E_Die();
            animator.SetTrigger("DieTrigger");
            animator.SetBool("IsDie", true);
            GameObject.Find("Enemies").GetComponent<EnemyCount>().EnemyCountDown(this.gameObject);
            DOTween.Sequence()
            .AppendInterval(1.2f)
            .AppendCallback(() => gameObject.SetActive(false))
            .SetUpdate(true);
            PlayerAttack playerAttack = GameObject.Find("Player").GetComponent<PlayerAttack>();
            playerAttack.FindEnemy();
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
