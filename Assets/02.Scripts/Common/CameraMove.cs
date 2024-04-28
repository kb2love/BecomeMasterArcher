using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private Transform playerTr;
    private float shakeDuration = 0.5f; // ��鸲 ���� �ð�
    private float shakeStrength = 0.5f; // ��鸲 ����
    private Vector3 originalPosition;

    void Start()
    {
        if(GameObject.Find("Player") != null)
        playerTr = GameObject.Find("Player").transform;
        // ī�޶��� �ʱ� ��ġ ����
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
        // Shake �Լ��� ȣ��� ������ ī�޶� ���� ��
        transform.DOShakePosition(shakeDuration, shakeStrength);
    }

    public void ShakeOnce()
    {
        // �� ���� ª�� ��鸮�� ȿ���� �ִ� �Լ�
        transform.DOShakePosition(0.3f, 0.2f, 10, 90f, false, false);
    }

    public void ShakeCustom()
    {
        // ����� ���� ��鸲�� ����� �Լ�
        transform.DOShakePosition(shakeDuration, shakeStrength, 10, 90f, false, false);
    }

    public void ResetPosition()
    {
        // ���� ��ġ�� ī�޶� �ǵ���
        transform.DOLocalMove(originalPosition, 0.5f);
    }
}
