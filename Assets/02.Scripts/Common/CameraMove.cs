using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private Transform playerTr;
    private float shakeDuration = 0.5f; // 흔들림 지속 시간
    private float shakeStrength = 0.5f; // 흔들림 강도
    private Vector3 originalPosition;

    void Start()
    {
        if(GameObject.Find("Player") != null)
        playerTr = GameObject.Find("Player").transform;
        // 카메라의 초기 위치 저장
        originalPosition = transform.localPosition;
    }


    void Update()
    {
        if(playerTr != null)
        {
            Vector3 plTr = new Vector3(0, 5.1f, playerTr.position.z - 1.5f);
            transform.position = plTr;
        }
    }
    public void Shake()
    {
        // Shake 함수가 호출될 때마다 카메라를 흔들게 함
        transform.DOShakePosition(shakeDuration, shakeStrength);
    }

    public void ShakeOnce()
    {
        // 한 번만 짧게 흔들리는 효과를 주는 함수
        transform.DOShakePosition(0.3f, 0.2f, 10, 90f, false, false);
    }

    public void ShakeCustom()
    {
        // 사용자 정의 흔들림을 만드는 함수
        transform.DOShakePosition(shakeDuration, shakeStrength, 10, 90f, false, false);
    }

    public void ResetPosition()
    {
        // 원래 위치로 카메라를 되돌림
        transform.DOLocalMove(originalPosition, 0.5f);
    }
}
