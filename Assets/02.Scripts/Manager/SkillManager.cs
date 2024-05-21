
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SkillManager : MonoBehaviour
{
    public static SkillManager skillInst; // SkillManager의 싱글톤 인스턴스
    [SerializeField] private SkillData skillData; // 스킬 데이터
    [SerializeField] private SoundData soundData; // 사운드 데이터
    [SerializeField] private Transform[] enemies; // 적의 Transform 배열
    [SerializeField] private List<GameObject> enemiesList; // 적의 GameObject 리스트
    private GameData gameData; // 게임 데이터
    private GameObject skillGost; // 스킬 상점
    private int enemyCount = 0; // 적의 수
    private TouchPad touchPadCom; // 터치패드 컴포넌트
    private GameObject skillPanel; // 스킬 패널
    private GameObject touchPad; // 터치패드
    private Image skillButtonIm01; // 첫 번째 스킬 버튼 이미지
    private Image skillButtonIm02; // 두 번째 스킬 버튼 이미지
    private int firstIndex; // 첫 번째 스킬 인덱스
    private int secondIndex; // 두 번째 스킬 인덱스
    private string playerName = "Player"; // 플레이어 이름 태그
    [SerializeField] private Sprite[] skillSprites; // 스킬 이미지 배열

    private void Awake()
    {
        skillInst = this; // 싱글톤 인스턴스 설정
    }

    void Start()
    {
        skillPanel = GameObject.Find("Canvas").transform.GetChild(1).gameObject; // 스킬 패널 객체 가져오기
        skillButtonIm01 = skillPanel.transform.GetChild(0).GetComponent<Image>(); // 첫 번째 스킬 버튼 이미지 컴포넌트 가져오기
        skillButtonIm02 = skillPanel.transform.GetChild(1).GetComponent<Image>(); // 두 번째 스킬 버튼 이미지 컴포넌트 가져오기
        touchPad = GameObject.Find("Canvas").transform.GetChild(0).gameObject; // 터치패드 객체 가져오기
        skillSprites = new Sprite[] { skillData.attackPower_Im, skillData.attackSpeed_Im, skillData.criticalUp_Im, skillData.hpUp_Im, skillData.doubleAttack_Im }; // 스킬 이미지 배열 초기화
        touchPadCom = GameObject.Find("TouchPad_Image").GetComponent<TouchPad>(); // 터치패드 컴포넌트 가져오기
        skillGost = GameObject.Find("SkillGost").gameObject; // 스킬 상점 객체 가져오기
        gameData = DataManager.dataInst.gameData; // 게임 데이터 가져오기

        if (GameObject.Find("Enemies") != null) // 적 객체가 존재하면
        {
            enemies = GameObject.Find("Enemies").GetComponentsInChildren<Transform>(); // 적의 Transform 배열 가져오기
            if (enemies.Length > 1) // 적이 하나 이상 있으면
            {
                for (int i = 1; i < enemies.Length; i++)
                {
                    enemiesList.Add(enemies[i].gameObject); // 적 리스트에 추가
                }
            }
            enemyCount = enemiesList.Count; // 적의 수 초기화
            if (enemyCount == 0) // 적이 없으면
            {
                skillGost.transform.DOJump(new Vector3(0, 0, 2), 0, 0, 0.5f); // 스킬 상점 점프 애니메이션 실행
            }
        }
        else // 적 객체가 없으면
        {
            skillGost.transform.DOJump(new Vector3(0, 0, 2), 0, 0, 0.5f); // 스킬 상점 점프 애니메이션 실행
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(playerName)) // 플레이어와 충돌하면
        {
            SkillStoreOpen(); // 스킬 상점 열기
        }
    }

    public void SkillStoreOpen()
    {
        touchPad.SetActive(false); // 터치패드 비활성화
        skillPanel.SetActive(true); // 스킬 패널 활성화
        touchPadCom.enabled = false; // 터치패드 컴포넌트 비활성화
        touchPadCom.buttonPress = false; // 터치패드 버튼 비활성화
        Time.timeScale = 0; // 시간 정지
        SoundManager.soundInst.GetComponent<AudioSource>().PlayOneShot(soundData.storeOpenClip); // 상점 오픈 사운드 재생

        if (!gameData.isDoubleAtc) // 이중 공격이 아닐 때
        {
            firstIndex = Random.Range(0, skillSprites.Length); // 첫 번째 스킬 인덱스 랜덤 설정
            secondIndex = Random.Range(0, skillSprites.Length); // 두 번째 스킬 인덱스 랜덤 설정
            while (firstIndex == secondIndex) // 두 인덱스가 같으면 다시 랜덤 설정
                secondIndex = Random.Range(0, skillSprites.Length);
        }
        else // 이중 공격일 때
        {
            firstIndex = Random.Range(0, skillSprites.Length);
            while (firstIndex == 4) // 이중 공격 스킬 제외
                firstIndex = Random.Range(0, skillSprites.Length);
            secondIndex = Random.Range(0, skillSprites.Length);
            while (firstIndex == secondIndex || secondIndex == 4) // 이중 공격 스킬 제외
                secondIndex = Random.Range(0, skillSprites.Length);
        }

        // 두 개의 스킬 버튼 이미지 설정
        skillButtonIm01.sprite = skillSprites[firstIndex];
        skillButtonIm02.sprite = skillSprites[secondIndex];
    }

    public void SkillButton01()
    {
        ApplySkill(firstIndex); // 첫 번째 스킬 적용
    }

    public void SkillButton02()
    {
        ApplySkill(secondIndex); // 두 번째 스킬 적용
    }

    private void ApplySkill(int index)
    {
        // 인덱스에 따라 스킬 적용
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
        gameData.plDamage *= 1.2f; // 공격력 증가
        Debug.Log(gameData.plDamage);
        SkillSelect(); // 스킬 선택 후 설정
    }

    private void AttackSpeed()
    {
        gameData.plAtcSpeed *= 1.2f; // 공격 속도 증가
        Debug.Log(gameData.plAtcSpeed);
        SkillSelect(); // 스킬 선택 후 설정
    }

    private void CriticalUp()
    {
        if (gameData.plCritical >= 0.5)
        {
            gameData.plCritical += 0.025f; // 크리티컬 확률 증가
        }
        else
        {
            gameData.plCritical *= 2f; // 크리티컬 확률 2배 증가
        }
        Debug.Log(gameData.plCritical);
        SkillSelect(); // 스킬 선택 후 설정
    }

    private void HpUp()
    {
        gameData.plHP *= 1.2f; // 체력 증가
        gameData.plMaxHP *= 1.2f; // 최대 체력 증가
        Debug.Log(gameData.plHP);
        SkillSelect(); // 스킬 선택 후 설정
    }

    private void DoubleAttack()
    {
        gameData.isDoubleAtc = true; // 이중 공격 활성화
        Debug.Log(gameData.isDoubleAtc);
        SkillSelect(); // 스킬 선택 후 설정
    }

    private void SkillSelect()
    {
        skillPanel.SetActive(false); // 스킬 패널 비활성화
        Time.timeScale = 1.0f; // 시간 재개
        touchPad.SetActive(true); // 터치패드 활성화
        touchPadCom.enabled = true; // 터치패드 컴포넌트 활성화
        GameObject skillGost = GameObject.Find("SkillStore").transform.GetChild(0).gameObject; 
        skillGost.SetActive(false); // 스킬 상점 비활성화
        GameObject Potal = GameObject.Find("Door").transform.GetChild(0).gameObject;
        Potal.SetActive(true); // 포탈 활성화
        SoundManager.soundInst.GetComponent<AudioSource>().PlayOneShot(soundData.skillSelect); // 스킬 선택 사운드 재생
        DataManager.dataInst.SaveData(); // 데이터 저장
    }

    public void EnemyDieCount()
    {
        enemyCount--; // 적의 수 감소
        if (enemyCount <= 0) // 적이 모두 죽으면
        {
            skillGost.transform.DOJump(new Vector3(0, 0, 2), 0, 0, 0.5f); // 스킬 상점 점프 애니메이션 실행
        }
    }
}