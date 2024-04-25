
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    [SerializeField] SkillData skillData;
    [SerializeField] PlayerData playerData;
    private TouchPad touchPadIm;
    private GameObject skillPanel;
    private GameObject touchPad;
    private Image skillButtonIm01;
    private Image skillButtonIm02;
    private int firstIndex;
    private int secondIndex;
    private string playerName = "Player";
    [SerializeField] private Sprite[] skillSprites;
    void Start()
    {
        skillPanel = GameObject.Find("Canvas").transform.GetChild(1).gameObject;
        skillButtonIm01 = skillPanel.transform.GetChild(0).GetComponent<Image>();
        skillButtonIm02 = skillPanel.transform.GetChild(1).GetComponent<Image>();
        touchPad = GameObject.Find("Canvas").transform.GetChild(0).gameObject;
        skillSprites = new Sprite[]{ skillData.attackPower_Im, skillData.attackSpeed_Im, skillData.criticalUp_Im, skillData.hpUp_Im, skillData.doubleAttack_Im};
        touchPadIm = GameObject.Find("TouchPad_Image").GetComponent<TouchPad>();
    }

    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag(playerName))
        {
            SkillStoreOpen();
        }
    }

    private void SkillStoreOpen()
    {
        touchPad.SetActive(false);
        skillPanel.SetActive(true);
        touchPadIm.buttonPress = false;
        Time.timeScale = 0;
        firstIndex = Random.Range(0, skillSprites.Length);
        secondIndex = Random.Range(0, skillSprites.Length);
        while (firstIndex == secondIndex)
            secondIndex = Random.Range(0, skillSprites.Length);
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
        playerData.plDamage *= 1.2f;
        Debug.Log(playerData.plDamage);
        SkillSelect();
    }
    private void AttackSpeed()
    {
        SkillSelect();
        playerData.plAtcSpeed *= 1.2f;
        Debug.Log(playerData.plAtcSpeed);
    }
    private void CriticalUp()
    {
        SkillSelect();
        if(playerData.plCritical >= 0.5)
        {
            playerData.plCritical += 0.025f;
        }
        else
        {
            playerData.plCritical *= 2f;
        }
        Debug.Log(playerData.plCritical);
    }
    private void HpUp()
    {
        SkillSelect();
        playerData.plHP *= 1.2f;
        Debug.Log(playerData.plHP);
    }
    private void DoubleAttack()
    {
        playerData.isDoubleAtc = true;
        Debug.Log(playerData.isDoubleAtc);
        SkillSelect();
    }
    private void SkillSelect()
    {
        skillPanel.SetActive(false);
        Time.timeScale = 1.0f;
        touchPad.SetActive(true);
        touchPadIm.buttonPress = true;
        GameObject skillGost = GameObject.Find("SkillGost").gameObject;
        skillGost.SetActive(false);
        GameObject Potal = GameObject.Find("Door").transform.GetChild(0).gameObject;
        Potal.SetActive(true);
    }
}
