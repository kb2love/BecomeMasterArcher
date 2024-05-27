using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemyBossDragon : MonoBehaviour
{
    [SerializeField] EnemyData enemyData; // �� ������
    [SerializeField] SoundData soundData; // ���� ������
    private EnemyBossDamage enemyDamage; // ���� ������ ��ũ��Ʈ
    private Animator animator; // �ִϸ����� ������Ʈ
    private GameObject shield; // ���� ������Ʈ
    private Transform plTr; // �÷��̾� Ʈ������
    private AudioSource source; // ����� �ҽ�
    private int aniIdx = Animator.StringToHash("RandomAttackIdx"); // ���� ���� �ִϸ��̼� �ε��� �ؽ�
    private int aniFlyIdx = Animator.StringToHash("RandomFlyAttackIdx"); // ���� ���� ���� �ִϸ��̼� �ε��� �ؽ�
    private int randomAttackIdx = 0; // ���� ���� �ε���
    private int atIdx = 0; // ���� �ε��� �񱳿� �ӽ� ����
    private int randomFlyAttackIdx = 0; // ���� ���� ���� �ε���
    private int atFIdx = 0; // ���� ���� �ε��� �񱳿� �ӽ� ����
    private bool isFly = false; // ���� ���� �÷���
    private bool isNotFly = false; // �������� �ʴ� ���� �÷���
    private Vector3 flyVec; // ���� ����
    private Tween currentTween; // ���� Ʈ��

    void Start()
    {
        animator = GetComponent<Animator>(); // �ִϸ����� ������Ʈ ��������
        enemyDamage = GetComponent<EnemyBossDamage>(); // EnemyBossDamage ������Ʈ ��������
        source = GetComponent<AudioSource>(); // ����� �ҽ� ������Ʈ ��������
        plTr = GameObject.Find("Player").transform; // �÷��̾� Ʈ������ ã��
        randomFlyAttackIdx = Random.Range(0, 2); // ���� ���� ���� �ε��� �ʱ�ȭ
        shield = Instantiate(enemyData.bossShield, transform.position, Quaternion.identity); // ���� ����
        shield.SetActive(false); // ���� ��Ȱ��ȭ
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player") // �浹�� ��ü�� �÷��̾����� Ȯ��
        {
            other.GetComponent<PlayerMovement>().PlayerRecieveDamage(enemyData.bossDamage); // �÷��̾�� ������ �ֱ�
        }
    }

    public void RandomAttack()
    {
        if (isFly) // �巡���� ���� ������ Ȯ��
        {
            atIdx = randomAttackIdx; // ���� ���� ���� �ε��� ����
            while (randomAttackIdx == atIdx) // ���ο� �ε����� �ٸ� ������ �ݺ�
                randomAttackIdx = Random.Range(2, 5); // ���ο� ���� ���� �ε��� ����
        }
        else
        {
            if (isNotFly) // �巡���� ���� ���� �ƴ��� Ȯ��
            {
                atIdx = randomAttackIdx; // ���� ���� ���� �ε��� ����
                while (randomAttackIdx == atIdx) // ���ο� �ε����� �ٸ� ������ �ݺ�
                    randomAttackIdx = Random.Range(2, 5); // ���ο� ���� ���� �ε��� ����
            }
            else
            {
                atIdx = randomAttackIdx; // ���� ���� ���� �ε��� ����
                while (randomAttackIdx == atIdx) // ���ο� �ε����� �ٸ� ������ �ݺ�
                    randomAttackIdx = Random.Range(1, 5); // ���ο� ���� ���� �ε��� ����
            }
            if (randomAttackIdx == 0) // �� ���� �ε����� 0���� Ȯ�� (���� ����)
            {
                animator.SetBool("IsFly", true); // �ִϸ����Ϳ� IsFly�� true�� ����
                Debug.Log("IsFly Ʈ��"); // ����� �α� ���
                isFly = true; // ���� ���� �÷��� ����
                isNotFly = false; // ����� ���� �÷��� ����
            }
            else
            {
                isNotFly = true; // ����� ���� �÷��� ����
            }
        }

        Debug.Log(randomAttackIdx); // ���� ���� �ε��� ����� �α� ���
        animator.SetInteger(aniIdx, randomAttackIdx); // �ִϸ����Ϳ� ���� ���� �ε��� ����
    }

    //******************************************* �ִϸ��̼ǿ��� ȣ��Ǵ� �޼��� ******************************************//
    public void DragonRun()
    {
        Vector3 dir = PlayerLookRotation(); // �÷��̾� ���� ���
        currentTween = transform.DOMove(dir * 2.0f, 2f); // DOTween�� ����� �÷��̾� �������� �̵�
    }

    public void DragonFlyForward()
    {
        Vector3 dir = PlayerLookRotation(); // �÷��̾� ���� ���
        currentTween = transform.DOMove(dir * 2.0f, 2f); // DOTween�� ����� ������ �̵�
    }

    public void DragonFly()
    {
        currentTween = transform.DOMove(transform.up * 1.0f, 2f); // DOTween�� ����� ���� �̵�
    }

    public void DragonLand()
    {
        currentTween = transform.DOMove(transform.up * -1.0f, 2f); // DOTween�� ����� �Ʒ��� �̵�
    }

    public void DefendOn()
    {
        enemyDamage.isDefand = true; // ��� ���� ����
        shield.transform.position = transform.position; // ���� ��ġ ����
        shield.SetActive(true); // ���� Ȱ��ȭ
    }

    public void DefendOff()
    {
        enemyDamage.isDefand = false; // ��� ���� ����
        shield.SetActive(false); // ���� ��Ȱ��ȭ
    }

    public void NormalAttack()
    {
        AttackOverlap(); // �Ϲ� ���� ó��
    }

    public void EnemyFireAttack()
    {
        if (isFly)
            flyVec = transform.position - transform.up * 1.0f; // ���� ���̸� ��ġ ����
        else
            flyVec = transform.position; // ���� ���� �ƴϸ� ���� ��ġ ���

        source.PlayOneShot(soundData.boomClip); // �� ���� �Ҹ� ���
        FireEff(); // �� ���� ȿ�� ����
        AttackOverlap(); // ���� ó��
    }

    public void Scream()
    {
        source.PlayOneShot(soundData.screamClip); // ��� �Ҹ� ���
    }

    //********************************* �ִϸ��̼ǿ��� ȣ��Ǵ� �޼��� ���� �޼��� ********************************//
    private void AttackOverlap()
    {
        Collider[] overSh = Physics.OverlapSphere(flyVec * 1.0f, 2f, 1 << 3); // ������ �ݰ� ������ �浹 üũ
        foreach (Collider col in overSh)
        {
            Debug.Log(col.gameObject.name); // �浹�� ��ü �̸� ����� �α� ���
            col.GetComponent<PlayerMovement>().PlayerRecieveDamage(enemyData.bossDamage); // �÷��̾�� ������ �ֱ�
        }
    }

    public void RandomFlyAttack()
    {
        atFIdx = randomFlyAttackIdx; // ���� ���� ���� ���� �ε��� ����
        while (randomFlyAttackIdx == atFIdx) // ���ο� �ε����� �ٸ� ������ �ݺ�
            randomFlyAttackIdx = Random.Range(0, 3); // ���ο� ���� ���� ���� �ε��� ����

        Debug.Log(randomFlyAttackIdx); // ���� ���� ���� �ε��� ����� �α� ���
        animator.SetInteger(aniFlyIdx, randomFlyAttackIdx); // �ִϸ����Ϳ� ���� ���� ���� �ε��� ����

        if (randomFlyAttackIdx == 2) // ���� ���� �ε����� 2�̸� ����
        {
            animator.SetBool("IsFly", false); // �ִϸ����Ϳ� IsFly�� false�� ����
            Debug.Log("IsFly ����"); // ����� �α� ���
        }
    }

    private void FireEff()
    {
        GameObject fireEff = Instantiate(enemyData.bossFireEff); // �� ȿ�� ����
        fireEff.transform.position = flyVec + transform.forward * 1.0f; // �� ȿ�� ��ġ ����
        fireEff.transform.rotation = transform.rotation; // �� ȿ�� ȸ�� ����
        fireEff.SetActive(true); // �� ȿ�� Ȱ��ȭ
        DOTween.Sequence() // DOTween ������ ����
            .AppendCallback(() => fireEff.SetActive(true)) // �� ȿ�� Ȱ��ȭ
            .AppendInterval(1f) // 1�� ���
            .AppendCallback(() => fireEff.SetActive(false)) // �� ȿ�� ��Ȱ��ȭ
            .SetUpdate(true); // �ǽð� ������Ʈ ����
    }

    private Vector3 PlayerLookRotation()
    {
        if (currentTween != null && currentTween.IsActive()) // ���� Ʈ���� Ȱ��ȭ �Ǿ��ִ��� Ȯ��
        {
            currentTween.Kill(); // ���� Ʈ�� ����
        }

        Vector3 dir = (plTr.position - transform.position).normalized; // �÷��̾� ���� ���
        Quaternion lookRotation = Quaternion.LookRotation(dir); // �÷��̾ ���� ȸ�� ����
        lookRotation.z = lookRotation.x = 0; // z��� x�� ȸ�� ����
        currentTween = transform.DORotateQuaternion(lookRotation, 0.5f); // DOTween�� ����� �÷��̾ ���� ȸ��
        return dir; // ���� ��ȯ
    }
}