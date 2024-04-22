
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    [SerializeField] SkillData skillData;
    [SerializeField] PlayerData playerData;
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
        
    }

    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag(playerName))
        {
            touchPad.SetActive(false);
            skillPanel.SetActive(true);
            Time.timeScale = 0;
            GameObject touchPadIm = GameObject.Find("TouchPad_Image").gameObject;
            touchPadIm.GetComponent<TouchPad>().buttonPress = false;
            firstIndex = Random.Range(0, skillSprites.Length);
            secondIndex = Random.Range(0, skillSprites.Length);
            while(firstIndex == secondIndex)
                secondIndex = Random.Range(0, skillSprites.Length);
            // 두 개의 스프라이트 할당
            skillButtonIm01.sprite = skillSprites[firstIndex];
            skillButtonIm02.sprite = skillSprites[secondIndex];
        }
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
        Debug.Log("어택파워");
        SkillSelect();
    }
    private void AttackSpeed()
    {
        Debug.Log("어택스피드");
        SkillSelect();
    }
    private void CriticalUp()
    {
        Debug.Log("크리티컬업");
        SkillSelect();
    }
    private void HpUp()
    {
        Debug.Log("애취피업");
        SkillSelect();
    }
    private void DoubleAttack()
    {
        Debug.Log("더블어택");
        SkillSelect();
    }
    private void SkillSelect()
    {
        Time.timeScale = 1.0f;
        skillPanel.SetActive(false);
        GameObject skillGost = GameObject.Find("SkillGost").gameObject;
        skillGost.SetActive(false);
        GameObject Potal = GameObject.Find("Door").transform.GetChild(0).gameObject;
        Potal.SetActive(true);
    }
}
