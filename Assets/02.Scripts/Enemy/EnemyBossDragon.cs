using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemyBossDragon : MonoBehaviour
{
    [SerializeField] EnemyData enemyData;
    private EnemyDamage enemyDamage;
    private Animator animator;
    private GameObject shield;
    private Transform plTr;
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
        enemyDamage = GetComponent<EnemyDamage>();
        plTr = GameObject.Find("Player").transform;
        randomFlyAttackIdx = Random.Range(0, 2);
        shield = Instantiate(enemyData.bossShield, transform.position, Quaternion.identity);
        shield.SetActive(false);
    }

    void Update()
    {
        Quaternion rot = Quaternion.LookRotation((plTr.position - transform.position).normalized);
        rot.z = rot.x = 0;
        Quaternion.Slerp(transform.rotation, rot, 10f * Time.deltaTime);
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
                Debug.Log("IsFly 飘风");
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
    public void DragonRun()
    {
        if (currentTween != null && currentTween.IsActive())
        {
            currentTween.Kill();
        }

        // 货肺款 Tween 角青
        currentTween = transform.DOMove(transform.forward * 2.0f, 2f);
    }
    public void DragunFlyFoward()
    {
        if (currentTween != null && currentTween.IsActive())
        {
            currentTween.Kill();
        }
        currentTween = transform.DOMove(transform.forward * 2.0f, 2f);
    }
    public void DragonFly()
    {
        if (currentTween != null && currentTween.IsActive())
        {
            currentTween.Kill();
        }
        currentTween = transform.DOMove(transform.up * 1.0f, 2f);
    }
    public void DragonLand()
    {
        if (currentTween != null && currentTween.IsActive())
        {
            currentTween.Kill();
        }

        // 货肺款 Tween 角青
        currentTween = transform.DOMove(transform.up * -1.0f, 2f);
    }
    public void DefendOn()
    {
        enemyDamage.isDefand = true;
        shield.transform.position = transform.position;
        shield.SetActive(true);
    }
    public void DefendOff()
    {
        enemyDamage.isDefand = false;
        shield.SetActive(false);
        
    }
    public void NormalAttack()
    {
        Collider[] overSh = Physics.OverlapSphere(flyVec * 1.0f, 1.6f, 1 << 3);
        foreach (Collider col in overSh)
        {
            col.GetComponent<PlayerMovement>().PlayerRecieveDamage(enemyData.bossDamage);
        }
    }
    public void EnemyFireAttack()
    {

        if (isFly)
            flyVec = transform.position - transform.up * 1.0f;
        else
            flyVec = transform.position;
        Collider[] overSh = Physics.OverlapSphere(flyVec * 1.0f, 1.6f, 1 << 3);
        FireEff();
        foreach (Collider col in overSh)
        {
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
            Debug.Log("IsFly 弃胶");
        }
    }
    private void FireEff()
    {
        GameObject fireEff = ObjectPoolingManager.objInstance.GetFireEff();
        fireEff.transform.position = flyVec + transform.forward * 1.0f;
        fireEff.transform.rotation = transform.rotation;
        fireEff.SetActive(true);
        DOTween.Sequence()
        .AppendCallback(() => fireEff.SetActive(true))
        .AppendInterval(1f)
        .AppendCallback(() => fireEff.SetActive(false))
        .SetUpdate(true);
    }
}
