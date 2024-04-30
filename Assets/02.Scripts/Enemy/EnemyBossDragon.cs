using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemyBossDragon : MonoBehaviour
{
    [SerializeField] EnemyData enemyData;
    [SerializeField] SoundData soundData;
    private EnemyBossDamage enemyDamage;
    private Animator animator;
    private GameObject shield;
    private Transform plTr;
    private AudioSource source;
    private int aniIdx = Animator.StringToHash("RandomAttackIdx");
    private int aniFlyIdx = Animator.StringToHash("RandomFlyAttackIdx");
    private int randomAttackIdx = 0;
    private int atIdx = 0;
    private int randomFlyAttackIdx = 0;
    private int atFIdx = 0;
    private bool isFly = false;
    private bool isNotFly = false;
    private Vector3 flyVec;
    private Tween currentTween;
    void Start()
    {
        animator = GetComponent<Animator>();
        enemyDamage = GetComponent<EnemyBossDamage>();
        source = GetComponent<AudioSource>();
        plTr = GameObject.Find("Player").transform;
        randomFlyAttackIdx = Random.Range(0, 2);
        shield = Instantiate(enemyData.bossShield, transform.position, Quaternion.identity);
        shield.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.GetComponent<PlayerMovement>().PlayerRecieveDamage(enemyData.bossDamage);
        }
    }
    public void RandomAttack()
    {
        if (isFly)
        {
            atIdx = randomAttackIdx;
            while (randomAttackIdx == atIdx)
                randomAttackIdx = Random.Range(2, 5);
        }
        else
        {
            if(isNotFly)
            {
                atIdx = randomAttackIdx;
                while (randomAttackIdx == atIdx)
                    randomAttackIdx = Random.Range(2, 5);
            }
            else
            {
                atIdx = randomAttackIdx;
                while (randomAttackIdx == atIdx)
                    randomAttackIdx = Random.Range(1, 5);
            }
            if (randomAttackIdx == 0)
            {
                animator.SetBool("IsFly", true);
                Debug.Log("IsFly 트루");
                isFly = true;
                isNotFly = false;
            }
            else
            {
                isNotFly = true;
            }
        }
        
        Debug.Log(randomAttackIdx);
        animator.SetInteger(aniIdx, randomAttackIdx);
    }
    //*******************************************애니메이션에 들어가는 메서드******************************************//
    public void DragonRun()
    {
        Vector3 dir = PlayerLookRotation();
        // 새로운 Tween 실행
        currentTween = transform.DOMove(dir * 2.0f, 2f);
    }
    public void DragunFlyFoward()
    {
        Vector3 dir = PlayerLookRotation();
        currentTween = transform.DOMove(dir * 2.0f, 2f);
    }

    public void DragonFly()
    {
        Vector3 dir = PlayerLookRotation();
        currentTween = transform.DOMove(transform.up * 1.0f, 2f);
    }
    public void DragonLand()
    {
        Vector3 dir = PlayerLookRotation();
        currentTween = transform.DOMove(transform.up * -1.0f, 2f);
    }
    public void DefendOn()
    {
        Vector3 dir = PlayerLookRotation();
        enemyDamage.isDefand = true;
        shield.transform.position = transform.position;
        shield.SetActive(true);
    }
    public void DefendOff()
    {
        enemyDamage.isDefand = false;
        shield.SetActive(false);

        Vector3 dir = PlayerLookRotation();
    }
    public void NormalAttack()
    {
        Vector3 dir = PlayerLookRotation();
        AttackOverlap();
    }
    public void EnemyFireAttack()
    {

        Vector3 dir = PlayerLookRotation();
        if (isFly)
            flyVec = transform.position - transform.up * 1.0f;
        else
            flyVec = transform.position;
        source.PlayOneShot(soundData.boomClip);
        FireEff();
        AttackOverlap();
    }
    public void Scream()
    {
        source.PlayOneShot(soundData.screamClip);
    }
    //*********************************애니메이션에 들어가는 메서드의 메서드********************************//
    private void AttackOverlap()
    {
        Collider[] overSh = Physics.OverlapSphere(flyVec * 1.0f, 2f, 1 << 3);
        foreach (Collider col in overSh)
        {
            Debug.Log(col.gameObject.name);
            col.GetComponent<PlayerMovement>().PlayerRecieveDamage(enemyData.bossDamage);
        }
    }

    public void RandomFlyAttack()
    {
        atFIdx = randomFlyAttackIdx;
        while(randomFlyAttackIdx == atFIdx)
         randomFlyAttackIdx = Random.Range(0, 3);
        Debug.Log(randomFlyAttackIdx);
        animator.SetInteger(aniFlyIdx, randomFlyAttackIdx);
        if(randomFlyAttackIdx == 2)
        {
            animator.SetBool("IsFly", false);
            Debug.Log("IsFly 폴스");
        }
    }
    private void FireEff()
    {
        GameObject fireEff = Instantiate(enemyData.bossFireEff);
        fireEff.transform.position = flyVec + transform.forward * 1.0f;
        fireEff.transform.rotation = transform.rotation;
        fireEff.SetActive(true);
        DOTween.Sequence()
        .AppendCallback(() => fireEff.SetActive(true))
        .AppendInterval(1f)
        .AppendCallback(() => fireEff.SetActive(false))
        .SetUpdate(true);
    }
    private Vector3 PlayerLookRotation()
    {

        if (currentTween != null && currentTween.IsActive())
        {
            currentTween.Kill();
        }
        Vector3 dir = (plTr.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        lookRotation.z = lookRotation.x = 0;
        // 적의 회전을 시선 벡터 방향으로 Tween을 사용하여 변경하기
        currentTween = transform.DORotateQuaternion(lookRotation, 0.5f);
        return dir;
    }
}
