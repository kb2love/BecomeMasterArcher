
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SkillManager : MonoBehaviour
{
    public static SkillManager skillInst; // SkillManager�� �̱��� �ν��Ͻ�
    [SerializeField] private SkillData skillData; // ��ų ������
    [SerializeField] private SoundData soundData; // ���� ������
    [SerializeField] private Transform[] enemies; // ���� Transform �迭
    [SerializeField] private List<GameObject> enemiesList; // ���� GameObject ����Ʈ
    private GameData gameData; // ���� ������
    private GameObject skillGost; // ��ų ����
    private int enemyCount = 0; // ���� ��
    private TouchPad touchPadCom; // ��ġ�е� ������Ʈ
    private GameObject skillPanel; // ��ų �г�
    private GameObject touchPad; // ��ġ�е�
    private Image skillButtonIm01; // ù ��° ��ų ��ư �̹���
    private Image skillButtonIm02; // �� ��° ��ų ��ư �̹���
    private int firstIndex; // ù ��° ��ų �ε���
    private int secondIndex; // �� ��° ��ų �ε���
    private string playerName = "Player"; // �÷��̾� �̸� �±�
    [SerializeField] private Sprite[] skillSprites; // ��ų �̹��� �迭

    private void Awake()
    {
        skillInst = this; // �̱��� �ν��Ͻ� ����
    }

    void Start()
    {
        skillPanel = GameObject.Find("Canvas").transform.GetChild(1).gameObject; // ��ų �г� ��ü ��������
        skillButtonIm01 = skillPanel.transform.GetChild(0).GetComponent<Image>(); // ù ��° ��ų ��ư �̹��� ������Ʈ ��������
        skillButtonIm02 = skillPanel.transform.GetChild(1).GetComponent<Image>(); // �� ��° ��ų ��ư �̹��� ������Ʈ ��������
        touchPad = GameObject.Find("Canvas").transform.GetChild(0).gameObject; // ��ġ�е� ��ü ��������
        skillSprites = new Sprite[] { skillData.attackPower_Im, skillData.attackSpeed_Im, skillData.criticalUp_Im, skillData.hpUp_Im, skillData.doubleAttack_Im }; // ��ų �̹��� �迭 �ʱ�ȭ
        touchPadCom = GameObject.Find("TouchPad_Image").GetComponent<TouchPad>(); // ��ġ�е� ������Ʈ ��������
        skillGost = GameObject.Find("SkillGost").gameObject; // ��ų ���� ��ü ��������
        gameData = DataManager.dataInst.gameData; // ���� ������ ��������

        if (GameObject.Find("Enemies") != null) // �� ��ü�� �����ϸ�
        {
            enemies = GameObject.Find("Enemies").GetComponentsInChildren<Transform>(); // ���� Transform �迭 ��������
            if (enemies.Length > 1) // ���� �ϳ� �̻� ������
            {
                for (int i = 1; i < enemies.Length; i++)
                {
                    enemiesList.Add(enemies[i].gameObject); // �� ����Ʈ�� �߰�
                }
            }
            enemyCount = enemiesList.Count; // ���� �� �ʱ�ȭ
            if (enemyCount == 0) // ���� ������
            {
                skillGost.transform.DOJump(new Vector3(0, 0, 2), 0, 0, 0.5f); // ��ų ���� ���� �ִϸ��̼� ����
            }
        }
        else // �� ��ü�� ������
        {
            skillGost.transform.DOJump(new Vector3(0, 0, 2), 0, 0, 0.5f); // ��ų ���� ���� �ִϸ��̼� ����
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(playerName)) // �÷��̾�� �浹�ϸ�
        {
            SkillStoreOpen(); // ��ų ���� ����
        }
    }

    public void SkillStoreOpen()
    {
        touchPad.SetActive(false); // ��ġ�е� ��Ȱ��ȭ
        skillPanel.SetActive(true); // ��ų �г� Ȱ��ȭ
        touchPadCom.enabled = false; // ��ġ�е� ������Ʈ ��Ȱ��ȭ
        touchPadCom.buttonPress = false; // ��ġ�е� ��ư ��Ȱ��ȭ
        Time.timeScale = 0; // �ð� ����
        SoundManager.soundInst.GetComponent<AudioSource>().PlayOneShot(soundData.storeOpenClip); // ���� ���� ���� ���

        if (!gameData.isDoubleAtc) // ���� ������ �ƴ� ��
        {
            firstIndex = Random.Range(0, skillSprites.Length); // ù ��° ��ų �ε��� ���� ����
            secondIndex = Random.Range(0, skillSprites.Length); // �� ��° ��ų �ε��� ���� ����
            while (firstIndex == secondIndex) // �� �ε����� ������ �ٽ� ���� ����
                secondIndex = Random.Range(0, skillSprites.Length);
        }
        else // ���� ������ ��
        {
            firstIndex = Random.Range(0, skillSprites.Length);
            while (firstIndex == 4) // ���� ���� ��ų ����
                firstIndex = Random.Range(0, skillSprites.Length);
            secondIndex = Random.Range(0, skillSprites.Length);
            while (firstIndex == secondIndex || secondIndex == 4) // ���� ���� ��ų ����
                secondIndex = Random.Range(0, skillSprites.Length);
        }

        // �� ���� ��ų ��ư �̹��� ����
        skillButtonIm01.sprite = skillSprites[firstIndex];
        skillButtonIm02.sprite = skillSprites[secondIndex];
    }

    public void SkillButton01()
    {
        ApplySkill(firstIndex); // ù ��° ��ų ����
    }

    public void SkillButton02()
    {
        ApplySkill(secondIndex); // �� ��° ��ų ����
    }

    private void ApplySkill(int index)
    {
        // �ε����� ���� ��ų ����
        if (skillSprites[index] == skillSprites[0])
        {
            AttackPower();
        }
        else if (skillSprites[index] == skillSprites[1])
        {
            AttackSpeed();
        }
        else if (skillSprites[index] == skillSprites[2])
        {
            CriticalUp();
        }
        else if (skillSprites[index] == skillSprites[3])
        {
            HpUp();
        }
        else if (skillSprites[index] == skillSprites[4])
        {
            DoubleAttack();
        }
    }

    private void AttackPower()
    {
        gameData.plDamage *= 1.2f; // ���ݷ� ����
        Debug.Log(gameData.plDamage);
        SkillSelect(); // ��ų ���� �� ����
    }

    private void AttackSpeed()
    {
        gameData.plAtcSpeed *= 1.2f; // ���� �ӵ� ����
        Debug.Log(gameData.plAtcSpeed);
        SkillSelect(); // ��ų ���� �� ����
    }

    private void CriticalUp()
    {
        if (gameData.plCritical >= 0.5)
        {
            gameData.plCritical += 0.025f; // ũ��Ƽ�� Ȯ�� ����
        }
        else
        {
            gameData.plCritical *= 2f; // ũ��Ƽ�� Ȯ�� 2�� ����
        }
        Debug.Log(gameData.plCritical);
        SkillSelect(); // ��ų ���� �� ����
    }

    private void HpUp()
    {
        gameData.plHP *= 1.2f; // ü�� ����
        gameData.plMaxHP *= 1.2f; // �ִ� ü�� ����
        Debug.Log(gameData.plHP);
        SkillSelect(); // ��ų ���� �� ����
    }

    private void DoubleAttack()
    {
        gameData.isDoubleAtc = true; // ���� ���� Ȱ��ȭ
        Debug.Log(gameData.isDoubleAtc);
        SkillSelect(); // ��ų ���� �� ����
    }

    private void SkillSelect()
    {
        skillPanel.SetActive(false); // ��ų �г� ��Ȱ��ȭ
        Time.timeScale = 1.0f; // �ð� �簳
        touchPad.SetActive(true); // ��ġ�е� Ȱ��ȭ
        touchPadCom.enabled = true; // ��ġ�е� ������Ʈ Ȱ��ȭ
        GameObject skillGost = GameObject.Find("SkillStore").transform.GetChild(0).gameObject; 
        skillGost.SetActive(false); // ��ų ���� ��Ȱ��ȭ
        GameObject Potal = GameObject.Find("Door").transform.GetChild(0).gameObject;
        Potal.SetActive(true); // ��Ż Ȱ��ȭ
        SoundManager.soundInst.GetComponent<AudioSource>().PlayOneShot(soundData.skillSelect); // ��ų ���� ���� ���
        DataManager.dataInst.SaveData(); // ������ ����
    }

    public void EnemyDieCount()
    {
        enemyCount--; // ���� �� ����
        if (enemyCount <= 0) // ���� ��� ������
        {
            skillGost.transform.DOJump(new Vector3(0, 0, 2), 0, 0, 0.5f); // ��ų ���� ���� �ִϸ��̼� ����
        }
    }
}