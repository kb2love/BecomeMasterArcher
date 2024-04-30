using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class EnemyBossDamage : MonoBehaviour
{
    [SerializeField] private EnemyData _enemyData;
    [SerializeField] private SoundData soundData;
    [SerializeField] private PlayerData playerData;
    private Image hpImage;
    public float hp;
    private Animator animator;
    private AudioSource source;
    public bool isDie;
    public bool isHit = false;
    public bool isDefand = true;
    void Start()
    {
        hp = _enemyData.bossHP;
        hpImage = GameObject.Find("EnemyBossHp_Image").transform.GetChild(0).GetComponent<Image>();
        isDie = false;
        source = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }
    public void RecieveDamage(float damage)
    {
        if (!isDefand)
        {
            hp -= damage;
            HitEff();
            source.PlayOneShot(soundData.e_hitClip);
            hpImage.fillAmount = hp / _enemyData.bossHP;
            DOTween.Sequence()
                .AppendCallback(() => isHit = true)
                .AppendInterval(0.5f)
                .AppendCallback(() => isHit = false)
                .SetUpdate(true);
            if (hp <= 0)
            {
                gameObject.GetComponent<Collider>().enabled = false;
                animator.SetTrigger("DieTrigger");
                hpImage.gameObject.SetActive(false);
                PlayerMovement player = GameObject.Find("Player").GetComponent<PlayerMovement>();
                player.PlayerExpUp(5);
                player.Heal(60);
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
