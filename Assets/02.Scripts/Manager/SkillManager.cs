
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    public static SkillManager skillInst;
    [SerializeField] SkillData skillData;
    [SerializeField] SoundData soundData;
    [SerializeField] private Transform[] enemies;
    [SerializeField] private List<GameObject> enemiesList;
    private GameData gameData;
    private GameObject skillGost;
    private int enemyCount = 0;
    private TouchPad touchPadCom;
    private GameObject skillPanel;
    private GameObject touchPad;
    private Image skillButtonIm01;
    private Image skillButtonIm02;
    private int firstIndex;
    private int secondIndex;
    private string playerName = "Player";
    [SerializeField] private Sprite[] skillSprites;
    private void Awake()
    {
        skillInst = this;
    }
    void Start()
    {
        skillPanel = GameObject.Find("Canvas").transform.GetChild(1).gameObject;
        skillButtonIm01 = skillPanel.transform.GetChild(0).GetComponent<Image>();
        skillButtonIm02 = skillPanel.transform.GetChild(1).GetComponent<Image>();
        touchPad = GameObject.Find("Canvas").transform.GetChild(0).gameObject;
        skillSprites = new Sprite[] { skillData.attackPower_Im, skillData.attackSpeed_Im, skillData.criticalUp_Im, skillData.hpUp_Im, skillData.doubleAttack_Im };
        touchPadCom = GameObject.Find("TouchPad_Image").GetComponent<TouchPad>();
        skillGost = GameObject.Find("SkillGost").gameObject;
        gameData = DataManager.dataInst.gameData;
        if (GameObject.Find("Enemies") != null)
        {
            enemies = GameObject.Find("Enemies").GetComponentsInChildren<Transform>();
            if (enemies.Length > 1)
            {
                for (int i = 1; i < enemies.Length; i++)
                {
                    enemiesList.Add(enemies[i].gameObject);
                }
            }
            enemyCount = enemiesList.Count;
            if (enemyCount == 0)
            {
                skillGost.transform.DOJump(new Vector3(0, 0, 2), 0, 0, 0.5f);


            }
        }
        else
        {
            skillGost.transform.DOJump(new Vector3(0, 0, 2), 0, 0, 0.5f);
        }
    }

    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(playerName))
        {
            SkillStoreOpen();
        }
    }

    public void SkillStoreOpen()
    {
        touchPad.SetActive(false);
        skillPanel.SetActive(true);
        touchPadCom.enabled = false;
        touchPadCom.buttonPress = false;
        Time.timeScale = 0;
        SoundManager.soundInst.GetComponent<AudioSource>().PlayOneShot(soundData.storeOpenClip);
        if (!gameData.isDoubleAtc)
        {
            firstIndex = Random.Range(0, skillSprites.Length);
            secondIndex = Random.Range(0, skillSprites.Length);
            while (firstIndex == secondIndex)
                secondIndex = Random.Range(0, skillSprites.Length);
        }
        else
        {
            firstIndex = Random.Range(0, skillSprites.Length);
            while(firstIndex == 4)
                firstIndex = Random.Range(0, skillSprites.Length);
            secondIndex = Random.Range(0, skillSprites.Length);
            while (firstIndex == secondIndex || secondIndex == 4)
                secondIndex = Random.Range(0, skillSprites.Length);
        }
        // 두 개의 스프라이트 할당
        skillButtonIm01.sprite = skillSprites[firstIndex];
        skillButtonIm02.sprite = skillSprites[secondIndex];
    }

    public void SkillButton01()
    {
        if (skillSprites[firstIndex] == skillSprites[0])
        {
            AttackPower();
        }
        else if (skillSprites[firstIndex] == skillSprites[1])
        {
            AttackSpeed();
        }
        else if (skillSprites[firstIndex] == skillSprites[2])
        {
            CriticalUp();
        }
        else if (skillSprites[firstIndex] == skillSprites[3])
        {
            HpUp();
        }
        else if (skillSprites[firstIndex] == skillSprites[4])
        {
            DoubleAttack();
        }
    }
    public void SkillButton02()
    {
        if (skillSprites[secondIndex] == skillSprites[0])
        {
            AttackPower();
        }
        else if (skillSprites[secondIndex] == skillSprites[1])
        {
            AttackSpeed();
        }
        else if (skillSprites[secondIndex] == skillSprites[2])
        {
            CriticalUp();
        }
        else if (skillSprites[secondIndex] == skillSprites[3])
        {
            HpUp();
        }
        else if (skillSprites[secondIndex] == skillSprites[4])
        {
            DoubleAttack();
        }
    }
    private void AttackPower()
    {
        gameData.plDamage *= 1.2f;
        Debug.Log(gameData.plDamage);
        SkillSelect();
    }
    private void AttackSpeed()
    {
        gameData.plAtcSpeed *= 1.2f;
        Debug.Log(gameData.plAtcSpeed);
        SkillSelect();
    }
    private void CriticalUp()
    {
        if (gameData.plCritical >= 0.5)
        {
            gameData.plCritical += 0.025f;
        }
        else
        {
            gameData.plCritical *= 2f;
        }
        Debug.Log(gameData.plCritical);
        SkillSelect();
    }
    private void HpUp()
    {
        gameData.plHP *= 1.2f;
        gameData.plMaxHP *= 1.2f;
        Debug.Log(gameData.plHP);
        SkillSelect();
    }
    private void DoubleAttack()
    {
        gameData.isDoubleAtc = true;
        Debug.Log(gameData.isDoubleAtc);
        SkillSelect();
    }
    private void SkillSelect()
    {
        skillPanel.SetActive(false);
        Time.timeScale = 1.0f;
        touchPad.SetActive(true);
        touchPadCom.enabled = true;
        GameObject skillGost = GameObject.Find("SkillGost").gameObject;
        skillGost.SetActive(false);
        GameObject Potal = GameObject.Find("Door").transform.GetChild(0).gameObject;
        Potal.SetActive(true);
        SoundManager.soundInst.GetComponent<AudioSource>().PlayOneShot(soundData.skillSelect);
        DataManager.dataInst.SaveData();
    }
    public void EnemyDieCount()
    {
        enemyCount--;
        if (enemyCount <= 0)
        {
            skillGost.transform.DOJump(new Vector3(0, 0, 2), 0, 0, 0.5f);
        }
    }
}
