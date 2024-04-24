
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
            touchPad.SetActive(false);
            skillPanel.SetActive(true);
            touchPadIm.buttonPress = false;
            Time.timeScale = 0;
            firstIndex = Random.Range(0, skillSprites.Length);
            secondIndex = Random.Range(0, skillSprites.Length);
            while(firstIndex == secondIndex)
                secondIndex = Random.Range(0, skillSprites.Length);
            // �� ���� ��������Ʈ �Ҵ�
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
        Debug.Log("�����Ŀ�");
        SkillSelect();
    }
    private void AttackSpeed()
    {
        Debug.Log("���ý��ǵ�");
        SkillSelect();
    }
    private void CriticalUp()
    {
        Debug.Log("ũ��Ƽ�þ�");
        SkillSelect();
    }
    private void HpUp()
    {
        Debug.Log("�����Ǿ�");
        SkillSelect();
    }
    private void DoubleAttack()
    {
        Debug.Log("�������");
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
