using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using DG.Tweening;

public class ArrowCtrl : MonoBehaviour
{
    [SerializeField] PlayerData playerData;
    [SerializeField] Transform atackPos;
    private Tweener tweener;
    void OnEnable()
    {
        atackPos = GameObject.Find("AttackPos").transform;
        if(tweener != null)
            tweener.Kill();
        tweener = transform.DOLocalMove(transform.forward * 10f, 2.5f).OnComplete(SetOff);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            other.GetComponent<EnemyDamage>().RecieveDamage(playerData.plDamage);
            gameObject.SetActive(false);
        }
    }
    void OnDisable()
    {
        transform.position = Vector3.zero;
    }
    private void SetOff()
    {
        gameObject.SetActive(false);
        transform.position = Vector3.zero;
    }
}
