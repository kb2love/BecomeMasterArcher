using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class ArrowCtrl : MonoBehaviour
{
    void Start()
    {
        Vector3 targetPosition = transform.position + transform.forward * 20f;
        transform.DOMove(targetPosition, 10f).OnComplete(() =>
        {
            gameObject.SetActive(false); ;
        });
    }
    public void ArrowDamage(float a_damage)
    {
        float damage = 20f;
        a_damage -= damage;
    }
}
