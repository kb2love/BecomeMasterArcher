using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private PlayerData playerData; // �÷��̾� ������ ��ũ���ͺ� ������Ʈ
    [SerializeField] private SoundData soundData; // ���� ������ ��ũ���ͺ� ������Ʈ
    private Animator animator; // �ִϸ����� ������Ʈ
    private TouchPad touchPad; // ��ġ�е� ������Ʈ
    private AudioSource audioSource; // ����� �ҽ� ������Ʈ
    private GameData gameData; // ���� ������
    private Transform attackPos; // ���� ��ġ
    private LayerMask enemyLayer = 1 << 6; // �� ���̾�
    [SerializeField] private Transform closeEnemy; // ���� ����� ��

    void Start()
    {
        touchPad = GameObject.Find("TouchPad_Image").GetComponent<TouchPad>(); // ��ġ�е� ������Ʈ ��������
        animator = transform.GetChild(0).GetComponent<Animator>(); // �ڽ� ������Ʈ�� �ִϸ����� ������Ʈ ��������
        gameData = DataManager.dataInst.gameData; // ���� ������ ��������
        animator.SetFloat("AttackSpeed", gameData.plAtcSpeed); // �ִϸ������� ���� �ӵ� ����
        attackPos = GameObject.Find("AttackPos").transform; // ���� ��ġ Ʈ������ ��������
        audioSource = GetComponent<AudioSource>(); // ����� �ҽ� ������Ʈ ��������
        FindEnemy(); // ���� ����� �� ã��
    }

    void Update()
    {
        if (touchPad.buttonPress)
        {
            // ������ ���� �������� ����
            animator.SetBool("IsWalk", true); // �ȱ� �ִϸ��̼� ���
        }
        else
        {
            if (closeEnemy != null)
            {
                // ����� ���� ���� ��� ���� �ٶ󺸰� ����
                Quaternion rot = Quaternion.LookRotation((closeEnemy.position - transform.position).normalized);
                rot.x = rot.z = 0f; // ȸ�� �� ����
                transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * 10.0f); // �ε巴�� ȸ��
                animator.SetBool("IsWalk", false); // �ȱ� �ִϸ��̼� ����
                animator.SetTrigger("AttackTrigger"); // ���� �ִϸ��̼� ���
            }
            else
            {
                animator.SetBool("IsWalk", true); // �ȱ� �ִϸ��̼� ���
            }
        }
    }

    public void FindEnemy()
    {
        // ���� ����� ���� ã�� �Լ�
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, 10f, transform.forward, 20f, enemyLayer);

        if (hits.Length > 0)
        {
            // ���� �ִ� ��� �Ÿ��� ���� �����Ͽ� ���� ����� ���� ����
            System.Array.Sort(hits, (x, y) => Vector3.Distance(transform.position, x.point).CompareTo(Vector3.Distance(transform.position, y.point)));
            closeEnemy = hits[0].transform;
        }
        else
        {
            closeEnemy = null; // ���� ���� ��� null�� ����
        }
    }

    public void Attack()
    {
        if (ObjectPoolingManager.objInstance.GetArrow() != null)
        {
            if (!gameData.isDoubleAtc)
            {
                ArrowShot(); // ���� ����
            }
            else
            {
                ArrowShot(); // ù ��° ����
                Sequence sequence = DOTween.Sequence();
                sequence.AppendInterval(0.15f) // 0.15�� �� �� ��° ����
                        .AppendCallback(() => ArrowShot());
            }
        }
    }

    void ArrowShot()
    {
        GameObject arrow = ObjectPoolingManager.objInstance.GetArrow(); // ȭ�� ������Ʈ ��������
        arrow.transform.position = attackPos.position; // ȭ�� ��ġ ����
        arrow.transform.rotation = attackPos.rotation; // ȭ�� ȸ�� ����
        arrow.SetActive(true); // ȭ�� Ȱ��ȭ
        audioSource.PlayOneShot(soundData.arrowClip); // ȭ�� �߻� �Ҹ� ���
    }

    public void AttackSpeedUp()
    {
        // ���� �ӵ� ������Ʈ
        animator.SetFloat("AttackSpeed", gameData.plAtcSpeed);
    }
}