using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private Transform playerTr;
    private float shakeDuration = 0.1f; // ��鸲 ���� �ð�
    private float shakeStrength = 0.1f; // ��鸲 ����
    private Vector3 originalPosition;
    private Tweener tweener;
    [SerializeField] private bool isShaking = false;
    void Start()
    {
        if(GameObject.Find("Player") != null)
        playerTr = GameObject.Find("Player").transform;
        // ī�޶��� �ʱ� ��ġ ����
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
        // Shake �Լ��� ȣ��� ������ ī�޶� ���� ��
        Debug.Log("Shake");
        isShaking = true;
        // tweener ����
        tweener = transform.DOShakePosition(shakeDuration, shakeStrength);
        // ������ ����
        DOTween.Sequence()
            .AppendInterval(0.25f)
            .AppendCallback(() => isShaking = false)
            .SetUpdate(false);
    }
}
