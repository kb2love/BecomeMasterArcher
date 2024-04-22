using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class ArrowMove : MonoBehaviour
{
    void Start()
    {
        Vector3 targetPosition = transform.position + transform.forward * 20f;
        targetPosition = transform.TransformDirection(targetPosition);
        transform.DOMove(targetPosition, 1f);
        transform.DOMove(targetPosition, 1f).OnComplete(() =>
        {
            gameObject.SetActive(false); ;
        });
    }

}
