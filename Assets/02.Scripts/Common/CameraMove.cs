using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private Transform playerTr;
    private float shakeDuration = 0.1f; // 흔들림 지속 시간
    private float shakeStrength = 0.1f; // 흔들림 강도
    private Vector3 originalPosition;
    private Tweener tweener;
    [SerializeField] private bool isShaking = false;
    void Start()
    {
        if(GameObject.Find("Player") != null)
        playerTr = GameObject.Find("Player").transform;
        // 카메라의 초기 위치 저장
        originalPosition = transform.localPosition;
    }


    void Update()
    {
        if (playerTr != null && !isShaking)
        {
            tweener.Pause();
            Vector3 plTr = new Vector3(0, 5.1f, playerTr.position.z - 1.5f);
            transform.position = plTr;
        }
    }
    public void Shake()
    {
        // Shake 함수가 호출될 때마다 카메라를 흔들게 함
        Debug.Log("Shake");
        isShaking = true;
        // tweener 실행
        tweener = transform.DOShakePosition(shakeDuration, shakeStrength);
        // 시퀀스 생성
        DOTween.Sequence()
            .AppendInterval(0.25f)
            .AppendCallback(() => isShaking = false)
            .SetUpdate(false);
    }
}
