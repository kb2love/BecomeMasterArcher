using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.ComponentModel;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField] private EnemyData _enemyData;
    [SerializeField] private SoundData soundData;
    [SerializeField] private PlayerData playerData;
    public float hp;
    private Animator animator;
    private AudioSource source;
    public bool isDie;
    public bool isHit = false;
    public bool isDefand = false;
    void Start()
    {
        hp = _enemyData.Hp;
        isDie = false;
        source = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }
    public void RecieveDamage(float damage)
    {
        if(!isDefand)
        {
            hp -= damage;
            animator.SetTrigger("HitTrigger");
            HitEff();
            SoundManager.soundInst.PlaySound(source, soundData.e_hitClip);
            DOTween.Sequence()
                .AppendCallback(() => isHit = true)
                .AppendInterval(0.5f)
                .AppendCallback(() => isHit = false)
                .SetUpdate(true);
            if (hp <= 0)
            {
                gameObject.GetComponent<Collider>().enabled = false;
                animator.SetTrigger("DieTrigger");
                GameObject.Find("Player").GetComponent<PlayerMovement>().PlayerExpUp();
                GameObject.Find("Enemies").GetComponent<EnemyCount>().EnemyCountDown(this.gameObject);
                DOTween.Sequence()
                .AppendInterval(5f)
                .AppendCallback(() => gameObject.SetActive(false))
                .SetUpdate(true);
                PlayerAttack playerAttack = GameObject.Find("Player").GetComponent<PlayerAttack>();
                playerAttack.FindEnemy();
            }
        }
        else
        {
            HitEff();
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
