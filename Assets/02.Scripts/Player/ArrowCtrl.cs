using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using DG.Tweening;

public class ArrowCtrl : MonoBehaviour
{
    [SerializeField] PlayerData playerData;
    private Tweener tweener;
    void OnEnable()
    {
        if(tweener != null)
            tweener.Kill();
        tweener = transform.DOLocalMove(transform.forward * 10f, 2.5f).OnComplete(SetOff);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            float randomValue = Random.value;
            if(randomValue < playerData.plCritical)
            {
                other.GetComponent<EnemyDamage>().RecieveDamage(playerData.plDamage * 2);
                Debug.Log("크리티컬");
            }
            else
            {
                other.GetComponent<EnemyDamage>().RecieveDamage(playerData.plDamage);
                Debug.Log("일반공격");
            }
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
