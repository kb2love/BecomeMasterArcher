using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private PlayerData playerData; // 플레이어 데이터 스크립터블 오브젝트
    [SerializeField] private SoundData soundData; // 사운드 데이터 스크립터블 오브젝트
    private Animator animator; // 애니메이터 컴포넌트
    private TouchPad touchPad; // 터치패드 컴포넌트
    private AudioSource audioSource; // 오디오 소스 컴포넌트
    private GameData gameData; // 게임 데이터
    private Transform attackPos; // 공격 위치
    private LayerMask enemyLayer = 1 << 6; // 적 레이어
    [SerializeField] private Transform closeEnemy; // 가장 가까운 적

    void Start()
    {
        touchPad = GameObject.Find("TouchPad_Image").GetComponent<TouchPad>(); // 터치패드 컴포넌트 가져오기
        animator = transform.GetChild(0).GetComponent<Animator>(); // 자식 오브젝트의 애니메이터 컴포넌트 가져오기
        gameData = DataManager.dataInst.gameData; // 게임 데이터 가져오기
        animator.SetFloat("AttackSpeed", gameData.plAtcSpeed); // 애니메이터의 공격 속도 설정
        attackPos = GameObject.Find("AttackPos").transform; // 공격 위치 트랜스폼 가져오기
        audioSource = GetComponent<AudioSource>(); // 오디오 소스 컴포넌트 가져오기
        FindEnemy(); // 가장 가까운 적 찾기
    }

    void Update()
    {
        if (touchPad.buttonPress)
        {
            // 움직일 때는 공격하지 않음
            animator.SetBool("IsWalk", true); // 걷기 애니메이션 재생
        }
        else
        {
            if (closeEnemy != null)
            {
                // 가까운 적이 있을 경우 적을 바라보고 공격
                Quaternion rot = Quaternion.LookRotation((closeEnemy.position - transform.position).normalized);
                rot.x = rot.z = 0f; // 회전 축 고정
                transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * 10.0f); // 부드럽게 회전
                animator.SetBool("IsWalk", false); // 걷기 애니메이션 중지
                animator.SetTrigger("AttackTrigger"); // 공격 애니메이션 재생
            }
            else
            {
                animator.SetBool("IsWalk", true); // 걷기 애니메이션 재생
            }
        }
    }

    public void FindEnemy()
    {
        // 가장 가까운 적을 찾는 함수
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, 10f, transform.forward, 20f, enemyLayer);

        if (hits.Length > 0)
        {
            // 적이 있는 경우 거리에 따라 정렬하여 가장 가까운 적을 설정
            System.Array.Sort(hits, (x, y) => Vector3.Distance(transform.position, x.point).CompareTo(Vector3.Distance(transform.position, y.point)));
            closeEnemy = hits[0].transform;
        }
        else
        {
            closeEnemy = null; // 적이 없을 경우 null로 설정
        }
    }

    public void Attack()
    {
        if (ObjectPoolingManager.objInstance.GetArrow() != null)
        {
            if (!gameData.isDoubleAtc)
            {
                ArrowShot(); // 단일 공격
            }
            else
            {
                ArrowShot(); // 첫 번째 공격
                Sequence sequence = DOTween.Sequence();
                sequence.AppendInterval(0.15f) // 0.15초 후 두 번째 공격
                        .AppendCallback(() => ArrowShot());
            }
        }
    }

    void ArrowShot()
    {
        GameObject arrow = ObjectPoolingManager.objInstance.GetArrow(); // 화살 오브젝트 가져오기
        arrow.transform.position = attackPos.position; // 화살 위치 설정
        arrow.transform.rotation = attackPos.rotation; // 화살 회전 설정
        arrow.SetActive(true); // 화살 활성화
        audioSource.PlayOneShot(soundData.arrowClip); // 화살 발사 소리 재생
    }

    public void AttackSpeedUp()
    {
        // 공격 속도 업데이트
        animator.SetFloat("AttackSpeed", gameData.plAtcSpeed);
    }
}