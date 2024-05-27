using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemyBossDragon : MonoBehaviour
{
    [SerializeField] EnemyData enemyData; // 적 데이터
    [SerializeField] SoundData soundData; // 사운드 데이터
    private EnemyBossDamage enemyDamage; // 적의 데미지 스크립트
    private Animator animator; // 애니메이터 컴포넌트
    private GameObject shield; // 방패 오브젝트
    private Transform plTr; // 플레이어 트랜스폼
    private AudioSource source; // 오디오 소스
    private int aniIdx = Animator.StringToHash("RandomAttackIdx"); // 랜덤 공격 애니메이션 인덱스 해시
    private int aniFlyIdx = Animator.StringToHash("RandomFlyAttackIdx"); // 랜덤 비행 공격 애니메이션 인덱스 해시
    private int randomAttackIdx = 0; // 랜덤 공격 인덱스
    private int atIdx = 0; // 공격 인덱스 비교용 임시 변수
    private int randomFlyAttackIdx = 0; // 랜덤 비행 공격 인덱스
    private int atFIdx = 0; // 비행 공격 인덱스 비교용 임시 변수
    private bool isFly = false; // 비행 여부 플래그
    private bool isNotFly = false; // 비행하지 않는 여부 플래그
    private Vector3 flyVec; // 비행 벡터
    private Tween currentTween; // 현재 트윈

    void Start()
    {
        animator = GetComponent<Animator>(); // 애니메이터 컴포넌트 가져오기
        enemyDamage = GetComponent<EnemyBossDamage>(); // EnemyBossDamage 컴포넌트 가져오기
        source = GetComponent<AudioSource>(); // 오디오 소스 컴포넌트 가져오기
        plTr = GameObject.Find("Player").transform; // 플레이어 트랜스폼 찾기
        randomFlyAttackIdx = Random.Range(0, 2); // 랜덤 비행 공격 인덱스 초기화
        shield = Instantiate(enemyData.bossShield, transform.position, Quaternion.identity); // 방패 생성
        shield.SetActive(false); // 방패 비활성화
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player") // 충돌한 객체가 플레이어인지 확인
        {
            other.GetComponent<PlayerMovement>().PlayerRecieveDamage(enemyData.bossDamage); // 플레이어에게 데미지 주기
        }
    }

    public void RandomAttack()
    {
        if (isFly) // 드래곤이 비행 중인지 확인
        {
            atIdx = randomAttackIdx; // 현재 랜덤 공격 인덱스 저장
            while (randomAttackIdx == atIdx) // 새로운 인덱스가 다를 때까지 반복
                randomAttackIdx = Random.Range(2, 5); // 새로운 랜덤 공격 인덱스 생성
        }
        else
        {
            if (isNotFly) // 드래곤이 비행 중이 아닌지 확인
            {
                atIdx = randomAttackIdx; // 현재 랜덤 공격 인덱스 저장
                while (randomAttackIdx == atIdx) // 새로운 인덱스가 다를 때까지 반복
                    randomAttackIdx = Random.Range(2, 5); // 새로운 랜덤 공격 인덱스 생성
            }
            else
            {
                atIdx = randomAttackIdx; // 현재 랜덤 공격 인덱스 저장
                while (randomAttackIdx == atIdx) // 새로운 인덱스가 다를 때까지 반복
                    randomAttackIdx = Random.Range(1, 5); // 새로운 랜덤 공격 인덱스 생성
            }
            if (randomAttackIdx == 0) // 새 공격 인덱스가 0인지 확인 (비행 시작)
            {
                animator.SetBool("IsFly", true); // 애니메이터에 IsFly를 true로 설정
                Debug.Log("IsFly 트루"); // 디버그 로그 출력
                isFly = true; // 비행 여부 플래그 설정
                isNotFly = false; // 비비행 여부 플래그 해제
            }
            else
            {
                isNotFly = true; // 비비행 여부 플래그 설정
            }
        }

        Debug.Log(randomAttackIdx); // 랜덤 공격 인덱스 디버그 로그 출력
        animator.SetInteger(aniIdx, randomAttackIdx); // 애니메이터에 랜덤 공격 인덱스 설정
    }

    //******************************************* 애니메이션에서 호출되는 메서드 ******************************************//
    public void DragonRun()
    {
        Vector3 dir = PlayerLookRotation(); // 플레이어 방향 계산
        currentTween = transform.DOMove(dir * 2.0f, 2f); // DOTween을 사용해 플레이어 방향으로 이동
    }

    public void DragonFlyForward()
    {
        Vector3 dir = PlayerLookRotation(); // 플레이어 방향 계산
        currentTween = transform.DOMove(dir * 2.0f, 2f); // DOTween을 사용해 앞으로 이동
    }

    public void DragonFly()
    {
        currentTween = transform.DOMove(transform.up * 1.0f, 2f); // DOTween을 사용해 위로 이동
    }

    public void DragonLand()
    {
        currentTween = transform.DOMove(transform.up * -1.0f, 2f); // DOTween을 사용해 아래로 이동
    }

    public void DefendOn()
    {
        enemyDamage.isDefand = true; // 방어 상태 설정
        shield.transform.position = transform.position; // 방패 위치 설정
        shield.SetActive(true); // 방패 활성화
    }

    public void DefendOff()
    {
        enemyDamage.isDefand = false; // 방어 상태 해제
        shield.SetActive(false); // 방패 비활성화
    }

    public void NormalAttack()
    {
        AttackOverlap(); // 일반 공격 처리
    }

    public void EnemyFireAttack()
    {
        if (isFly)
            flyVec = transform.position - transform.up * 1.0f; // 비행 중이면 위치 조정
        else
            flyVec = transform.position; // 비행 중이 아니면 현재 위치 사용

        source.PlayOneShot(soundData.boomClip); // 불 공격 소리 재생
        FireEff(); // 불 공격 효과 실행
        AttackOverlap(); // 공격 처리
    }

    public void Scream()
    {
        source.PlayOneShot(soundData.screamClip); // 비명 소리 재생
    }

    //********************************* 애니메이션에서 호출되는 메서드 내부 메서드 ********************************//
    private void AttackOverlap()
    {
        Collider[] overSh = Physics.OverlapSphere(flyVec * 1.0f, 2f, 1 << 3); // 지정된 반경 내에서 충돌 체크
        foreach (Collider col in overSh)
        {
            Debug.Log(col.gameObject.name); // 충돌한 객체 이름 디버그 로그 출력
            col.GetComponent<PlayerMovement>().PlayerRecieveDamage(enemyData.bossDamage); // 플레이어에게 데미지 주기
        }
    }

    public void RandomFlyAttack()
    {
        atFIdx = randomFlyAttackIdx; // 현재 랜덤 비행 공격 인덱스 저장
        while (randomFlyAttackIdx == atFIdx) // 새로운 인덱스가 다를 때까지 반복
            randomFlyAttackIdx = Random.Range(0, 3); // 새로운 랜덤 비행 공격 인덱스 생성

        Debug.Log(randomFlyAttackIdx); // 랜덤 비행 공격 인덱스 디버그 로그 출력
        animator.SetInteger(aniFlyIdx, randomFlyAttackIdx); // 애니메이터에 랜덤 비행 공격 인덱스 설정

        if (randomFlyAttackIdx == 2) // 비행 공격 인덱스가 2이면 착륙
        {
            animator.SetBool("IsFly", false); // 애니메이터에 IsFly를 false로 설정
            Debug.Log("IsFly 폴스"); // 디버그 로그 출력
        }
    }

    private void FireEff()
    {
        GameObject fireEff = Instantiate(enemyData.bossFireEff); // 불 효과 생성
        fireEff.transform.position = flyVec + transform.forward * 1.0f; // 불 효과 위치 설정
        fireEff.transform.rotation = transform.rotation; // 불 효과 회전 설정
        fireEff.SetActive(true); // 불 효과 활성화
        DOTween.Sequence() // DOTween 시퀀스 생성
            .AppendCallback(() => fireEff.SetActive(true)) // 불 효과 활성화
            .AppendInterval(1f) // 1초 대기
            .AppendCallback(() => fireEff.SetActive(false)) // 불 효과 비활성화
            .SetUpdate(true); // 실시간 업데이트 설정
    }

    private Vector3 PlayerLookRotation()
    {
        if (currentTween != null && currentTween.IsActive()) // 현재 트윈이 활성화 되어있는지 확인
        {
            currentTween.Kill(); // 현재 트윈 종료
        }

        Vector3 dir = (plTr.position - transform.position).normalized; // 플레이어 방향 계산
        Quaternion lookRotation = Quaternion.LookRotation(dir); // 플레이어를 향해 회전 생성
        lookRotation.z = lookRotation.x = 0; // z축과 x축 회전 고정
        currentTween = transform.DORotateQuaternion(lookRotation, 0.5f); // DOTween을 사용해 플레이어를 향해 회전
        return dir; // 방향 반환
    }
}